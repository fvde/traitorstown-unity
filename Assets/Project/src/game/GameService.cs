using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Traitorstown.src.game;
using Traitorstown.src.game.state;
using Traitorstown.src.http;
using Traitorstown.src.http.representation;
using Traitorstown.src.model;
using UnityEngine;

public class GameService {

    private static GameService instance = new GameService();

    public static GameService Instance
    {
        get
        {
            return instance;
        }
    }

    public IEnumerator GetCurrentGame()
    {
        PlayerRequired();

        yield return HttpRequestService.Instance.GetCurrentGame(GameStorage.Instance.PlayerId.Value, game =>
        {
            GameStorage.Instance.Game = game;
            game.Players.FindAll(player => !GameObjectFactory.Instance.HasCreatedPlayer(player.Id))
                .ForEach(player => GameObjectFactory.Instance.SpawnPlayer(player));

            HttpRequestService.Instance.RegisterForServerSentEventsForGame(game.Id);

            Debug.Log("Found current game with id " + game.Id);
        });
    }

    public IEnumerator CreateNewGame()
    {
        yield return HttpRequestService.Instance.CreateNewGame(game =>
        {
            Debug.Log("Created game with id " + game.Id);
            GameStorage.Instance.OpenGames.Add(game);
        });
    }

    public IEnumerator GetOpenGames()
    {
        yield return HttpRequestService.Instance.GetOpenGames(games =>
        {
            Debug.Log("Found open games:");
            GameStorage.Instance.OpenGames = new List<Game>(games);
            GameStorage.Instance.OpenGames.Sort((a, b) => a.GetReadyPlayerCount().CompareTo(b.GetReadyPlayerCount()));
            games.ForEach(game => Debug.Log(game));
        });
    }

    public IEnumerator JoinGame(int gameId)
    {
        PlayerRequired();

        if (gameId == -1 && GameStorage.Instance.OpenGames.Count > 0)
        {
            gameId = GameStorage.Instance.OpenGames[0].Id;
        }

        yield return HttpRequestService.Instance.JoinGame(gameId, GameStorage.Instance.PlayerId.Value, game =>
        {
            GameStorage.Instance.Game = game;
            Debug.Log("Joined game with id " + game.Id);
        });
    }

    public IEnumerator LeaveGame()
    {
        PlayerRequired();
        GameRequired();

        yield return HttpRequestService.Instance.LeaveGame(GameStorage.Instance.GameId.Value, GameStorage.Instance.PlayerId.Value, game =>
        {
            GameStorage.Instance.Game = null;
            HttpRequestService.Instance.CloseServerSentEventsConnection();
            Debug.Log("Left game with id " + game.Id);
        });
    }

    public IEnumerator SetReady()
    {
        PlayerRequired();
        GameRequired();

        yield return HttpRequestService.Instance.SetReady(GameStorage.Instance.GameId.Value, GameStorage.Instance.PlayerId.Value, true, game =>
        {
            Debug.Log("Set ready to start game with id " + game.Id);
        });
    }

    public IEnumerator SetNotReady()
    {
        PlayerRequired();
        GameRequired();

        yield return HttpRequestService.Instance.SetReady(GameStorage.Instance.GameId.Value, GameStorage.Instance.PlayerId.Value, false, game =>
        {
            Debug.Log("Not ready to start game with id " + game.Id);
        });
    }

    public IEnumerator GetCards()
    {
        PlayerRequired();
        GameRequired();

        yield return HttpRequestService.Instance.GetCards(GameStorage.Instance.GameId.Value, GameStorage.Instance.PlayerId.Value, cards =>
        {
            var newCards = mergeNewCardsWithCurrent(cards);
            foreach (Card card in newCards)
            {
                GameObjectFactory.Instance.SpawnCard(card);
                GameStorage.Instance.Cards.Add(card);
            }
            cards.ForEach(card => Debug.Log(card.Name));
        });
    }

    public IEnumerator GetCurrentTurn()
    {
        PlayerRequired();
        GameRequired();

        yield return HttpRequestService.Instance.GetTurn(GameStorage.Instance.GameId.Value, GameStorage.Instance.Game.Turn, turn =>
        {
            Debug.Log("Game's current turn is " + turn.Counter);
        });
    }

    public IEnumerator PlayCard(int cardId, int targetPlayerId)
    {
        PlayerRequired();
        GameRequired();
        CardRequired(cardId);

        yield return HttpRequestService.Instance.PlayCard(GameStorage.Instance.GameId.Value, GameStorage.Instance.Game.Turn, cardId, targetPlayerId, () =>
        {
            Card card = GameStorage.Instance.Cards.Find(c => c.Id == cardId);
            Debug.Log("Played card with id " + card.Id + ", "+ card.Name + "targeting player " + targetPlayerId);
            GameStorage.Instance.Cards.Remove(card);
            GameObjectFactory.Instance.DestroyOneCardWithId(card.Id);
        });
    }

    public IEnumerator SendMessageToAll(string content)
    {
        PlayerRequired();
        GameRequired();

        yield return HttpRequestService.Instance.SendMessage(GameStorage.Instance.GameId.Value, GameStorage.Instance.Game.Players.ConvertAll(p => p.Id), content, () =>
        {
            Debug.Log("Send message " + content);
        });
    }

    public IEnumerator SendMessageToPlayer(string content, int playerId)
    {
        PlayerRequired();
        GameRequired();

        yield return HttpRequestService.Instance.SendMessage(GameStorage.Instance.GameId.Value, new List<int> { playerId }, content, () =>
        {
            Debug.Log("Send message " + content);
        });
    }

    public void EndGame()
    {
        HttpRequestService.Instance.CloseServerSentEventsConnection();
    }

    private void PlayerRequired()
    {
        if (!GameStorage.Instance.PlayerId.HasValue)
        {
            throw new Exception("Player required. Login or Register.");
        }
    }

    private void GameRequired()
    {
        if (!GameStorage.Instance.GameId.HasValue)
        {
            throw new Exception("Game required. Join a game!");
        }
    }

    private void CardRequired(int cardId)
    {
        if (cardId == -1 && GameStorage.Instance.Cards.Count == 0)
        {
            throw new Exception("Card required. Query cards first!");
        }
    }

    private List<Card> mergeNewCardsWithCurrent(List<Card> newCards)
    {
        List<Card> newCardsAfterMerge = new List<Card>();
        Dictionary<Card, int> newCardDictionary = new Dictionary<Card, int>();
        newCards.ForEach(card => newCardDictionary[card] = 0);
        newCards.ForEach(card => newCardDictionary[card]++);

        foreach (Card currentCard in GameStorage.Instance.Cards) 
        {
            if (newCardDictionary.ContainsKey(currentCard) && newCardDictionary[currentCard] > 0)
            {
                newCardDictionary[currentCard]--;
            }
        }

        foreach (Card newCard in newCardDictionary.Keys)
        {
            for (int amount = newCardDictionary[newCard]; amount > 0; amount--)
            {
                newCardsAfterMerge.Add(newCard);
            }
        }

        return newCardsAfterMerge;
    }
}
