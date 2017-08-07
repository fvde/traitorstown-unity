using System.Collections;
using System.Collections.Generic;
using Traitorstown.src.model;
using UnityEngine;
using UnityEngine.UI;

public class GameObjectFactory : MonoBehaviour {

    public GameObject playerType;
    public GameObject cardType;
    public GameObject handArea;
    public GameObject playerArea;
    private List<KeyValuePair<int, GameObject>> createdObjects = new List<KeyValuePair<int, GameObject>>();
    private Dictionary<int, GameObject> createdPlayers = new Dictionary<int, GameObject>();

    private static GameObjectFactory instance;

    public static GameObjectFactory Instance
    {
        get
        {
            if (instance == null)
            {
                instance = GameObject.FindWithTag("GameObjectFactory").GetComponent<GameObjectFactory>();
            }
            return instance;
        }
    }

    public void DestroyAll()
    {
        createdObjects.ForEach(pair => Destroy(pair.Value));
        foreach (GameObject o in createdPlayers.Values)
        {
            Destroy(o);
        }
        createdPlayers.Clear();
        createdObjects.Clear();
    }

    public void SpawnPlayer(Player p)
    {
        GameObject player = Instantiate(playerType);
        PlayerGameObject playerGameObject = player.GetComponent<PlayerGameObject>();
        playerGameObject.Initialize(p);
        player.transform.SetParent(playerArea.transform);

        createdPlayers.Add(p.Id, player);
    }

    public bool HasCreatedPlayer(int id)
    {
        return createdPlayers.ContainsKey(id);
    }

    public void SpawnCard(Card c)
    {
        GameObject card = Instantiate(cardType);
        CardGameObject cardGameObject = card.GetComponent<CardGameObject>();
        cardGameObject.Initialize(c.Id, c.Name, c.Description, c.Costs);

        card.transform.SetParent(handArea.transform);

        createdObjects.Add(new KeyValuePair<int, GameObject>(c.Id, card));
    }

    public void DestroyOneCardWithId(int id)
    {
        int toBeRemoved = createdObjects.FindIndex(pair => pair.Key == id);
        Destroy(createdObjects[toBeRemoved].Value);
        createdObjects.RemoveAt(toBeRemoved);
    }
}
