using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerBoard : MonoBehaviour
{
    private TurnBasedSystem turnBasedSystem;

    
    [SerializeField] private AutoCreateCarousel PlayerUI;

    public bool inAttackPhase;

    public List<Card> cardsOnBoard;
    public List<CardAttributes> cardsInHand;
    public List<CardAttributes> cardsDeck;

    [Header("Life system")]
    [SerializeField] private int lifePoint;
    [SerializeField] private TextMeshPro lifeText;

    [Header("Deck")]
    [SerializeField] private Animator deckAnimator;
    [SerializeField] private GameObject deck;



    // Start is called before the first frame update
    void Start()
    {
        turnBasedSystem = GameObject.Find("TurnManager").GetComponent<TurnBasedSystem>();

        if (lifeText != null) lifeText.text = lifePoint.ToString();

        ShuffleDeck(cardsDeck);
        GiveStarterCards();
    }

    public IEnumerator DrawDeckCard()
    {
        if(cardsDeck.Count > 0)
        {
            
            if (deckAnimator != null)
            {
                deckAnimator.SetTrigger("Draw");
                yield return new WaitForSeconds(0.7f);
            }
            else
            {
                yield return new WaitForSeconds(0.1f);
            }

            cardsInHand.Add(cardsDeck[0]);

            cardsDeck.RemoveAt(0);

            if(PlayerUI != null)
            {
                PlayerUI.UpdateCarousel();
            }

            if (cardsDeck.Count <= 0 && deck != null)
            {
                deck.SetActive(false);
            }
        }
        StartCoroutine(turnBasedSystem.FreePhase());
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

        if (PlayerUI != null)
        {
            PlayerUI.UpdateCarousel();
        }
    }

    public void TakeDamage(int amount)
    {
        lifePoint -= amount;
        if(lifeText != null) lifeText.text = lifePoint.ToString();

        if(lifePoint <= 0)
        {
            turnBasedSystem.UpdateText("Defeat");
        }
    }
}
