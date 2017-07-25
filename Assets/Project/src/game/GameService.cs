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

    public EventHandler<Boolean> OperationFinished;
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

        GameStorage.Instance.GameId = null;
        yield return HttpRequestService.Instance.GetCurrentGame(GameStorage.Instance.PlayerId.Value, game =>
        {
            GameStorage.Instance.GameId = game.Id;
            GameStorage.Instance.TurnCounter = game.Turn;

            foreach (Player p in game.Players.FindAll(player => !GameStorage.Instance.Players.Contains(player)))
            {
                GameObjectFactory.Instance.spawnPlayer(p);

            }
            GameStorage.Instance.Players = game.Players;

            GameStorage.Instance.Resources = new List<Resource>(game.Players.Find(p => p.Id == GameStorage.Instance.PlayerId).Resources);
            Debug.Log("Found current game with id " + game.Id);
            GameStorage.Instance.NotifyListeners();
        });
    }

    public IEnumerator CreateNewGame()
    {
        yield return HttpRequestService.Instance.CreateNewGame(game =>
        {
            Debug.Log("Created game with id " + game.Id);
            GameStorage.Instance.OpenGames.Add(game);
            GameStorage.Instance.NotifyListeners();
        });
    }

    public IEnumerator GetOpenGames()
    {
        yield return HttpRequestService.Instance.GetOpenGames(games =>
        {
            Debug.Log("Found open games:");
            GameStorage.Instance.OpenGames = new List<Game>(games);
            games.ForEach(game => Debug.Log(game));
            GameStorage.Instance.NotifyListeners();
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
            GameStorage.Instance.GameId = game.Id;
            Debug.Log("Joined game with id " + game.Id);
            GameStorage.Instance.NotifyListeners();
        });
    }

    public IEnumerator LeaveGame()
    {
        PlayerRequired();
        GameRequired();

        yield return HttpRequestService.Instance.LeaveGame(GameStorage.Instance.GameId.Value, GameStorage.Instance.PlayerId.Value, game =>
        {
            GameStorage.Instance.GameId = null;
            Debug.Log("Left game with id " + game.Id);
            GameStorage.Instance.NotifyListeners();
        });
    }

    public IEnumerator SetReady()
    {
        PlayerRequired();
        GameRequired();

        yield return HttpRequestService.Instance.SetReady(GameStorage.Instance.GameId.Value, GameStorage.Instance.PlayerId.Value, true, game =>
        {
            Debug.Log("Set ready to start game with id " + game.Id);
            GameStorage.Instance.NotifyListeners();
        });
    }

    public IEnumerator SetNotReady()
    {
        PlayerRequired();
        GameRequired();

        yield return HttpRequestService.Instance.SetReady(GameStorage.Instance.GameId.Value, GameStorage.Instance.PlayerId.Value, false, game =>
        {
            Debug.Log("Not ready to start game with id " + game.Id);
            GameStorage.Instance.NotifyListeners();
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
                GameObjectFactory.Instance.spawnCard(card);
                GameStorage.Instance.Cards.Add(card);
            }
            cards.ForEach(card => Debug.Log(card.Name));
            GameStorage.Instance.NotifyListeners();
        });
    }

    public IEnumerator GetCurrentTurn()
    {
        PlayerRequired();
        GameRequired();
        TurnRequired();

        yield return HttpRequestService.Instance.GetTurn(GameStorage.Instance.GameId.Value, GameStorage.Instance.TurnCounter.Value, turn =>
        {
            GameStorage.Instance.TurnCounter = turn.Counter;
            Debug.Log("Game's current turn is " + turn.Counter);
            GameStorage.Instance.NotifyListeners();
        });
    }

    public IEnumerator PlayCard(int cardId, int targetPlayerId)
    {
        PlayerRequired();
        GameRequired();
        TurnRequired();
        CardRequired(cardId);

        yield return HttpRequestService.Instance.PlayCard(GameStorage.Instance.GameId.Value, GameStorage.Instance.TurnCounter.Value, cardId, targetPlayerId, () =>
        {
            Card card = GameStorage.Instance.Cards.Find(c => c.Id == cardId);
            Debug.Log("Played card with id " + card.Id + ", "+ card.Name + "targeting player " + targetPlayerId);
            GameStorage.Instance.Cards.Remove(card);
            GameObjectFactory.Instance.destroyOneCardWithId(card.Id);
            GameStorage.Instance.NotifyListeners();
        });
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

    private void TurnRequired()
    {
        if (!GameStorage.Instance.TurnCounter.HasValue)
        {
            throw new Exception("Turn required. Query game first!");
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
