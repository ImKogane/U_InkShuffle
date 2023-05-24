using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

public class TurnBasedSystem : MonoBehaviour
{

    [SerializeField] private enum turnPhase { START, DRAW, FREE, ATTACK, END};
    [SerializeField] private enum playerTurn { PLAYER1, PLAYER2 };

    private playerTurn actualPlayerTurn;
    private turnPhase actualPhase;
    [SerializeField] private int turnNumber = 0;

    [SerializeField] private IAPlayer gameAI;

    [SerializeField] public bool playerCanAttack;
    [SerializeField] public bool playerCanPutCard;


    [Header("Player boards")]
    [SerializeField] private PlayerBoard Player1Board;
    [SerializeField] private PlayerBoard Player2Board;

    private void Start()
    {
        StartNewTurn();
    }

    void Update()
    {
        if (Input.GetKeyDown("space"))
        {
            if (actualPlayerTurn == playerTurn.PLAYER1)
            {
                SkipPhase();
            }
        }
    }


    public void SkipPhase()
    {
        switch (actualPhase)
        {
            case turnPhase.DRAW:
                FreePhase();
                break;
            case turnPhase.FREE:
                AttackPhase();
                break;
            case turnPhase.ATTACK:
                EndAttackPhase();
                break;
        }
    }

    public void StartNewTurn()
    {
        turnNumber++;
        actualPhase = turnPhase.START;
        actualPlayerTurn = playerTurn.PLAYER1;

        ResetCardAttack();

        Debug.Log("New turn ("+turnNumber+")");

        DrawPhase();
    }

    public void DrawPhase()
    {
        actualPhase = turnPhase.DRAW;
        Debug.Log(actualPhase + " phase - " + actualPlayerTurn);

        switch (actualPlayerTurn)
        {
            case playerTurn.PLAYER1:

                StartCoroutine(Player1Board.DrawDeckCard());


                break;
            case playerTurn.PLAYER2:

                StartCoroutine(Player2Board.DrawDeckCard());

                break;
        }

        
    }

    public void FreePhase()
    {
        actualPhase = turnPhase.FREE;
        Debug.Log(actualPhase + " phase - " + actualPlayerTurn);

        switch (actualPlayerTurn)
        {
            case playerTurn.PLAYER1:

                playerCanPutCard = true;

                break;
            case playerTurn.PLAYER2:

                gameAI.PutCard();
                AttackPhase();

                break;
        }

        
    }
    
    public void AttackPhase()
    {
        actualPhase = turnPhase.ATTACK;
        Debug.Log(actualPhase + " phase - " + actualPlayerTurn);

        switch (actualPlayerTurn)
        {
            case playerTurn.PLAYER1:

                playerCanAttack = true;
                playerCanPutCard = false;

                break;
            case playerTurn.PLAYER2:

                gameAI.Attack();
                EndAttackPhase();
                break;
        }

        
    }

    public void EndAttackPhase()
    {
        switch (actualPlayerTurn)
        {
            case playerTurn.PLAYER1:
                playerCanAttack = false;
                NewPlayerPhase();
                break;
            case playerTurn.PLAYER2:
                EndTurn();
                break;
        }
    }

    public void EndTurn()
    { 
        actualPhase = turnPhase.END;
        StartNewTurn();
    }


    private void NewPlayerPhase()
    {
        switch (actualPlayerTurn)
        {
            case playerTurn.PLAYER1:
                actualPlayerTurn = playerTurn.PLAYER2;
                DrawPhase();
                break;
            case playerTurn.PLAYER2:
                StartNewTurn();
                break;
        }
    }

    private void ResetCardAttack()
    {
        Debug.Log("Reset card attack");
        foreach (Card card in Player1Board.cardsOnBoard)
        {
            card.canAttack = true;
        }
        foreach (Card card in Player2Board.cardsOnBoard)
        {
            card.canAttack = true;
        }

    }

}
