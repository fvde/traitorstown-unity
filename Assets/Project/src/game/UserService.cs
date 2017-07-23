using System;
using System.Collections;
using System.Collections.Generic;
using Traitorstown.src.game;
using Traitorstown.src.http;
using Traitorstown.src.http.representation;
using Traitorstown.src.http.request;
using Traitorstown.src.model;
using UnityEngine;

public class UserService : MonoBehaviour {

    public void Register(string username)
    {
        StartCoroutine(HttpRequestService.Instance.Register(username, username + DateTime.Now.Millisecond, playerId =>
        {
            GameState.Instance.PlayerId = playerId;
            Debug.Log("Obtained player id " + playerId);
        }));
    }

    public void Login(string username)
    {
        StartCoroutine(HttpRequestService.Instance.Login(username, username, playerId =>
        {
            GameState.Instance.PlayerId = playerId;
            Debug.Log("Logged in with player " + playerId);
        }));
    }
}
