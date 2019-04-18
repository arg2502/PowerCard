using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    public List<string> deckStr; // public for testing
    public List<CardData> deck;
    public List<Card> hand;
    public List<Card> field;
    public List<CardData> discard;

    private void Start()
    {
        PopulateDeck();
        ShuffleDeck();
        //PrintDeck();
    }

    void PopulateDeck()
    {
        deck = new List<CardData>();
        foreach(var s in deckStr)
        {
            var cd = GameControl.CardManager.cardDatabase.Find(s);
            deck.Add(cd);
        }
    }

    void ShuffleDeck()
    {
        for(int i = 0; i < deck.Count; i++)
        {
            var temp = deck[i];
            int randomIndex = Random.Range(i, deck.Count);
            deck[i] = deck[randomIndex];
            deck[randomIndex] = temp;
        }
    }

    void PrintDeck()
    {
        foreach(var c in deck)
        {
            print(c.name);
        }
    }

    public void Draw()
    {
        if(deck.Count <= 0)
        {
            Debug.LogWarning("Deck is empty");
            return;
        }
        // remove card from deck
        var drawCard = deck[0];
        deck.RemoveAt(0);    
            
        // create card based on carddata drawn
        var c = Instantiate(GameControl.control.CardPrefab, this.transform);
        var card = c.GetComponent<Card>();
        card.Init(drawCard);

        // add to hand
        hand.Add(card);
    }
}
