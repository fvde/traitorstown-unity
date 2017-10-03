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
    public Day day;

    // Use this for initialization
    void Start () {
		
	}

    void Update()
    {
        playerId = GameStorage.Instance.PlayerId.HasValue ? GameStorage.Instance.PlayerId : null;
        gameId = GameStorage.Instance.GameId.HasValue ? GameStorage.Instance.GameId : null;

        if (GameStorage.Instance.Game != null)
        {
            turnId = GameStorage.Instance.Game.Turn;
            day = turnId % 7 == 0 ? Day.SUNDAY : (Day)(turnId % 7);
        }
    }

    void OnGUI()
    {
        int elementOffset = 20;
        int sectionOffset = 20;
        int sectionCounter = 0;
        int elementCounter = 0;
        int normalHeight = 100;
        int width = 400;

        GUI.Label(new Rect(10, elementCounter++ * elementOffset + sectionCounter * sectionOffset, width, normalHeight), "You are player: " + playerId);
        GUI.Label(new Rect(10, elementCounter++ * elementOffset + sectionCounter * sectionOffset, width, normalHeight), "Game Id: " + gameId);
        GUI.Label(new Rect(10, elementCounter++ * elementOffset + sectionCounter * sectionOffset, width, normalHeight), "Turn: " + turnId);
        GUI.Label(new Rect(10, elementCounter++ * elementOffset + sectionCounter * sectionOffset, width, normalHeight), "Today is " + day);
        GUI.Label(new Rect(10, elementCounter++ * elementOffset + sectionCounter * sectionOffset, width, normalHeight), "You have:");
        foreach (Resource r in GameStorage.Instance.Resources)
        {
            GUI.Label(new Rect(10, elementCounter++ * elementOffset + sectionCounter * sectionOffset, width, normalHeight), r.Type.ToString() + ": " + r.Amount);
        }
    }
}
