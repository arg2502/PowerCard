using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardManager {

    public CardDatabase cardDatabase;

    public CardManager()
    {
        cardDatabase = Resources.Load<CardDatabase>("CardDatabase");
    }

}
