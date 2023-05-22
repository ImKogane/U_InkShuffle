using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour
{
    #region Variables

    [SerializeField]
    public CardAttributes Stats;
    private string _name;
    private Texture _image;
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
    public Texture Image
    {
        get => _image;
        set => _image = value;
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
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void Init(CardAttributes s)
    {
        _name = s._name;
        _image = s._image;
        _pv = s._pv;
        _attack = s._attack;
        _type = s._type;

        Debug.Log(_name);
        Debug.Log(_image);
        Debug.Log(_pv);
        Debug.Log(_attack);
        Debug.Log(_type);
    }

    #endregion
}
