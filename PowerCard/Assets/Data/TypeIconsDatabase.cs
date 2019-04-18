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

    public Dictionary<DenigenData.Type, Sprite> typeDatabase;
    private void Awake()
    {
        typeDatabase = new Dictionary<DenigenData.Type, Sprite>() { };
    
    }
}
