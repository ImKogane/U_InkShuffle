using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardClick : MonoBehaviour
{
    Card originAttack;
    Card target;

    private TurnBasedSystem turnBasedSystem;
    [SerializeField] private Image cardPreview;

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
                            cardPreview.enabled = true;
                            cardPreview.sprite = tempCard.Stats._fullImage;
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

                                        if(enemyBoard != null && enemyBoard.cardsOnBoard.Count <= 0)
                                        {
                                            tempCard.PlayAnimAttack();
                                            enemyBoard.TakeDamage(originAttack.Stats._attack);

                                            //Reset
                                            originAttack.canAttack = false;
                                            target = null;
                                            originAttack = null;
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
                                        
                                    }
                                    
                                }
                                else
                                {
                                    Debug.Log("Tu attaque la m�me carte !");
                                    ResetAttack();
                                }

                            }
                        }
                    }
                }
                else
                {
                    tempCard = null;
                    cardPreview.enabled = false;
                }
            }
        }
    }

    private void ResetAttack()
    {
        tempCard = null;
        originAttack = null;
        target = null;
    }
}

