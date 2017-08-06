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

    public void TargetWithCard(int cardId)
    {
        StartCoroutine(GameService.Instance.PlayCard(cardId, Id));
    }
}
