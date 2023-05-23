using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBoard : MonoBehaviour
{
    private TurnBasedSystem turnBasedSystem;
    [SerializeField] private Animator deck;


    public List<Card> cardsOnBoard;
    public List<CardAttributes> cardsInHand;
    public List<CardAttributes> cardsDeck;
    [SerializeField] private int lifePoint;

    // Start is called before the first frame update
    void Start()
    {
        turnBasedSystem = GameObject.Find("TurnManager").GetComponent<TurnBasedSystem>();

        ShuffleDeck(cardsDeck);
        GiveStarterCards();
    }

    // Update is called once per frame
    void Update()
    {

    }

    /// <summary>
    /// Draw card in deck for player hand
    /// </summary>
    public IEnumerator DrawDeckCard()
    {
        if(cardsDeck.Count > 0)
        {
            
            if (deck != null)
            {
                deck.Play(0);
                yield return new WaitForSeconds(1);
            }
            else
            {
                yield return new WaitForSeconds(0.1f);
            }

            CardAttributes drawingCard = cardsDeck[0];
            cardsInHand.Add(drawingCard);
            cardsDeck.RemoveAt(0);
            turnBasedSystem.SkipPhase();
        }
    }

    void ShuffleDeck<T>(List<T> list)
    {
        int count = list.Count;
        for (int i = 0; i < count - 1; i++)
        {
            int randomIndex = Random.Range(i, count);
            T temp = list[i];
            list[i] = list[randomIndex];
            list[randomIndex] = temp;
        }
    }

    private void GiveStarterCards()
    {
        for (int i = 1; i < 3; i++)
        {
            CardAttributes drawingCard = cardsDeck[0];
            cardsInHand.Add(drawingCard);
            cardsDeck.RemoveAt(0);
        }
    }
}
