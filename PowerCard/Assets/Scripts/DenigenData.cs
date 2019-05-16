﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Denigen", menuName = "Cards/Denigen")]
public class DenigenData : CardData
{    
    public int Stars { get; set; }
    public enum Rarity { COMMON, UNCOMMON, RARE, JUDGE }
    Rarity rarity;
    public int Atk { get; set; }
    public int Sh { get; set; }
    public enum Type
    {
        //ASTRAL, FIRE, WATER, GRASS, ELECTRIC,
        //ICE, ROCK, POISON, PSYCHIC, MYSTERY,
        //LIGHT, DARK, DRAGON, FLYING, PLATNUM,
        //PARADOX, RAINBOW
        FIRE, METAL, WOOD, EARTH, ELECTRIC, WATER,
        SPIRIT, VOID, STAR, PSYCHIC,
        ASCENDANT, SEEKER
    }
    public List<Type> Types { get; set; }
    public List<Type> Weaknesses { get; set; }
    public List<Type> Resistances { get; set; }
    public int Str {
        get
        {
            switch (Stars)
            {
                case 1: return 5;
                case 2: return 7;
                case 3: return 9;
                case 4: return 12;
                case 5: return 15;
                default: return 0;
            }
        }
    }

    Dictionary<Type, Type> TypeWeaknesses = new Dictionary<Type, Type>()
    {
        //{Type.ASTRAL, new List<Type>(){Type.POISON, Type.MYSTERY } },
        //{Type.FIRE, new List<Type>(){Type.WATER, Type.FLYING } },
        //{Type.WATER, new List<Type>(){Type.ELECTRIC, Type.GRASS} },
        //{Type.GRASS, new List<Type>(){Type.FIRE, Type.FLYING} },
        //{Type.ELECTRIC, new List<Type>(){Type.PLATNUM, Type.ROCK } },
        //{Type.ICE, new List<Type>(){Type.POISON, Type.FIRE } },
        //{Type.ROCK, new List<Type>(){Type.GRASS, Type.WATER} },
        //{Type.POISON, new List<Type>(){Type.DARK, Type.PSYCHIC} },
        //{Type.PSYCHIC, new List<Type>(){Type.MYSTERY, Type.ASTRAL} },
        //{Type.MYSTERY, new List<Type>(){Type.DARK, Type.LIGHT} },
        //{Type.LIGHT, new List<Type>(){Type.DRAGON, Type.PSYCHIC} },
        //{Type.DARK, new List<Type>(){Type.LIGHT, Type.DRAGON} },
        //{Type.DRAGON, new List<Type>(){Type.ICE, Type.PLATNUM} },
        //{Type.FLYING, new List<Type>(){Type.ELECTRIC, Type.ROCK} },
        //{Type.PLATNUM, new List<Type>(){Type.ICE, Type.ASTRAL} }
        {Type.FIRE, Type.WATER},
        {Type.METAL, Type.FIRE },
        {Type.WOOD, Type.METAL },
        {Type.EARTH, Type.WOOD },
        {Type.ELECTRIC, Type.EARTH },
        {Type.WATER, Type.ELECTRIC },
        {Type.SPIRIT, Type.PSYCHIC },
        {Type.VOID, Type.SPIRIT },
        {Type.STAR, Type.VOID },
        {Type.PSYCHIC, Type.STAR },
        {Type.ASCENDANT, Type.ASCENDANT }
    };

    Dictionary<Type, Type> TypeResistances = new Dictionary<Type, Type>()
    {
        //{Type.ASTRAL, new List<Type>(){Type.PSYCHIC, Type.PLATNUM } },
        //{Type.FIRE, new List<Type>(){Type.GRASS, Type.ICE } },
        //{Type.WATER, new List<Type>(){Type.FIRE, Type.ROCK} },
        //{Type.GRASS, new List<Type>(){Type.ROCK, Type.WATER} },
        //{Type.ELECTRIC, new List<Type>(){Type.FLYING, Type.WATER } },
        //{Type.ICE, new List<Type>(){Type.DRAGON, Type.PLATNUM } },
        //{Type.ROCK, new List<Type>(){Type.FLYING, Type.ELECTRIC} },
        //{Type.POISON, new List<Type>(){Type.ASTRAL, Type.ICE} },
        //{Type.PSYCHIC, new List<Type>(){Type.LIGHT, Type.POISON} },
        //{Type.MYSTERY, new List<Type>(){Type.PSYCHIC, Type.ASTRAL} },
        //{Type.LIGHT, new List<Type>(){Type.DARK, Type.MYSTERY} },
        //{Type.DARK, new List<Type>(){Type.MYSTERY, Type.POISON} },
        //{Type.DRAGON, new List<Type>(){Type.DARK, Type.LIGHT} },
        //{Type.FLYING, new List<Type>(){Type.FIRE, Type.GRASS} },
        //{Type.PLATNUM, new List<Type>(){Type.DRAGON, Type.ELECTRIC} }
        {Type.FIRE, Type.METAL },
        {Type.METAL, Type.WOOD },
        {Type.WOOD, Type.EARTH },
        {Type.EARTH, Type.ELECTRIC },
        {Type.ELECTRIC, Type.WATER },
        {Type.WATER, Type.FIRE },
        {Type.SPIRIT, Type.VOID },
        {Type.VOID, Type.STAR },
        {Type.STAR, Type.PSYCHIC },
        {Type.PSYCHIC, Type.SPIRIT }
    };
    public override void Init()
    {
        DecipherSpreadsheet();
        FillWeaknessesAndResistances();
    }

    void DecipherSpreadsheet()
    {
        // find the row that matches the key
        if(GameControl.control.denigenKeys.ContainsKey(key))
        {
            var data = GameControl.control.denigenKeys[key];

            // key, name, stars, type, atk, sh, effect

            // 0 is key -- ignore
            // add name
            name = data[1];
            Stars = int.Parse(data[2]);
            Types = DecipherTypes(data[3]);
            Atk = int.Parse(data[4]);
            Sh = int.Parse(data[5]);
            effect = data[6];

        }
    }

    List<Type> DecipherTypes(string typedata)
    {
        // split data by '/' in case of dual type
        var typesArray = typedata.Split(' ');

        List<Type> myTypes = new List<Type>();
        foreach(var t in typesArray)
        {
            switch(t)
            {
                //case "AST": myTypes.Add(Type.ASTRAL); break;
                //case "FIR": myTypes.Add(Type.FIRE); break;
                //case "WAT": myTypes.Add(Type.WATER); break;
                //case "GRS": myTypes.Add(Type.GRASS); break;
                //case "ETR": myTypes.Add(Type.ELECTRIC); break;
                //case "ICE": myTypes.Add(Type.ICE); break;
                //case "RCK": myTypes.Add(Type.ROCK); break;
                //case "PSN": myTypes.Add(Type.POISON); break;
                //case "PSY": myTypes.Add(Type.PSYCHIC); break;
                //case "MYS": myTypes.Add(Type.MYSTERY); break;
                //case "LGT": myTypes.Add(Type.LIGHT); break;
                //case "DRK": myTypes.Add(Type.DARK); break;
                //case "DRG": myTypes.Add(Type.DRAGON); break;
                //case "FLY": myTypes.Add(Type.FLYING); break;
                //case "PLT": myTypes.Add(Type.PLATNUM); break;
                //case "PDX": myTypes.Add(Type.PARADOX); break;
                //case "RNB": myTypes.Add(Type.RAINBOW); break;
                case "Fire": myTypes.Add(Type.FIRE); break;
                case "Metal": myTypes.Add(Type.METAL); break;
                case "Wood": myTypes.Add(Type.WOOD); break;
                case "Earth": myTypes.Add(Type.EARTH); break;
                case "Electric": myTypes.Add(Type.ELECTRIC); break;
                case "Water": myTypes.Add(Type.WATER); break;
                case "Spirit": myTypes.Add(Type.SPIRIT); break;
                case "Void": myTypes.Add(Type.VOID); break;
                case "Star": myTypes.Add(Type.STAR); break;
                case "Psychic": myTypes.Add(Type.PSYCHIC); break;
                case "Ascendant": myTypes.Add(Type.ASCENDANT); break;
                case "Seeker": myTypes.Add(Type.SEEKER); break;

            }
        }
        return myTypes;
    }

    public void FillWeaknessesAndResistances()
    {
        Weaknesses = new List<Type>();
        Resistances = new List<Type>();

        foreach (var t in Types)
        {
            // make sure it's normal type -- Paradox and Rainbow will be special cases
            if (TypeWeaknesses.ContainsKey(t))
            {
                //foreach (var w in TypeWeaknesses[t])
                //{
                    // do not add if the weakness is the same as the denigen's types
                    // do not add if we already have the weakness added
                    //if (!Types.Contains(w) && !Weaknesses.Contains(w))
                        Weaknesses.Add(TypeWeaknesses[t]);
                //}
            }
            if (TypeResistances.ContainsKey(t))
            {
                //foreach (var r in TypeResistances[t])
                //{
                    // do not add if we already have the resistance added
                    //if (!Resistances.Contains(r))
                        Resistances.Add(TypeResistances[t]);
                //}
            }
        }

        // remove weakness if its the same type as the denigen
        //foreach(var t in Types)
        //{
        //    if (Weaknesses.Contains(t))
        //        Weaknesses.Remove(t);
        //}

        //// remove any weakness that is also in resistance
        //foreach (var r in Resistances)
        //{
        //    if (Weaknesses.Contains(r))
        //        Weaknesses.Remove(r);
        //}
    }

    public void Print()
    {
        Debug.Log("Name: " + name);
        Debug.Log("types: ");
        foreach (var t in Types)
            Debug.Log(t);
        Debug.Log("Weaknesses: ");
        foreach (var w in Weaknesses)
            Debug.Log(w);
        Debug.Log("Resistances: ");
        foreach (var r in Resistances)
            Debug.Log(r);
    }
}
