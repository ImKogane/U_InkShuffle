using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnBasedSystem : MonoBehaviour
{

    [SerializeField] private enum turnPhase { START, DRAW, FREE, ATTACK, END};
    [SerializeField] private enum playerTurn { PLAYER1, PLAYER2};

    private playerTurn actualPlayerTurn;
    [SerializeField] private turnPhase actualPhase;
    [SerializeField] private int turnNumber = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartNewTurn()
    {
        turnNumber++;
        actualPhase = turnPhase.START;
        actualPlayerTurn = playerTurn.PLAYER1;

        DrawPhase();
    }

    public void DrawPhase()
    {
        switch (actualPlayerTurn)
        {
            case playerTurn.PLAYER1:

                //Pioche une carte au joueur

                break;
            case playerTurn.PLAYER2:

                //Pioche une carte à l'IA

                break;
        }

        FreePhase();
    }

    public void FreePhase()
    {
        switch (actualPlayerTurn)
        {
            case playerTurn.PLAYER1:

                //Débloque les actions du joueur pour deposer une carte
                
                break;
            case playerTurn.PLAYER2:

                //Action de l'IA de poser une carte

                break;
        }

        AttackPhase()
    }
    
    public void AttackPhase()
    {
        switch (actualPlayerTurn)
        {
            case playerTurn.PLAYER1:

                //Debloque les actions d'attaque

                NewPlayerPhase();
                break;
            case playerTurn.PLAYER2:

                //Attaque de l'IA

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
