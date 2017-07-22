using System;
using System.Collections;
using System.Collections.Generic;
using Traitorstown.src.http;
using Traitorstown.src.http.request;
using UnityEngine;

public class UserService : MonoBehaviour {

    public void register(string username)
    {
        StartCoroutine(HttpRequestService.Instance.register(username, username + DateTime.Now.Millisecond));
    }
}
