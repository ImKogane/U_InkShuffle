using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IAPlayer : MonoBehaviour
{

    [SerializeField] private PlayerBoard ownBoard;
    [SerializeField] private PlayerBoard playerBoard;

    private List<Transform> points = new List<Transform>();
    [SerializeField] private GameObject tempCard;

    private void Start()
    {
        GameObject[] waypoints = GameObject.FindGameObjectsWithTag("IACardPlaces");

        foreach (GameObject w in waypoints)
        {
            points.Add(w.gameObject.transform);
        }
    }

    public void Attack()
    {
        foreach (Card card in ownBoard.cardsOnBoard)
        {
            if(card.canAttack) 
            {
                if(playerBoard.cardsOnBoard.Count > 0)
                {
                    card.PlayAnimAttack();
                    Card randomTarget = playerBoard.cardsOnBoard[Random.Range(0, playerBoard.cardsOnBoard.Count)];
                    card.ApplyDamage(randomTarget, "PlayerManager");
                }
                else
                {
                    playerBoard.TakeDamage(card.Stats._attack);
                }
            }
        }  
    }

    public void PutCard()
    {
        int randCardIndex = Random.Range(0, ownBoard.cardsInHand.Count);

        bool allTrue = CheckAllBooleansTrue();
        if (!allTrue)
        {
            if (points.Count != 0)
            {
                tempCard.GetComponent<Card>().Stats = ownBoard.cardsInHand[randCardIndex];
                //GameObject card = ownBoard.cardsInHand[randCardIndex].

                int randomPosIndex = Random.Range(0, points.Count);
                if (points[randomPosIndex].GetComponent<WaypointManager>().IsBusy != true)
                {
                    Vector3 randomPosition = points[randomPosIndex].position;
                    Quaternion randomRot = points[randomPosIndex].rotation;
                    GameObject placedCard =  Instantiate(tempCard, randomPosition, randomRot);
                    points[randomPosIndex].GetComponent<WaypointManager>().IsBusy = true;
                    ownBoard.cardsInHand.RemoveAt(randCardIndex);

                    if (placedCard.GetComponent<Card>() != null)
                    {
                        placedCard.GetComponent<Card>().Placed = true;
                        placedCard.GetComponent<Card>().Side = Card.cardSide.AICard;
                        ownBoard.cardsOnBoard.Add(placedCard.GetComponent<Card>());
                        
                    }

                }
                else
                {
                    PutCard();
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
        foreach (Transform t in points)
        {
            if (!t.GetComponent<WaypointManager>().IsBusy)
            {
                return false;
            }
        }
        return true;
    }

}
