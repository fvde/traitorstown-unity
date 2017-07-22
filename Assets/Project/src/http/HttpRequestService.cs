using System.Collections;
using System.Collections.Generic;
using Traitorstown.src;
using UnityEngine;
using UnityEngine.Networking;

public class HttpRequestService : MonoBehaviour {

    public string token = ""; 

    public void createNewGame()
    {
        Debug.Log("Creating new game...");
        UnityWebRequest request = UnityWebRequest.Get(Configuration.API_URL + Endpoints.POST_GAME);
        StartCoroutine(makeRequest(request));
    }

    private IEnumerator makeRequest(UnityWebRequest request)
    {
        request.SetRequestHeader("token", token);

        yield return request.Send();

        if (request.isNetworkError)
        {
            Debug.Log("Error: " + request.error);
        }
        else
        {
            Debug.Log("Received " + request.downloadHandler.text);
        }
    }
}
