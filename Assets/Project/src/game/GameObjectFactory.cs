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
    private Dictionary<int, GameObject> createdPlayers = new Dictionary<int, GameObject>();
    private Dictionary<string, GameObject> createdCards = new Dictionary<string, GameObject>();

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

    public void Reset()
    {
        DestroyPlayers();
        DestroyCards();
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
        cardGameObject.Initialize(c.Id, System.Guid.NewGuid().ToString(), c.Name, c.Description, c.Costs);

        card.transform.SetParent(handArea.transform);

        createdCards.Add(cardGameObject.UUID, card);
    }

    public void DestroyPlayers()
    {
        foreach (GameObject o in createdPlayers.Values)
        {
            Destroy(o);
        }
        createdPlayers.Clear();
    }

    public void DestroyCards()
    {
        foreach (GameObject o in createdCards.Values)
        {
            Destroy(o);
        }
        createdCards.Clear();
    }

    public void DestroyCardWithUUID(string uuid)
    {
        Destroy(createdCards[uuid]);
        createdCards.Remove(uuid);
    }
}
