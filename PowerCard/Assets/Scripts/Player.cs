using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour {

    public List<string> deckStr; // public for testing
    public List<CardData> deck;
    public List<Card> hand;
    public List<Card> field;
    public List<CardData> discard;

    // SUMMON -- MAYBE MOVE TO SEPARATE SUMMONING CLASS???
    // but maybe it can stay here since a player can only normal
    // summon one card at a time
    Card cardToSummon;
    List<Card> tributedCards;
    public Text tributeMessage;
    bool readyToSummon;

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
        GameObject cardObj;
        if (GameControl.control.cardObjBank.Count > 0)
        {
            cardObj = GameControl.control.cardObjBank[0];
            GameControl.control.cardObjBank.RemoveAt(0);
            cardObj.SetActive(true);
        }
        else
        {
            cardObj = Instantiate(GameControl.control.CardPrefab, this.transform);
        }
        var card = cardObj.GetComponent<Card>();
        card.Init(drawCard, this);

        if (hand.Count > 0)
        {
            card.transform.position = hand[hand.Count - 1].transform.position;
            card.transform.position += new Vector3(.75f, 0, 0);
        }
            
        hand.Add(card);        
    }
    
    public bool EnoughTributes(DenigenData data)
    {
        var denCount = field.FindAll((c) => c.data is DenigenData).Count;
        return denCount >= data.Stars - 1;
    }

    public void SelectInHand(Card card)
    {
        if (card.dataInstance is DenigenData)
        {
            // check if there are enough tributes for this card to be summoned
            var denData = (DenigenData)card.dataInstance;
            if (!EnoughTributes(denData))
                return;

            BeginSummon(card);
        }
    }

    public void SelectOnField(Card card)
    {
        if(cardToSummon != null)
        {
            tributedCards.Add(card);
            ConfirmSummon();
        }
    }

    void BeginSummon(Card summon)
    {
        cardToSummon = summon;
        tributedCards = new List<Card>();
        ConfirmSummon();
    }

    public void ConfirmSummon()
    {
        if(tributedCards.Count < cardToSummon.Stars-1)
        {
            tributeMessage.gameObject.SetActive(true);
            tributeMessage.text = "Select cards for tribute.";
        }
        else
        {
            tributeMessage.gameObject.SetActive(true);
            readyToSummon = true;
            tributeMessage.text = "Up arrow = summon faceup\nDown Arrow = summon facedown";
        }
    }

    void Summon(bool facedown)
    {
        foreach (var c in tributedCards)
            DiscardFromField(c);
        FromHandToField(cardToSummon, facedown);

        readyToSummon = false;
        tributedCards.Clear();
        tributeMessage.gameObject.SetActive(false);
    }

    void FromHandToField(Card card, bool facedown)
    {
        hand.Remove(card);
        field.Add(card);
        card.position = Card.Position.FIELD;
        if (facedown)
        {
            card.face = Card.Face.FACEDOWN;
            card.transform.Rotate(Vector3.up, 180f);
        }
        else
            card.face = Card.Face.FACEUP;
        
        // temp
        card.transform.position += Vector3.up * 2;
    }

    void DiscardFromHand(Card card)
    {
        hand.Remove(card);
        discard.Add(card.data);
        card.Discard();
    }

    void DiscardFromField(Card card)
    {
        field.Remove(card);
        discard.Add(card.data);
        card.Discard();
    }

    private void Update()
    {
        if (readyToSummon)
        {
            if (Input.GetKeyUp(KeyCode.UpArrow))
            {
                Summon(facedown: false);
            }
            else if (Input.GetKeyUp(KeyCode.DownArrow))
            {
                Summon(facedown: true);
            }
        }
    }
}
