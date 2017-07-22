﻿using System;
using Traitorstown.src.http;
using Traitorstown.src.http.representation;
using Traitorstown.src.model;
using UnityEngine;

public class GameService : MonoBehaviour {

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
            games.ForEach(game => Debug.Log(game));
        }));
    }
}