using System;
using System.Collections.Generic;
using System.Linq;
using Traitorstown.src.game;
using Traitorstown.src.http;
using Traitorstown.src.http.representation;
using Traitorstown.src.model;
using UnityEngine;

public class GameService : MonoBehaviour {

    private List<Game> openGames = new List<Game>();
    public GameObjectFactory gameObjectfactory;

    public void GetCurrentGame()
    {
        PlayerRequired();

        GameState.Instance.GameId = null;
        StartCoroutine(HttpRequestService.Instance.GetCurrentGame(GameState.Instance.PlayerId.Value, game =>
        {
            GameState.Instance.GameId = game.Id;
            GameState.Instance.TurnCounter = game.Turn;

            foreach (Player p in game.Players.FindAll(player => !GameState.Instance.Players.Contains(player)))
            {
                gameObjectfactory.spawnPlayer(p);

            }
            GameState.Instance.Players = game.Players;

            GameState.Instance.Resources = new List<Resource>(game.Players.Find(p => p.Id == GameState.Instance.PlayerId).Resources);
            Debug.Log("Found current game with id " + game.Id);
        }));
    }

    public void CreateNewGame()
    {
        StartCoroutine(HttpRequestService.Instance.CreateNewGame(game =>
        {
            Debug.Log("Started game with id " + game.Id);
        }));
    }

    public void GetOpenGames()
    {
        StartCoroutine(HttpRequestService.Instance.GetOpenGames(games =>
        {
            Debug.Log("Found open games:");
            openGames = new List<Game>(games);
            games.ForEach(game => Debug.Log(game));
        }));
    }

    public void JoinGame(int gameId)
    {
        PlayerRequired();

        if (gameId == -1 && openGames.Count > 0)
        {
            gameId = openGames[0].Id;
        }

        StartCoroutine(HttpRequestService.Instance.JoinGame(gameId, GameState.Instance.PlayerId.Value, game =>
        {
            GameState.Instance.GameId = game.Id;
            Debug.Log("Joined game with id " + game.Id);
        }));
    }

    public void LeaveGame()
    {
        PlayerRequired();
        GameRequired();

        StartCoroutine(HttpRequestService.Instance.LeaveGame(GameState.Instance.GameId.Value, GameState.Instance.PlayerId.Value, game =>
        {
            GameState.Instance.GameId = null;
            Debug.Log("Left game with id " + game.Id);
        }));
    }

    public void SetReady()
    {
        PlayerRequired();
        GameRequired();

        StartCoroutine(HttpRequestService.Instance.SetReady(GameState.Instance.GameId.Value, GameState.Instance.PlayerId.Value, true, game =>
        {
            Debug.Log("Set ready to start game with id " + game.Id);
        }));
    }

    public void SetNotReady()
    {
        PlayerRequired();
        GameRequired();

        StartCoroutine(HttpRequestService.Instance.SetReady(GameState.Instance.GameId.Value, GameState.Instance.PlayerId.Value, false, game =>
        {
            Debug.Log("Not ready to start game with id " + game.Id);
        }));
    }

    public void GetCards()
    {
        PlayerRequired();
        GameRequired();

        StartCoroutine(HttpRequestService.Instance.GetCards(GameState.Instance.GameId.Value, GameState.Instance.PlayerId.Value, cards =>
        {
            var newCards = mergeNewCardsWithCurrent(cards);
            foreach (Card card in newCards)
            {
                gameObjectfactory.spawnCard(card);
                GameState.Instance.Cards.Add(card);
            }
            cards.ForEach(card => Debug.Log(card.Name));
        }));
    }

    public void GetCurrentTurn()
    {
        PlayerRequired();
        GameRequired();
        TurnRequired();

        StartCoroutine(HttpRequestService.Instance.GetTurn(GameState.Instance.GameId.Value, GameState.Instance.TurnCounter.Value, turn =>
        {
            GameState.Instance.TurnCounter = turn.Counter;
            Debug.Log("Game's current turn is " + turn.Counter);
        }));
    }

    public void PlayCard(int cardId, int targetPlayerId)
    {
        PlayerRequired();
        GameRequired();
        TurnRequired();
        CardRequired(cardId);

        StartCoroutine(HttpRequestService.Instance.PlayCard(GameState.Instance.GameId.Value, GameState.Instance.TurnCounter.Value, cardId, targetPlayerId, () =>
        {
            Card card = GameState.Instance.Cards.Find(c => c.Id == cardId);
            Debug.Log("Played card with id " + card.Id + ", "+ card.Name + "targeting player " + targetPlayerId);
            GameState.Instance.Cards.Remove(card);
            gameObjectfactory.destroyOneCardWithId(card.Id);
        }));
    }

    private void PlayerRequired()
    {
        if (!GameState.Instance.PlayerId.HasValue)
        {
            throw new Exception("Player required. Login or Register.");
        }
    }

    private void GameRequired()
    {
        if (!GameState.Instance.GameId.HasValue)
        {
            throw new Exception("Game required. Join a game!");
        }
    }

    private void TurnRequired()
    {
        if (!GameState.Instance.TurnCounter.HasValue)
        {
            throw new Exception("Turn required. Query game first!");
        }
    }

    public void CardRequired(int cardId)
    {
        if (cardId == -1 && GameState.Instance.Cards.Count == 0)
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

        foreach (Card currentCard in GameState.Instance.Cards) 
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
