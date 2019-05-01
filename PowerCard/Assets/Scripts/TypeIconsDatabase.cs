using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TypeIconsDatabase", menuName = "TypeIconsDatabase")]
public class TypeIconsDatabase : ScriptableObject {

    // BETTER IF THERE WAS A SERIALIZABLE DICTIONARY
    // TRY TO FIGURE THAT OUT

    public Sprite
         ASTRAL, FIRE, WATER, GRASS, ELECTRIC,
        ICE, ROCK, POISON, PSYCHIC, MYSTERY,
        LIGHT, DARK, DRAGON, FLYING, PLATNUM,
        PARADOX, RAINBOW;
    
    public Sprite GetTypeSprite(DenigenData.Type _type)
    {
        switch(_type)
        {
            case DenigenData.Type.ASTRAL: return ASTRAL;
            case DenigenData.Type.FIRE: return FIRE;
            case DenigenData.Type.WATER: return WATER;
            case DenigenData.Type.GRASS: return GRASS;
            case DenigenData.Type.ELECTRIC: return ELECTRIC;
            case DenigenData.Type.ICE: return ICE;
            case DenigenData.Type.ROCK: return ROCK;
            case DenigenData.Type.POISON: return POISON;
            case DenigenData.Type.PSYCHIC: return PSYCHIC;
            case DenigenData.Type.MYSTERY: return MYSTERY;
            case DenigenData.Type.LIGHT: return LIGHT;
            case DenigenData.Type.DARK: return DARK;
            case DenigenData.Type.DRAGON: return DRAGON;
            case DenigenData.Type.FLYING: return FLYING;
            case DenigenData.Type.PLATNUM: return PLATNUM;
            case DenigenData.Type.PARADOX: return PARADOX;
            case DenigenData.Type.RAINBOW: return RAINBOW;
            default: Debug.LogError("Type Icon not found."); return null;
        }
    }

}
