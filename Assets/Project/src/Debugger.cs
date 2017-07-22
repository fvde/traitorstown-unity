using System.Collections;
using System.Collections.Generic;
using Traitorstown.src.game;
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
        playerId = GameState.Instance.PlayerId.HasValue ? GameState.Instance.PlayerId : null;
        gameId = GameState.Instance.GameId.HasValue ? GameState.Instance.GameId : null;
        turnId = GameState.Instance.TurnCounter.HasValue ? GameState.Instance.TurnCounter : null;

    }

    void OnGUI()
    {
        GUI.Label(new Rect(0, 0, 100, 100), "PlayerId: " + playerId);
        GUI.Label(new Rect(0, 20, 100, 100), "GameId: " + gameId);
        GUI.Label(new Rect(0, 40, 100, 100), "Turn: " + turnId);
    }

    public void Reset()
    {
        GameState.Instance.Reset();
    }
}
