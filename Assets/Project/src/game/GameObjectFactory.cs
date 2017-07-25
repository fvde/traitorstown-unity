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

    public void destroyAll()
    {
        createdObjects.ForEach(pair => Destroy(pair.Value));
        createdObjects.Clear();
    }

    public void spawnPlayer(Player p)
    {
        GameObject player = Instantiate(playerType);
        PlayerGameObject playerGameObject = player.GetComponent<PlayerGameObject>();
        playerGameObject.Initialize(p.Id);
        player.transform.SetParent(playerArea.transform);

        createdObjects.Add(new KeyValuePair<int, GameObject>(p.Id, player));
    }

    public void spawnCard(Card c)
    {
        GameObject card = Instantiate(cardType);
        CardGameObject cardGameObject = card.GetComponent<CardGameObject>();
        cardGameObject.Initialize(c.Id, c.Name, c.Description, c.Costs);

        card.transform.SetParent(handArea.transform);

        createdObjects.Add(new KeyValuePair<int, GameObject>(c.Id, card));
    }

    public void destroyOneCardWithId(int id)
    {
        int toBeRemoved = createdObjects.FindIndex(pair => pair.Key == id);
        Destroy(createdObjects[toBeRemoved].Value);
    }
}
