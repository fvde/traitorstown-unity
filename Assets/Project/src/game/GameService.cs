using System;
using System.Collections.Generic;
using Traitorstown.src.game;
using Traitorstown.src.http;
using Traitorstown.src.http.representation;
using Traitorstown.src.model;
using UnityEngine;

public class GameService : MonoBehaviour {

    private List<Game> openGames = new List<Game>();
    private List<Card> handCards = new List<Card>();

    public void getCurrentGame()
    {
        PlayerRequired();

        GameState.Instance.GameId = null;
        StartCoroutine(HttpRequestService.Instance.getCurrentGame(GameState.Instance.PlayerId.Value, game =>
        {
            GameState.Instance.GameId = game.Id;
            GameState.Instance.TurnCounter = game.Turn;
            Debug.Log("Found current game with id " + game.Id);
        }));
    }

    public void createNewGame()
    {
        StartCoroutine(HttpRequestService.Instance.createNewGame(game =>
        {
            Debug.Log("Started game with id " + game.Id);
        }));
    }

    public void getOpenGames()
    {
        StartCoroutine(HttpRequestService.Instance.getOpenGames(games =>
        {
            Debug.Log("Found open games:");
            openGames = new List<Game>(games);
            games.ForEach(game => Debug.Log(game));
        }));
    }

    public void joinGame(int gameId)
    {
        PlayerRequired();

        if (gameId == -1 && openGames.Count > 0)
        {
            gameId = openGames[0].Id;
        }

        StartCoroutine(HttpRequestService.Instance.joinGame(gameId, GameState.Instance.PlayerId.Value, game =>
        {
            GameState.Instance.GameId = game.Id;
            Debug.Log("Joined game with id " + game.Id);
        }));
    }

    public void leaveGame()
    {
        PlayerRequired();
        GameRequired();

        StartCoroutine(HttpRequestService.Instance.leaveGame(GameState.Instance.GameId.Value, GameState.Instance.PlayerId.Value, game =>
        {
            GameState.Instance.GameId = null;
            Debug.Log("Left game with id " + game.Id);
        }));
    }

    public void setReady()
    {
        PlayerRequired();
        GameRequired();

        StartCoroutine(HttpRequestService.Instance.setReady(GameState.Instance.GameId.Value, GameState.Instance.PlayerId.Value, true, game =>
        {
            Debug.Log("Set ready to start game with id " + game.Id);
        }));
    }

    public void setNotReady()
    {
        PlayerRequired();
        GameRequired();

        StartCoroutine(HttpRequestService.Instance.setReady(GameState.Instance.GameId.Value, GameState.Instance.PlayerId.Value, false, game =>
        {
            Debug.Log("Not ready to start game with id " + game.Id);
        }));
    }

    public void getCards()
    {
        PlayerRequired();
        GameRequired();

        StartCoroutine(HttpRequestService.Instance.getCards(GameState.Instance.GameId.Value, GameState.Instance.PlayerId.Value, cards =>
        {
            handCards = new List<Card>(cards);
            cards.ForEach(card => Debug.Log(card.Name));
        }));
    }

    public void getCurrentTurn()
    {
        PlayerRequired();
        GameRequired();
        TurnRequired();

        StartCoroutine(HttpRequestService.Instance.getTurn(GameState.Instance.GameId.Value, GameState.Instance.TurnCounter.Value, turn =>
        {
            GameState.Instance.TurnCounter = turn.Counter;
            Debug.Log("Game's current turn is " + turn.Counter);
        }));
    }

    public void playCard(int cardId)
    {
        PlayerRequired();
        GameRequired();
        TurnRequired();
        CardRequired(cardId);

        if (cardId == -1 && handCards.Count > 0)
        {
            cardId = handCards[0].Id;
        }

        StartCoroutine(HttpRequestService.Instance.playCard(GameState.Instance.GameId.Value, GameState.Instance.TurnCounter.Value, cardId, () =>
        {
            Debug.Log("Played card " + cardId);
            handCards.Remove(handCards.Find(card => card.Id == cardId));
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
        if (cardId == -1 && handCards.Count == 0)
        {
            throw new Exception("Card required. Query cards first!");
        }
    }
}
