using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Denigen", menuName = "Cards/Denigen")]
public class DenigenData : CardData
{    
    public int Stars { get; set; }
    public enum Rarity { UNKNOWN, COMMON, UNCOMMON, RARE, JUDGE }
    Rarity rarity;
    public int Atk { get; set; }
    public int Sh { get; set; }
    public enum Type
    {
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
            rarity = DecipherRarity(data[7]);

        }
    }

    List<Type> DecipherTypes(string typedata)
    {
        // split data by space in case of dual type
        var typesArray = typedata.Split(' ');

        List<Type> myTypes = new List<Type>();
        foreach(var t in typesArray)
        {
            switch(t)
            {
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
            if (TypeWeaknesses.ContainsKey(t))
                Weaknesses.Add(TypeWeaknesses[t]);

            if (TypeResistances.ContainsKey(t))
                Resistances.Add(TypeResistances[t]);
             
        }
    }

    Rarity DecipherRarity(string r)
    {
        switch(r)
        {
            case "COMMON": return Rarity.COMMON;
            case "UNCOMMON": return Rarity.UNCOMMON;
            case "RARE": return Rarity.RARE;
            case "JUDGE": return Rarity.JUDGE;
            default: Debug.LogError("Rarity not found?"); return Rarity.UNKNOWN;
        }
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
