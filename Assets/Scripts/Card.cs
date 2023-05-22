using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour
{
    #region Variables

    [SerializeField]
    public CardAttributes Stats;
    public Vector3 CardSize;

    private string _name;
    private Material _cleanImage;
    private Texture _fullImage;
    private int _pv;
    private int _attack;
    public enum CardType { Normal, Special };
    private CardType _type;

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
    public Texture FullImage
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


    #endregion

    #region Functions

    // Start is called before the first frame update
    void Start()
    {
        Init(Stats);
        MeshAttributes();
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
    }

    private void ApplyDamage(Card c)
    {
        if(c != null)
        {
            c._pv -= _attack;
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

    #endregion
}
