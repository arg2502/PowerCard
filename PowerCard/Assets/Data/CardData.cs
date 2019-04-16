using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardData : ScriptableObject
{
    protected new string name;
    protected string effect;
    public string key;
    public Sprite image;

    public virtual void Init() { }
}
