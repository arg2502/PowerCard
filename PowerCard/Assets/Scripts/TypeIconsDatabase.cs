using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TypeIconsDatabase", menuName = "TypeIconsDatabase")]
public class TypeIconsDatabase : ScriptableObject {

    // BETTER IF THERE WAS A SERIALIZABLE DICTIONARY
    // TRY TO FIGURE THAT OUT

    public Sprite
        FIRE, METAL, WOOD, EARTH, ELECTRIC, WATER,
        SPIRIT, VOID, STAR, PSYCHIC,
        ASCENDANT, SEEKER;
    
    public Sprite GetTypeSprite(DenigenData.Type _type)
    {
        switch(_type)
        {
            case DenigenData.Type.FIRE: return FIRE;
            case DenigenData.Type.METAL: return METAL;
            case DenigenData.Type.WOOD: return WOOD;
            case DenigenData.Type.EARTH: return EARTH;
            case DenigenData.Type.ELECTRIC: return ELECTRIC;
            case DenigenData.Type.WATER: return WATER;
            case DenigenData.Type.SPIRIT: return SPIRIT;
            case DenigenData.Type.VOID: return VOID;
            case DenigenData.Type.STAR: return STAR;
            case DenigenData.Type.PSYCHIC: return PSYCHIC;
            case DenigenData.Type.ASCENDANT: return ASCENDANT;
            case DenigenData.Type.SEEKER: return SEEKER;
            default: Debug.LogError("Type Icon not found?"); return null;
        }
    }

}
