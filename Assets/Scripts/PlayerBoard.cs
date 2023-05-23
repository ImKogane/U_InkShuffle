using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBoard : MonoBehaviour
{
    private TurnBasedSystem turnBasedSystem;

    public List<Card> cardsOnBoard;
    public List<CardAttributes> cardsInHand;
    public List<CardAttributes> cardsDeck;
    [SerializeField] private int lifePoint;

    // Start is called before the first frame update
    void Start()
    {
        turnBasedSystem = GameObject.Find("TurnManager").GetComponent<TurnBasedSystem>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void DrawDeckCard()
    {
        CardAttributes drawingCard = cardsDeck[0];
        cardsInHand.Add(drawingCard);
        cardsDeck.RemoveAt(0);
        turnBasedSystem.SkipPhase();
    }
    
}
