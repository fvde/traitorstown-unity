using Traitorstown.src.http;
using UnityEngine;

public class GameService : MonoBehaviour {

    public void createNewGame()
    {
        StartCoroutine(HttpRequestService.Instance.createNewGame());
    }
}
