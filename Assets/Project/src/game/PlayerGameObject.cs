using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerGameObject : MonoBehaviour {
    public int Id;

    public void Initialize(int id)
    {
        Id = id;
    }

    void Start()
    {
        Text[] texts = GetComponentsInChildren<Text>();
        texts[0].text = "Player " + Id;
    }

    public void targetWithCard(int cardId)
    {
        GameObject gameService = GameObject.FindWithTag("GameService");
        gameService.GetComponent<GameService>().PlayCard(cardId, Id);
    }
}
