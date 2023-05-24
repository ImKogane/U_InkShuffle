using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
        bool allTrue = CheckAllBooleansTrue();
        if (!allTrue)
        {
            if (positionsList.Count != 0)
            {
                card.GetComponent<Card>().Stats = GetComponent<SpriteLookup>().associatedScriptableObject;

                int randomIndex = Random.Range(0, positionsList.Count);
                if (positionsList[randomIndex].GetComponent<WaypointManager>().IsBusy != true)
                {
                    Vector3 randomPosition = positionsList[randomIndex].position;
                    GameObject placedCard = Instantiate(card, randomPosition, Quaternion.identity);
                    positionsList[randomIndex].GetComponent<WaypointManager>().IsBusy = true;

                    if(placedCard.GetComponent<Card>() != null) placedCard.GetComponent<Card>().actualCardSide = Card.cardSide.PLAYERCARD;
                    ClearDeckHand();
                }
                else
                {
                    SpawnCardAtRandomPosition();
                }
            }
        }
        else
        {
            return;
        }
    }

    private bool CheckAllBooleansTrue()
    {
        foreach (Transform t in positionsList)
        {
            if (!t.GetComponent<WaypointManager>().IsBusy)
            {
                return false;
            }
        }

        return true;
    }

    private void ClearDeckHand()
    {
        GameObject tempGameObject = GameObject.FindGameObjectWithTag("PlayerManager");
        PlayerBoard tempDeck = tempGameObject.GetComponent<PlayerBoard>();
        List<CardAttributes> tempList = tempDeck.cardsInHand;
        foreach (CardAttributes c in tempList)
        {
            if (c == GetComponent<SpriteLookup>().associatedScriptableObject)
            {
                tempList.RemoveAt(tempList.IndexOf(c));
                AutoCreateCarousel tempUI = FindObjectOfType<AutoCreateCarousel>();
                tempUI.UpdateCarousel();
            }
        }
    }
}
