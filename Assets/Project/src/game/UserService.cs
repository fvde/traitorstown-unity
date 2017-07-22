﻿using System;
using System.Collections;
using System.Collections.Generic;
using Traitorstown.src.http;
using Traitorstown.src.http.representation;
using Traitorstown.src.http.request;
using Traitorstown.src.model;
using UnityEngine;

public class UserService : MonoBehaviour {

    public void register(string username)
    {
        StartCoroutine(HttpRequestService.Instance.register(username, username + DateTime.Now.Millisecond, player =>
        {
            Debug.Log("Obtained player id " + player.Id);
        }));
    }

    public void login(string username)
    {
        StartCoroutine(HttpRequestService.Instance.login(username, username, player =>
        {
            Debug.Log("Logged in with player " + player.Id);
        }));
    }
}
