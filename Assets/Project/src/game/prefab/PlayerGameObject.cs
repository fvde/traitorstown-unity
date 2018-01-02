using System.Collections;
using System.Collections.Generic;
using Traitorstown.src.game;
using Traitorstown.src.model;
using UnityEngine;
using UnityEngine.UI;

public class PlayerGameObject : MonoBehaviour {
    public int PlayerId;
    public GameObject ContentArea;
    public GameObject TextElement;

    public void Initialize(Player player)
    {
        PlayerId = player.Id;
    }

    void Start()
    {
        Text[] texts = GetComponentsInChildren<Text>();
        texts[0].text = "Player " + PlayerId;
        GameStorage.Instance.GameUpdated += HandleGameUpdated;
    }

    private void HandleGameUpdated(object sender, System.EventArgs e)
    {
        Player player = GameStorage.Instance.Players.Find(p => p.Id == PlayerId);
        if (player != null)
        {
            SetActiveEffects(player.ActiveEffects);
        }
    }

    public void TargetWithCard(int cardId, string uuid)
    {
        StartCoroutine(GameService.Instance.PlayCard(cardId, PlayerId, uuid));
    }

    public void SetActiveEffects(List<Effect> effects)
    {
        foreach (Transform child in ContentArea.transform)
        {
            Destroy(child.gameObject);
        }

        foreach (Effect effect in effects)
        {
            GameObject text = Instantiate(TextElement);
            Text textComponent = text.GetComponent<Text>();
            textComponent.text = effect.Name + (effect.RemainingTurns < 1000 ? " (" + effect.RemainingTurns + ")" : "");
            text.transform.SetParent(ContentArea.transform);
        }
    }

    void OnDestroy()
    {
        GameStorage.Instance.GameUpdated -= HandleGameUpdated;
    }
}
