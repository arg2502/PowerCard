using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Card Database", menuName = "Cards/Database")]
public class CardDatabase : ScriptableObject {

    public List<CardData> database;

    /// <summary>
    /// Returns the CardData with the key of the search parameter
    /// </summary>
    /// <param name="keySearch"></param>
    /// <returns></returns>
    public CardData Find(string keySearch)
    {
        return database.Find(card => string.Equals(card.key, keySearch));
    }

}
