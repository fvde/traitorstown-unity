using Assets.Project.src.model;
using System;
using System.Collections;
using System.Collections.Generic;
using Traitorstown.src;
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
        GameStorage.Instance.ResetGamestateEvent += HandleResetGamestateEvent;
    }

    private void HandleResetGamestateEvent(object sender, EventArgs e)
    {
        ClearOldMessages(0);
    }

    public void Send()
    {
        var textInput = TextInput.GetComponent<InputField>();
        if (String.IsNullOrEmpty(textInput.text))
        {
            return;
        }

        Debug.Log("Sending: " + textInput.text);
        StartCoroutine(GameService.Instance.SendMessageToAll(textInput.text));
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

        ClearOldMessages(Configuration.MAX_CHAT_MESSAGES);

        ScrollRect scrollComponent = ScrollRect.GetComponent<ScrollRect>();
        scrollComponent.verticalNormalizedPosition = 0.0f;
    }

    private void ClearOldMessages(int desiredRemainingMessages)
    {
        var children = new List<Text>();
        children.AddRange(ContentArea.GetComponentsInChildren<Text>());
        children.GetRange(0, Mathf.Max(0, children.Count - desiredRemainingMessages)).ForEach(text => Destroy(text.transform.gameObject));
    }

    void OnDestroy()
    {
        HttpRequestService.Instance.ServerSentEvent -= HandleNewServerSentEvent;
        GameStorage.Instance.ResetGamestateEvent -= HandleResetGamestateEvent;
    }
}
