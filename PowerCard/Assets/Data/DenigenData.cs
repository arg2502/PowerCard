using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Denigen", menuName = "Cards/Denigen")]
public class DenigenData : CardData
{    
    int stars;
    public enum Rarity { COMMON, UNCOMMON, RARE, JUDGE }
    Rarity rarity;
    int atk;
    int sh;
    public enum Type
    {
        ASTRAL, FIRE, WATER, GRASS, ELECTRIC,
        ICE, ROCK, POISON, PSYCHIC, MYSTERY,
        LIGHT, DARK, DRAGON, FLYING, PLATNUM,
        PARADOX, RAINBOW
    }
    List<Type> types;
    List<Type> weaknesses;
    List<Type> resistances;

    Dictionary<Type, List<Type>> TypeWeaknesses = new Dictionary<Type, List<Type>>()
    {
        {Type.ASTRAL, new List<Type>(){Type.POISON, Type.MYSTERY } },
        {Type.FIRE, new List<Type>(){Type.WATER, Type.FLYING } },
        {Type.WATER, new List<Type>(){Type.ELECTRIC, Type.GRASS} },
        {Type.GRASS, new List<Type>(){Type.FIRE, Type.FLYING} },
        {Type.ELECTRIC, new List<Type>(){Type.PLATNUM, Type.ROCK } },
        {Type.ICE, new List<Type>(){Type.POISON, Type.FIRE } },
        {Type.ROCK, new List<Type>(){Type.GRASS, Type.WATER} },
        {Type.POISON, new List<Type>(){Type.DARK, Type.PSYCHIC} },
        {Type.PSYCHIC, new List<Type>(){Type.MYSTERY, Type.ASTRAL} },
        {Type.MYSTERY, new List<Type>(){Type.DARK, Type.LIGHT} },
        {Type.LIGHT, new List<Type>(){Type.DRAGON, Type.PSYCHIC} },
        {Type.DARK, new List<Type>(){Type.LIGHT, Type.DRAGON} },
        {Type.DRAGON, new List<Type>(){Type.ICE, Type.PLATNUM} },
        {Type.FLYING, new List<Type>(){Type.ELECTRIC, Type.ROCK} },
        {Type.PLATNUM, new List<Type>(){Type.ICE, Type.ASTRAL} }
    };

    Dictionary<Type, List<Type>> TypeResistances = new Dictionary<Type, List<Type>>()
    {
        {Type.ASTRAL, new List<Type>(){Type.PSYCHIC, Type.PLATNUM } },
        {Type.FIRE, new List<Type>(){Type.GRASS, Type.ICE } },
        {Type.WATER, new List<Type>(){Type.FIRE, Type.ROCK} },
        {Type.GRASS, new List<Type>(){Type.ROCK, Type.WATER} },
        {Type.ELECTRIC, new List<Type>(){Type.FLYING, Type.WATER } },
        {Type.ICE, new List<Type>(){Type.DRAGON, Type.PLATNUM } },
        {Type.ROCK, new List<Type>(){Type.FLYING, Type.ELECTRIC} },
        {Type.POISON, new List<Type>(){Type.ASTRAL, Type.ICE} },
        {Type.PSYCHIC, new List<Type>(){Type.LIGHT, Type.POISON} },
        {Type.MYSTERY, new List<Type>(){Type.PSYCHIC, Type.ASTRAL} },
        {Type.LIGHT, new List<Type>(){Type.DARK, Type.MYSTERY} },
        {Type.DARK, new List<Type>(){Type.MYSTERY, Type.POISON} },
        {Type.DRAGON, new List<Type>(){Type.DARK, Type.LIGHT} },
        {Type.FLYING, new List<Type>(){Type.FIRE, Type.GRASS} },
        {Type.PLATNUM, new List<Type>(){Type.DRAGON, Type.ELECTRIC} }
    };
    public void Init()
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
            stars = int.Parse(data[2]);
            types = DecipherTypes(data[3]);
            atk = int.Parse(data[4]);
            sh = int.Parse(data[5]);
            effect = data[6];

        }
    }

    List<Type> DecipherTypes(string typedata)
    {
        // split data by '/' in case of dual type
        var typesArray = typedata.Split('/');

        List<Type> myTypes = new List<Type>();
        foreach(var t in typesArray)
        {
            switch(t)
            {
                case "AST": myTypes.Add(Type.ASTRAL); break;
                case "FIR": myTypes.Add(Type.FIRE); break;
                case "WAT": myTypes.Add(Type.WATER); break;
                case "GRS": myTypes.Add(Type.GRASS); break;
                case "ETR": myTypes.Add(Type.ELECTRIC); break;
                case "ICE": myTypes.Add(Type.ICE); break;
                case "RCK": myTypes.Add(Type.ROCK); break;
                case "PSN": myTypes.Add(Type.POISON); break;
                case "PSY": myTypes.Add(Type.PSYCHIC); break;
                case "MYS": myTypes.Add(Type.MYSTERY); break;
                case "LGT": myTypes.Add(Type.LIGHT); break;
                case "DRK": myTypes.Add(Type.DARK); break;
                case "DRG": myTypes.Add(Type.DRAGON); break;
                case "FLY": myTypes.Add(Type.FLYING); break;
                case "PLT": myTypes.Add(Type.PLATNUM); break;
                case "PDX": myTypes.Add(Type.PARADOX); break;
                case "RNB": myTypes.Add(Type.RAINBOW); break;
            }
        }
        return myTypes;
    }

    public void FillWeaknessesAndResistances()
    {
        weaknesses = new List<Type>();
        resistances = new List<Type>();

        foreach (var t in types)
        {
            // make sure it's normal type -- Paradox and Rainbow will be special cases
            if (TypeWeaknesses.ContainsKey(t))
            {
                foreach (var w in TypeWeaknesses[t])
                {
                    // do not add if the weakness is the same as the denigen's types
                    // do not add if we already have the weakness added
                    if (!types.Contains(w) && !weaknesses.Contains(w))
                        weaknesses.Add(w);
                }
            }
            if (TypeResistances.ContainsKey(t))
            {
                foreach (var r in TypeResistances[t])
                {
                    // do not add if we already have the resistance added
                    if (!resistances.Contains(r))
                        resistances.Add(r);
                }
            }
        }

        // remove weakness if its the same type as the denigen
        foreach(var t in types)
        {
            if (weaknesses.Contains(t))
                weaknesses.Remove(t);
        }

        // remove any weakness that is also in resistance
        foreach (var r in resistances)
        {
            if (weaknesses.Contains(r))
                weaknesses.Remove(r);
        }
    }

    public void Print()
    {
        Debug.Log("Name: " + name);
        Debug.Log("types: ");
        foreach (var t in types)
            Debug.Log(t);
        Debug.Log("Weaknesses: ");
        foreach (var w in weaknesses)
            Debug.Log(w);
        Debug.Log("Resistances: ");
        foreach (var r in resistances)
            Debug.Log(r);
    }
}
