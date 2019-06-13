using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameControl : MonoBehaviour {

    public static GameControl control;
    public static CardManager CardManager;
    public TypeIconsDatabase TypeIconsDatabase;

    public TextAsset denigenSpeadsheet;
    public TextAsset powermagicSpreadsheet;

    // stores the card keys as the keys, and the rest of the denigen data as a hunk of data for now
    // the data (value) will get deciphered in the CardData classes
    public Dictionary<string, string[]> denigenKeys;
    public Dictionary<string, string[]> powermagicKeys;

    public GameObject CardPrefab;
    public List<GameObject> cardObjBank;
    public List<Player> playerList;

    int currentPlayerNum = 0;
    public Player CurrentPlayer { get { return playerList[currentPlayerNum]; } }
    
    public Text tributeMessage;
    
    private void Awake()
    {
        if (control == null)
        {
            control = this;
            CardManager = new CardManager();
            TypeIconsDatabase = Resources.Load<TypeIconsDatabase>("TypeIconsDatabase");
            cardObjBank = new List<GameObject>();
        }
        else
            Destroy(this);
    }

    private void Start()
    {
        DecipherSpreadsheets();
        //StartGame();
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

    public void StartGame()
    {
        foreach (var p in playerList)
        {
            p.powerpoints = 100;
            p.DrawStartGame();
            p.SetTurnState(Player.TurnState.STANDBY);
        }
        currentPlayerNum = 0;
        CurrentPlayer.SetTurnState(Player.TurnState.DRAW);
    }

    public void NextTurn()
    {
        currentPlayerNum++;
        if (currentPlayerNum >= playerList.Count)
            currentPlayerNum = 0;
        CurrentPlayer.SetTurnState(Player.TurnState.DRAW);
    }

    public void CheckForWinner()
    {
        var remainingPlayers = playerList.FindAll(p => p.powerpoints > 0);
        if(remainingPlayers.Count == 1)
        {
            print("GAME OVER");
            print(remainingPlayers[0].name + " wins!");
        }
    }

    public void OnCardClicked(Card card)
    {
        if (card.position == Card.Position.HAND)
        {
            CurrentPlayer.SelectInHand(card);
        }
        else if (card.position == Card.Position.FIELD)
        {
            CurrentPlayer.SelectOnField(card);
        }
    }
}
