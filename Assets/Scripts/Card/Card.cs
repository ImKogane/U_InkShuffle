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
    public enum CardType { Normal, Special };
    public enum Rarity { Common, Rare, Epic};
    private CardType _type;
    private Rarity _rarity;

    [SerializeField] public enum cardSide { PLAYERCARD, AICARD };
    public cardSide actualCardSide;

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


    public void ApplyDamage(Card c)
    {
        if(c != null)
        {
            Debug.Log(_name + " attaque " + c.Stats.name);
            c._pv -= _attack;

            if(c._pv <= 0)
            {
                Destroy(c.gameObject);
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
    #endregion
}
