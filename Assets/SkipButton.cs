using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkipButton : MonoBehaviour
{
    private TurnBasedSystem turnBasedSystem;
    private Animator buttonAnimaor;

    private void Start()
    {
        turnBasedSystem = GameObject.Find("TurnManager").GetComponent<TurnBasedSystem>();
        buttonAnimaor = gameObject.GetComponent<Animator>();
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
                if (hit.collider.gameObject.CompareTag("SkipButton"))
                {
                    if (Input.GetMouseButtonDown(0))
                    {
                        if(turnBasedSystem.canSkip)
                        {
                            buttonAnimaor.SetTrigger("Click");
                            turnBasedSystem.SkipPhase();
                        }
                        else
                        {

                        }
                    }
                }
            }
        }
    }
}
