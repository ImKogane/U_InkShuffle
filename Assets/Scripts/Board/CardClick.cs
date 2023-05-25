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
    [SerializeField] private GenerateCard cardPreview;

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
            // Vérifie si l'objet cliqué possède un collider attaché
            if (hit.collider != null)
            {
                // Vérifie si l'objet cliqué est celui que vous souhaitez interagir
                if (hit.collider.gameObject.CompareTag("Card"))
                {
                    // L'objet 3D a été cliqué !
                        
                    tempCard = hit.collider.gameObject.GetComponent<Card>();
                   
                    if (tempCard != null)
                    {
                        if(cardPreview != null)
                        {
                            cardPreview.gameObject.SetActive(true);
                            cardPreview.GenerateCardImage(tempCard.Stats);
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
                                        Debug.Log("Cette carte ne peux pas/plus attaquer !");
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
                                    Debug.Log("Tu attaque la même carte !");
                                    ResetAttack();
                                }

                            }
                        }
                    }
                }
                else
                {
                    tempCard = null;
                    //cardPreview.gameObject.SetActive(false);

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

