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
    [SerializeField] private Image cardPreview;
    [SerializeField] private TextMeshProUGUI lifePointPreview;
    [SerializeField] private TextMeshProUGUI atkPointPreview;

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
                            cardPreview.sprite = tempCard.Stats._fullImage;
                            lifePointPreview.text = tempCard.PV.ToString();
                            atkPointPreview.text = tempCard.Attack.ToString();
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
                    cardPreview.gameObject.SetActive(false);
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

