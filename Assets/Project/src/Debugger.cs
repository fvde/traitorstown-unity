using System.Collections;
using System.Collections.Generic;
using Traitorstown.src.game;
using Traitorstown.src.model;
using UnityEngine;

public class Debugger : MonoBehaviour {

    public int? playerId;
    public int? gameId;
    public int? turnId;

    public GameObjectFactory gameObjectfactory;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        playerId = GameState.Instance.PlayerId.HasValue ? GameState.Instance.PlayerId : null;
        gameId = GameState.Instance.GameId.HasValue ? GameState.Instance.GameId : null;
        turnId = GameState.Instance.TurnCounter.HasValue ? GameState.Instance.TurnCounter : null;

    }

    void OnGUI()
    {
        int elementOffset = 20;
        int sectionOffset = 20;
        int sectionCounter = 0;
        int elementCounter = 0;
        int normalHeight = 100;
        int largeHeight = 300;
        int width = 400;

        GUI.Label(new Rect(10, elementCounter++ * elementOffset + sectionCounter * sectionOffset, width, normalHeight), "Lobby ---------------- ");
        GUI.Label(new Rect(10, elementCounter++ * elementOffset + sectionCounter * sectionOffset, width, normalHeight), "PlayerId: " + playerId);
        GUI.Label(new Rect(10, elementCounter++ * elementOffset + sectionCounter * sectionOffset, width, normalHeight), "GameId: " + gameId);
        GUI.Label(new Rect(10, elementCounter++ * elementOffset + sectionCounter * sectionOffset, width, normalHeight), "Turn: " + turnId);

        GUI.Label(new Rect(10, elementCounter++ * elementOffset + ++sectionCounter * sectionOffset, width, normalHeight), "Player ---------------- ");
        foreach (Player player in GameState.Instance.Players)
        {
            GUI.Label(new Rect(10, elementCounter++ * elementOffset + sectionCounter * sectionOffset, width, normalHeight), "Player: " + player.Id);
            foreach (Resource resource in player.Resources)
            {
                GUI.Label(new Rect(10, elementCounter++ * elementOffset + sectionCounter * sectionOffset, width, normalHeight), resource.Type.ToString() + " : " + resource.Amount);
            }
        }
    }

    public void Reset()
    {
        GameState.Instance.Reset();
        gameObjectfactory.destroyAll();
    }
}
