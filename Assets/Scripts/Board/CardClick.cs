using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CardClick : MonoBehaviour
{
    Card originAttack;
    Card target;

    private TurnBasedSystem turnBasedSystem;

    [Header("UI Preview")]
    [SerializeField] public GameObject cardPreview;

    private PlayerBoard enemyBoard;

    private Card tempCard;

    private void Start()
    {
        turnBasedSystem = GameObject.Find("TurnManager").GetComponent<TurnBasedSystem>();
        enemyBoard = GameObject.FindGameObjectWithTag("IAManager").GetComponent<PlayerBoard>();
    }

    void Update()
    {
        
        //Debug.Log("Click");
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            // V�rifie si l'objet cliqu� poss�de un collider attach�
            if (hit.collider != null)
            {
                // V�rifie si l'objet cliqu� est celui que vous souhaitez interagir
                if (hit.collider.gameObject.CompareTag("Card"))
                {
                    // L'objet 3D a �t� cliqu� !
                        
                    tempCard = hit.collider.gameObject.GetComponent<Card>();
                   
                    if (tempCard != null)
                    {
                        
                        if(cardPreview != null)
                        {
                            cardPreview.gameObject.SetActive(true);
                            cardPreview.GetComponent<GenerateCard>().GenerateCardImage(null, tempCard);
                        }
                        
                        if (Input.GetMouseButtonDown(0) && turnBasedSystem.playerCanAttack)
                        {
                            if (originAttack is null)
                            {
                                if (tempCard.Side == Card.cardSide.PlayerCard)
                                {
                                    if (tempCard.canAttack)
                                    {
                                        originAttack = tempCard;
                                        turnBasedSystem.UpdateMidText("SELECT TARGET");


                                        if (enemyBoard != null && enemyBoard.cardsOnBoard.Count <= 0)
                                        {
                                            tempCard.PlayAnimAttack();
                                            enemyBoard.TakeDamage(originAttack.Stats._attack);

                                            //Reset
                                            originAttack.canAttack = false;
                                            target = null;
                                            originAttack = null;
                                            turnBasedSystem.ResetMidText();
                                        }
                                        else
                                        {
                                            
                                        }
                                    }
                                    else
                                    {
                                        tempCard.PlayAnimError();
                                    }
                                }
                                
                            }
                            else
                            {
                                if (tempCard != originAttack)
                                {
                                    if(tempCard.Side == Card.cardSide.AICard)
                                    {
                                            originAttack.PlayAnimAttack();
                                            target = tempCard;
                                            originAttack.ApplyDamage(target, "IAManager");

                                            target = null;
                                            originAttack = null;
                                            turnBasedSystem.ResetMidText();


                                    }
                                }
                                else
                                {
                                    ResetAttack();
                                }
                            }
                        }
                    }
                }
                else
                {
                    if (tempCard != null)
                        cardPreview.gameObject.SetActive(false);
                    
                    tempCard = null;
                }
            }
        }
    }

    private void ResetAttack()
    {
        tempCard = null;
        originAttack = null;
        target = null;
        turnBasedSystem.ResetMidText();
    }
}

