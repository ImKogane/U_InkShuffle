using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using MoonSharp.Interpreter;
using TMPro;
using UnityEngine;

[MoonSharpUserData]
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
    [MoonSharpHidden] [SerializeField] public bool playerCanAttack;
    [MoonSharpHidden] [SerializeField] public bool playerCanPutCard;

    [Header("User interface")]
    [SerializeField] private TextMeshProUGUI animText;
    [SerializeField] private TextMeshPro turnCount;
    [SerializeField] private TextMeshPro phaseText;
    [SerializeField] private UI_EndScreen EndCanvas;
    private Animator animatorText;

    public PlayerBoard GetPlayer() => Player1Board;
    public PlayerBoard GetOpponent() => Player2Board;
    public string GetPhase() => actualPhase.ToString();
    public string GetTurn() => actualPlayerTurn.ToString();
    public int GetTurnNumber() => turnNumber;

    [MoonSharpHidden] public bool canSkip;

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

        if(actualPlayerTurn == playerTurn.PLAYER2)
        {
            canSkip = false;
        }
    }


    [MoonSharpHidden]
    public void SkipPhase()
    {
        if (actualPlayerTurn == playerTurn.PLAYER1)
        {
            switch (actualPhase)
            {
                case turnPhase.FREE:
                    StartCoroutine(AttackPhase());
                    break;
                case turnPhase.ATTACK:
                    EndAttackPhase();
                    break;
            }
        }
  
    }

    [MoonSharpHidden]
    public IEnumerator StartNewTurn()
    {
        turnNumber++;
        ResetMidText();
        actualPhase = turnPhase.START;
        actualPlayerTurn = playerTurn.PLAYER1;

        ResetCardAttack();

        StartCoroutine(UpdateText("YOUR TURN"));
        yield return new WaitForSeconds(2.1f);


        

        DrawPhase();
    }

    [MoonSharpHidden]
    public void DrawPhase()
    {
        actualPhase = turnPhase.DRAW;
        ModLinker.OnGameAction(ModLinker.GameAction.PhaseChanged, actualPhase.ToString());

        switch (actualPlayerTurn)
        {
            case playerTurn.PLAYER1:
                canSkip = false;

                UpdatePhaseText("DRAW");
                StartCoroutine(Player1Board.DrawDeckCard());


                break;
            case playerTurn.PLAYER2:

                StartCoroutine(Player2Board.DrawDeckCard());

                break;
        }

        
    }

    [MoonSharpHidden]
    public IEnumerator FreePhase()
    {
        actualPhase = turnPhase.FREE;

        switch (actualPlayerTurn)
        {
            case playerTurn.PLAYER1:

                StartCoroutine(UpdateText("FREE PHASE"));
                UpdatePhaseText("FREE");
                yield return new WaitForSeconds(2.1f);
                ModLinker.OnGameAction(ModLinker.GameAction.PhaseChanged, actualPhase.ToString());

                canSkip = true;
                playerCanPutCard = true;

                break;
            case playerTurn.PLAYER2:


                yield return new WaitForSeconds(0.5f);
                ModLinker.OnGameAction(ModLinker.GameAction.PhaseChanged, actualPhase.ToString());
              
                gameAI.PutCard();

                yield return new WaitForSeconds(0.5f);

                StartCoroutine(AttackPhase());

                

                break;
        }

        
    }
    
    [MoonSharpHidden]
    public IEnumerator AttackPhase()
    {
        actualPhase = turnPhase.ATTACK;

        switch (actualPlayerTurn)
        {
            case playerTurn.PLAYER1:
                canSkip = true;

                if (Player1Board.cardsOnBoard.Count > 0)
                {
                    StartCoroutine(UpdateText("ATTACK PHASE"));
                    UpdatePhaseText("ATTACK");
                    yield return new WaitForSeconds(2.1f);
                    ModLinker.OnGameAction(ModLinker.GameAction.PhaseChanged, actualPhase.ToString());

                    
                    playerCanAttack = true;
                    playerCanPutCard = false;
                }
                else
                {
                    yield return new WaitForSeconds(0.1f);

                    EndAttackPhase();
                }

                break;
            case playerTurn.PLAYER2:

                if (Player2Board.cardsOnBoard.Count > 0)
                {
                    yield return new WaitForSeconds(0.5f);
                    ModLinker.OnGameAction(ModLinker.GameAction.PhaseChanged, actualPhase.ToString());

                    gameAI.Attack();
                    EndAttackPhase();
                }
                else
                {
                    yield return new WaitForSeconds(0.1f);
                    EndAttackPhase();
                }

                break;
        }

        
    }

    [MoonSharpHidden]
    public void EndAttackPhase()
    {
        canSkip = false;

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

    [MoonSharpHidden]
    public void EndTurn()
    { 
        actualPhase = turnPhase.END;
        StartCoroutine(StartNewTurn());
    }


    private IEnumerator NewPlayerPhase()
    {
        
        canSkip = false;
        UpdatePhaseText("");

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

    [MoonSharpHidden]
    public IEnumerator UpdateText(string text)
    {
        animText.gameObject.SetActive(true);
        animText.text = text;
        animatorText.Play(0);

        yield return new WaitForSeconds(2.1f);

        animText.gameObject.SetActive(false);

        yield return null;
    }

    [MoonSharpHidden]
    public void UpdatePhaseText(string text)
    {
        if(phaseText != null)
            phaseText.text = text;

    }
    
    [MoonSharpHidden]
    public void UpdateMidText(string text)
    {
        if (turnCount != null)
            turnCount.text = text;

    }

    [MoonSharpHidden]
    public void ResetMidText()
    {
        if (turnCount != null)
            turnCount.text = turnNumber.ToString();

    }

    [MoonSharpHidden]
    public void WinGame()
    {
        if(EndCanvas != null)
        {
            Time.timeScale = 0; //Pause game

            EndCanvas.gameObject.SetActive(true);
            EndCanvas.ShowWin();
        }
        
    }

    [MoonSharpHidden]
    public void LoseGame()
    {
        Time.timeScale = 0; //Pause game
    }

}
