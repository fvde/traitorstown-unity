using System;
using System.Collections;
using System.Collections.Generic;
using Traitorstown.src.game;
using Traitorstown.src.http;
using Traitorstown.src.http.representation;
using Traitorstown.src.http.request;
using Traitorstown.src.model;
using UnityEngine;

public class UserService {

    private static UserService instance = new UserService();

    public static UserService Instance
    {
        get
        {
            return instance;
        }
    }

    public IEnumerator Register(string username)
    {
        yield return HttpRequestService.Instance.Register(username + DateTime.Now.Millisecond, username + DateTime.Now.Millisecond, playerId =>
        {
            GameStorage.Instance.PlayerId = playerId;
            GameStorage.Instance.UserName = username;
            Debug.Log("Obtained player id " + playerId);
            GameStorage.Instance.NotifyListeners();
        });
    }

    public IEnumerator Login(string username)
    {
        yield return HttpRequestService.Instance.Login(username, username, playerId =>
        {
            GameStorage.Instance.PlayerId = playerId;
            Debug.Log("Logged in with player " + playerId);
            GameStorage.Instance.NotifyListeners();
        });
    }
}
