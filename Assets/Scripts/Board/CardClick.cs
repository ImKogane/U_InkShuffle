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

    private Card tempCard;

    private void Start()
    {
        turnBasedSystem = GameObject.Find("TurnManager").GetComponent<TurnBasedSystem>();
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
                            cardPreview.enabled = true;
                            cardPreview.sprite = tempCard.Stats._fullImage;
                        }


                        if (Input.GetMouseButtonDown(0) && turnBasedSystem.playerCanAttack)
                        {
                            if (originAttack is null)
                            {
                                originAttack = tempCard;
                                Debug.Log("Select target");
                            }
                            else
                            {
                                if (tempCard != originAttack)
                                {
                                    if(tempCard.Side == Card.cardSide.AICard)
                                    {
                                        target = tempCard;
                                        originAttack.ApplyDamage(target);

                                        target = null;
                                        originAttack = null;
                                    }
                                    else
                                    {
                                        Debug.Log("Tu t'attaque toi-même !");
                                        ResetAttack();
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

