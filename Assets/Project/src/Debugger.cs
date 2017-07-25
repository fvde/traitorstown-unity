using System.Collections;
using System.Collections.Generic;
using Traitorstown.src.game;
using Traitorstown.src.model;
using UnityEngine;

public class Debugger : MonoBehaviour {

    public int? playerId;
    public int? gameId;
    public int? turnId;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        playerId = GameStorage.Instance.PlayerId.HasValue ? GameStorage.Instance.PlayerId : null;
        gameId = GameStorage.Instance.GameId.HasValue ? GameStorage.Instance.GameId : null;
        turnId = GameStorage.Instance.TurnCounter.HasValue ? GameStorage.Instance.TurnCounter : null;

    }

    void OnGUI()
    {
        int elementOffset = 20;
        int sectionOffset = 20;
        int sectionCounter = 0;
        int elementCounter = 0;
        int normalHeight = 100;
        int width = 400;

        GUI.Label(new Rect(10, elementCounter++ * elementOffset + sectionCounter * sectionOffset, width, normalHeight), "Lobby ---------------- ");
        GUI.Label(new Rect(10, elementCounter++ * elementOffset + sectionCounter * sectionOffset, width, normalHeight), "PlayerId: " + playerId);
        GUI.Label(new Rect(10, elementCounter++ * elementOffset + sectionCounter * sectionOffset, width, normalHeight), "GameId: " + gameId);
        GUI.Label(new Rect(10, elementCounter++ * elementOffset + sectionCounter * sectionOffset, width, normalHeight), "Turn: " + turnId);
    }

    public void Reset()
    {
        GameStorage.Instance.Reset();
        GameObjectFactory.Instance.destroyAll();
    }

    public void ResetUser()
    {
        GameStorage.Instance.ResetUser();
    }
}
