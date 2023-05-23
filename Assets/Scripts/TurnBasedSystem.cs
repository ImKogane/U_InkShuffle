using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnBasedSystem : MonoBehaviour
{

    [SerializeField] private enum turnPhase { START, DRAW, FREE, ATTACK, END};
    [SerializeField] private enum playerTurn { PLAYER1, PLAYER2 };

    private playerTurn actualPlayerTurn;
    private turnPhase actualPhase;
    [SerializeField] private int turnNumber = 0;

    [SerializeField] private IAPlayer gameAI;


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

                Player1Board.DrawDeckCard();

                break;
            case playerTurn.PLAYER2:

                Player2Board.DrawDeckCard();
                FreePhase();

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

                //Débloque les actions du joueur pour deposer une carte
                
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

                //Debloque les actions d'attaque
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

}
