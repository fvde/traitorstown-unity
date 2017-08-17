using Assets.Project.src.model;
using System.Collections;
using System.Collections.Generic;
using Traitorstown.src.game;
using Traitorstown.src.http;
using Traitorstown.src.model;
using UnityEngine;
using UnityEngine.UI;

public class ChatGameObject : MonoBehaviour {

    public GameObject ScrollRect;
    public GameObject ContentArea;
    public GameObject TextInput;
    public GameObject ChatMessage;

    void Start()
    {
        HttpRequestService.Instance.ServerSentEvent += HandleNewServerSentEvent;
    }

    public void Send()
    {
        var textInput = TextInput.GetComponent<InputField>();
        Debug.Log("Sending: " + textInput.text);
        textInput.text = "";
    }

    private void HandleNewServerSentEvent(object sender, Traitorstown.src.http.representation.MessageRepresentation httpMessage)
    {
        Message m = httpMessage.toMessage();
        AddMessage(m.From, m.Content);
    }

    private void AddMessage(long playerId, string content)
    {
        GameObject text = Instantiate(ChatMessage);
        Text textComponent = text.GetComponent<Text>();
        textComponent.text = (playerId != -1 ? "Player " + playerId + ": " : "") + content;
        text.transform.SetParent(ContentArea.transform);

        ScrollRect scrollComponent = ScrollRect.GetComponent<ScrollRect>();
        scrollComponent.verticalNormalizedPosition = 0.0f;
    }

    void OnDestroy()
    {
        HttpRequestService.Instance.ServerSentEvent -= HandleNewServerSentEvent;
    }
}
