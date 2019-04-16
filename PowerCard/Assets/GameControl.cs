using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameControl : MonoBehaviour {

    public static GameControl control;
    public static CardManager CardManager;

    public TextAsset denigenSpeadsheet;
    public TextAsset powermagicSpreadsheet;

    // stores the card keys as the keys, and the rest of the denigen data as a hunk of data for now
    // the data (value) will get deciphered in the CardData classes
    public Dictionary<string, string[]> denigenKeys;
    public Dictionary<string, string[]> powermagicKeys;

    public GameObject CardPrefab;

    private void Awake()
    {
        if (control == null)
        {
            control = this;
            CardManager = new CardManager();
        }
        else
            Destroy(this);
    }

    private void Start()
    {
        DecipherSpreadsheets();
    }

    void DecipherSpreadsheets()
    {
        // split into rows
        var denRows = denigenSpeadsheet.text.Split('\n');
        var pmRows = powermagicSpreadsheet.text.Split('\n');

        denigenKeys = new Dictionary<string, string[]>();
        powermagicKeys = new Dictionary<string, string[]>();

        // skip 0 -- that row just describes what's in the columns
        for(int i = 1; i < denRows.Length; i++)
        {
            var denigenData = denRows[i].Split('\t');

            // set the first column (the key) as the key of the dictionary, and the entire row as the value
            if (!string.IsNullOrEmpty(denigenData[0]))
                denigenKeys.Add(denigenData[0], denigenData);
        }
        for(int i = 1; i < pmRows.Length; i++)
        {
            var powermagicData = pmRows[i].Split('\t');

            // set the first column (the key) as the key of the dictionary, and the entire row as the value
            powermagicKeys.Add(powermagicData[0], powermagicData);
        }
    }
}
