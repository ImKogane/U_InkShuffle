using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using TMPro;
using UnityEngine;

public class TurnBasedSystem : MonoBehaviour
{

    private enum turnPhase { START, DRAW, FREE, ATTACK, END};
    private enum playerTurn { PLAYER1, PLAYER2 };
    private playerTurn actualPlayerTurn;
    private turnPhase actualPhase;

    private int turnNumber = 0;

    [SerializeField] private IAPlayer gameAI;

    [Header("Player boards")]
    [SerializeField] private PlayerBoard Player1Board;
    [SerializeField] private PlayerBoard Player2Board;

    [Header("Player permissions")]
    [SerializeField] public bool playerCanAttack;
    [SerializeField] public bool playerCanPutCard;

    [Header("User interface")]
    [SerializeField] private TextMeshProUGUI animText;
    [SerializeField] private TextMeshPro turnCount;
    private Animator animatorText;

    private void Start()
    {
        if(animText != null)
        {
            animatorText = animText.gameObject.GetComponent<Animator>();
        }

        StartCoroutine(StartNewTurn());
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
                StartCoroutine(FreePhase());
                break;
            case turnPhase.FREE:
                StartCoroutine(AttackPhase());
                break;
            case turnPhase.ATTACK:
                EndAttackPhase();
                break;
        }
    }

    public IEnumerator StartNewTurn()
    {
        turnNumber++;
        if(turnCount != null) turnCount.text = turnNumber.ToString();
        actualPhase = turnPhase.START;
        actualPlayerTurn = playerTurn.PLAYER1;

        ResetCardAttack();

        StartCoroutine(UpdateText("YOUR TURN"));
        yield return new WaitForSeconds(2.1f);


        

        DrawPhase();
    }

    public void DrawPhase()
    {
        actualPhase = turnPhase.DRAW;

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

    public IEnumerator FreePhase()
    {
        actualPhase = turnPhase.FREE;

        switch (actualPlayerTurn)
        {
            case playerTurn.PLAYER1:

                StartCoroutine(UpdateText("FREE PHASE"));
                yield return new WaitForSeconds(2.1f);

                playerCanPutCard = true;

                break;
            case playerTurn.PLAYER2:

                yield return new WaitForSeconds(0.5f);

                gameAI.PutCard();
                StartCoroutine(AttackPhase());

                break;
        }

        
    }
    
    public IEnumerator AttackPhase()
    {
        actualPhase = turnPhase.ATTACK;


        switch (actualPlayerTurn)
        {
            case playerTurn.PLAYER1:

                if(Player1Board.cardsOnBoard.Count > 0)
                {
                    StartCoroutine(UpdateText("ATTACK PHASE"));
                    yield return new WaitForSeconds(2.1f);

                    playerCanAttack = true;
                    playerCanPutCard = false;
                }
                else
                {
                    yield return new WaitForSeconds(0.1f);
                    SkipPhase();
                }

                break;
            case playerTurn.PLAYER2:

                if (Player2Board.cardsOnBoard.Count > 0)
                {
                    yield return new WaitForSeconds(0.5f);

                    gameAI.Attack();
                    EndAttackPhase();
                }
                else
                {
                    yield return new WaitForSeconds(0.1f);
                    SkipPhase();
                }

                break;
        }

        
    }

    public void EndAttackPhase()
    {
        switch (actualPlayerTurn)
        {
            case playerTurn.PLAYER1:
                playerCanAttack = false;
                StartCoroutine(NewPlayerPhase());
                break;
            case playerTurn.PLAYER2:
                EndTurn();
                break;
        }
    }

    public void EndTurn()
    { 
        actualPhase = turnPhase.END;
        StartCoroutine(StartNewTurn());
    }


    private IEnumerator NewPlayerPhase()
    {
        switch (actualPlayerTurn)
        {
            case playerTurn.PLAYER1:

                actualPlayerTurn = playerTurn.PLAYER2;

                StartCoroutine(UpdateText("AI TURN"));

                yield return new WaitForSeconds(2.1f);

                DrawPhase();

                break;
            case playerTurn.PLAYER2:
                StartCoroutine(StartNewTurn());
                break;
        }
    }

    private void ResetCardAttack()
    {
        foreach (Card card in Player1Board.cardsOnBoard)
        {
            card.canAttack = true;
        }
        foreach (Card card in Player2Board.cardsOnBoard)
        {
            card.canAttack = true;
        }

    }

    public IEnumerator UpdateText(string text)
    {
        animText.gameObject.SetActive(true);
        animText.text = text;
        animatorText.Play(0);

        yield return new WaitForSeconds(2.1f);

        animText.gameObject.SetActive(false);

        yield return null;
    }

}
