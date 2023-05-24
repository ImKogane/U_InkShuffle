using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
                    if (placedCard.GetComponent<Card>() != null)
                    {
                        placedCard.GetComponent<Card>().Placed = true;
                        placedCard.GetComponent<Card>().Side = Card.cardSide.PlayerCard;
                    }
                    ClearDeckHand(placedCard);
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

    private void ClearDeckHand(GameObject g)
    {
        GameObject tempGameObject = GameObject.FindGameObjectWithTag("PlayerManager");
        PlayerBoard tempDeck = tempGameObject.GetComponent<PlayerBoard>();
        List<CardAttributes> tempListHand = tempDeck.cardsInHand;
        List<Card> tempListBoard = tempDeck.cardsOnBoard;
        if (g.GetComponent<Card>().Placed == true)
        {
            for (int i = 0; i < tempListHand.Count; ++i)
            {
                if (tempListHand[i] == GetComponent<SpriteLookup>().associatedScriptableObject)
                {
                    tempListHand.RemoveAt(i);
                    tempListBoard.Add(g.GetComponent<Card>());
                    AutoCreateCarousel tempUI = FindObjectOfType<AutoCreateCarousel>();
                    tempUI.UpdateCarousel();
                }
            }
        }
    }
}
