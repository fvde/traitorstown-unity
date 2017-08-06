using System;
using System.Collections;
using System.Collections.Generic;
using Traitorstown.src.game;
using Traitorstown.src.model;
using UnityEngine;

public class Debugger : MonoBehaviour {

    public int? playerId;
    public int? gameId;
    public int turnId;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	

    public void Reset()
    {
        GameStorage.Instance.Reset();
        GameObjectFactory.Instance.DestroyAll();
    }

    public void ResetUser()
    {
        GameStorage.Instance.ResetUser();
    }
}
