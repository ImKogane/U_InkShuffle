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


    public void StartNewTurn()
    {
        turnNumber++;
        actualPhase = turnPhase.START;
        actualPlayerTurn = playerTurn.PLAYER1;

        DrawPhase();
    }

    public void DrawPhase()
    {
        actualPhase = turnPhase.DRAW;

        switch (actualPlayerTurn)
        {
            case playerTurn.PLAYER1:

                Player1Board.DrawDeckCard();

                break;
            case playerTurn.PLAYER2:

                Player2Board.DrawDeckCard();

                break;
        }

        FreePhase();
    }

    public void FreePhase()
    {
        actualPhase = turnPhase.FREE;

        switch (actualPlayerTurn)
        {
            case playerTurn.PLAYER1:

                //Débloque les actions du joueur pour deposer une carte
                
                break;
            case playerTurn.PLAYER2:

                gameAI.PutCard();

                break;
        }

        AttackPhase();
    }
    
    public void AttackPhase()
    {
        actualPhase = turnPhase.ATTACK;

        switch (actualPlayerTurn)
        {
            case playerTurn.PLAYER1:

                //Debloque les actions d'attaque

                NewPlayerPhase();
                break;
            case playerTurn.PLAYER2:

                gameAI.Attack();

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
                break;
            case playerTurn.PLAYER2:
                actualPlayerTurn = playerTurn.PLAYER1;
                break;
        }
    }

}
