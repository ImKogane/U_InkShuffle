using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Card : MonoBehaviour
{
    #region Variables

    [SerializeField]
    public CardAttributes Stats;
    public Vector3 CardSize;

    private string _name;
    public string _imagePath;
    private int _pv;
    private int _attack;
    [SerializeField]
    private bool _isPlaced;
    public enum CardType { Normal, Special };
    public enum Rarity { Common, Rare, Epic};
    public enum cardSide { PlayerCard, AICard };

    private CardType _type;
    private Rarity _rarity;
    private cardSide _actualCardSide;

    public WaypointManager cardLocation;

    

    public bool canAttack;

    [SerializeField] private TextMeshProUGUI lifeText;
    [SerializeField] private TextMeshProUGUI atkText;

    #endregion

    #region Getter&Setter

    public string Name
    {
        get => _name;
        set => _name = value;
    }
    public int PV
    {
        get => _pv;
        set => _pv = value;
    }
    public int Attack
    {
        get => _attack;
        set => _attack = value;
    }

    public bool Placed
    {
        get => _isPlaced;
        set => _isPlaced = value;
    }

    public CardType Type
    {
        get => _type;
        set => _type = value;
    }
    public Rarity CardRarity
    {
        get => _rarity;
        set => _rarity = value;
    }

    public cardSide Side
    {
        get => _actualCardSide;
        set => _actualCardSide = value;
    }

    #endregion

    #region Functions

    // Start is called before the first frame update
    void Start()
    {
        Init(Stats);
        MeshAttributes();
        AdaptUI();


    }

    private void Init(CardAttributes s)
    {
        _name = s._name;
        _pv = s._pv;
        _attack = s._attack;
        _type = s._type;
        _rarity = s._rarity;
        _imagePath = s._imagePath;

        gameObject.GetComponent<GenerateCard>().GenerateCardImage(s, null);
    }


    public void ApplyDamage(Card target, string tag)
    {
        if(target != null)
        {
            target._pv -= _attack;
            canAttack = false;

            if (target._pv <= 0)
            {
                target.ClearBoard(target, tag);
            }
            else
            {
                target.AdaptUI();
            }
            
        }
    }

    private void MeshAttributes()
    {
        if(CardSize != null)
        {
            transform.localScale = CardSize;
        }
    }

    public void AdaptUI()
    {
        if (lifeText != null)
        {
            lifeText.text = _pv.ToString();
        }
        if (atkText != null)
        {
            atkText.text = _attack.ToString();
        }
    }

    private static GameObject FindGameObjectInChildWithTag(GameObject parent, string tag)
    {
        Transform t = parent.transform;

        for (int i = 0; i < t.childCount; i++)
        {
            if (t.GetChild(i).gameObject.tag == tag)
            {
                return t.GetChild(i).gameObject;
            }
        }
        return null;
    }

    private void ClearBoard(Card target, string tag)
    {
        GameObject tempGameObject = GameObject.FindGameObjectWithTag(tag);
        PlayerBoard tempDeck = tempGameObject.GetComponent<PlayerBoard>();
        List<Card> tempListBoard = tempDeck.cardsOnBoard;

        if (target.Placed == true)
        {
            for (int i = 0; i < tempListBoard.Count; ++i)
            {
                if (tempListBoard[i] == target)
                {
                    tempListBoard.RemoveAt(i);
                    target.cardLocation.IsBusy = false;
                    Destroy(target.gameObject);
                    break;
                }
            }
                
        }
    }

    


    #region Animations
    public void PlayAnimError()
        {
            Animator animator = GetComponent<Animator>();

            if(animator != null)
            {
                animator.SetTrigger("Error");
            }
        }

        public void PlayAnimAttack()
        {
            Animator animator = GetComponent<Animator>();

            if (animator != null && gameObject != null)
            {
                animator.SetTrigger("Attack");
            }
        }
        #endregion
    #endregion
}