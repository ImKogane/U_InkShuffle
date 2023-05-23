using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceCard : MonoBehaviour
{
    public GameObject card;
    public List<Transform> positionsList;

    private void Start()
    {
        GameObject[] waypoints = GameObject.FindGameObjectsWithTag("PlayerCardPlaces");
        foreach (GameObject w in waypoints) 
        {
            positionsList.Add(w.gameObject.transform);
        }
    }

    public void SpawnCardAtRandomPosition()
    {
        if (positionsList.Count != 0)
        {
            card.GetComponent<Card>().Stats = GetComponent<SpriteLookup>().associatedScriptableObject;
            int randomIndex = Random.Range(0, positionsList.Count);
            Vector3 randomPosition = positionsList[randomIndex].position;
            Instantiate(card, randomPosition, Quaternion.identity);
        }
    }
}
