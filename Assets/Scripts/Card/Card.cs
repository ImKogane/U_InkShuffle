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
    private Material _cleanImage;
    private Sprite _fullImage;
    private int _pv;
    private int _attack;
    private bool _isPlaced;
    public enum CardType { Normal, Special };
    public enum Rarity { Common, Rare, Epic};
    public enum cardSide { PlayerCard, AICard };

    private CardType _type;
    private Rarity _rarity;
    private cardSide _actualCardSide;

    public bool canAttack;

    #endregion

    #region Getter&Setter

    public string Name
    {
        get => _name;
        set => _name = value;
    }
    public Material CleanImage
    {
        get => _cleanImage;
        set => _cleanImage = value;
    }
    public Sprite FullImage
    {
        get => _fullImage;
        set => _fullImage = value;
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

    // Update is called once per frame
    void Update()
    {

    }

    private void Init(CardAttributes s)
    {
        _name = s._name;
        _cleanImage = s._cleanImage;
        _fullImage = s._fullImage;
        _pv = s._pv;
        _attack = s._attack;
        _type = s._type;
        _rarity = s._rarity;
    }


    public void ApplyDamage(Card c, string tag)
    {
        if(c != null)
        {
            Debug.Log(_name + " attaque " + c.Stats.name);
            c._pv -= _attack;
            canAttack = false;

            if(c._pv <= 0)
            {
                ClearBoard(c, tag);
            }
            else
            {
                c.AdaptUI();
            }
            
        }
    }

    private void MeshAttributes()
    {
        if(_cleanImage != null)
        {
            GetComponent<Renderer>().material = _cleanImage;
        }
        if(CardSize != null)
        {
            transform.localScale = CardSize;
        }
    }

    public void AdaptUI()
    {
        GameObject LifeText = FindGameObjectInChildWithTag(this.gameObject, "LifePoints");
        if (LifeText != null)
        {
            LifeText.GetComponent<TMP_Text>().text = _pv.ToString();
        }
        GameObject AttackText = FindGameObjectInChildWithTag(this.gameObject, "AttackPoints");
        if (AttackText != null)
        {
            AttackText.GetComponent<TMP_Text>().text = _attack.ToString();
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

    private void ClearBoard(Card card, string tag)
    {
        GameObject tempGameObject = GameObject.FindGameObjectWithTag(tag);
        PlayerBoard tempDeck = tempGameObject.GetComponent<PlayerBoard>();
        List<Card> tempListBoard = tempDeck.cardsOnBoard;
        if (card.Placed == true)
        {
            for (int i = 0; i < tempListBoard.Count; ++i)
            {
                if (tempListBoard[i] == card)
                {
                    Debug.Log("Identiques");
                    tempListBoard.RemoveAt(i);
                    Destroy(tempListBoard[i].gameObject);
                }
                else
                {
                    Debug.Log("Différents");
                }
            }
                
        }
    }
    #endregion
}