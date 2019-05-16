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
    bool readyToNormalSummon;

    public GameObject handPos;
    public GameObject fieldPos;
    public List<CardSlot> denCardSlots;
    float drawDistance = 1.75f;

    public enum TurnState { STANDBY, DRAW, SUMMON, ATTACK, POST, OUTOFGAME }
    public TurnState turnState;

    public enum CardHandleState { NORMAL, TRIBUTE, TARGET }
    public CardHandleState cardHandleState;

    public Card currentAttacker;
    public Card currentTarget;

    public int powerpoints;

    int normalSummonsPerformed = 0;
    int maxNormalSummons = 1;

    [Header("UI")]
    public Text turnStateUI;
    public Text cardHandleStateUI;
    public Text normalSummonsUI;
    public Text powerpointsUI;

    private void Start()
    {
        powerpoints = 100;
        PopulateDeck();
        ShuffleDeck();
        //PrintDeck();
    }

    public void SetTurnState(TurnState newState)
    {
        if (turnState == newState) return;

        turnState = newState;
        switch (turnState)
        {
            case TurnState.DRAW:
                break;
            case TurnState.SUMMON:
                normalSummonsPerformed = 0;
                break;
            case TurnState.ATTACK:
                break;
            case TurnState.POST:
                break;
            case TurnState.OUTOFGAME:
                break;
            case TurnState.STANDBY:
                // at end of turn, set any field cards hasAttacked flag to false
                foreach(var c in field)
                {
                    c.hasAttacked = false;                    
                }
                tributedCards.Clear();
                cardHandleState = CardHandleState.NORMAL;
                currentAttacker = null;
                currentTarget = null;
                cardToSummon = null;
                readyToNormalSummon = false;
                break;
        }        
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

    public void DrawStartGame()
    {
        for (int i = 0; i < 6; i++)
        {
            Draw();
        }
    }

    public void DrawStartTurn()
    {
        if (turnState != TurnState.DRAW)
            return;
        Draw();
        SetTurnState(TurnState.SUMMON);
    }

    void Draw()
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
            cardObj.transform.SetParent(handPos.transform);
            cardObj.transform.position = handPos.transform.position;
            cardObj.SetActive(true);
        }
        else
        {
            cardObj = Instantiate(GameControl.control.CardPrefab, handPos.transform);
        }
        var card = cardObj.GetComponent<Card>();
        card.Init(drawCard, this);

        if (hand.Count > 0)
        {
            card.transform.position = hand[hand.Count - 1].transform.position;
            card.transform.position += new Vector3(drawDistance, 0, 0);
        }
            
        hand.Add(card);        
    }
    
    public bool EnoughTributes(DenigenData data)
    {
        var denCount = field.FindAll((c) => c.data is DenigenData).Count;
        return denCount >= data.Stars - 1;
    }

    bool IsCardMine(Card card)
    {
        return card.player == this;
    }

    public void SelectInHand(Card card)
    {
        if (!IsCardMine(card)) return; // for now, just return if the card is not yours
        if (card.dataInstance is DenigenData)
        {
            // check if we are in summoning phase
            if (turnState == TurnState.SUMMON)
            {
                // check if we are allowed to normal summon
                if (normalSummonsPerformed < maxNormalSummons)
                {
                    // check if there are enough tributes for this card to be summoned
                    var denData = (DenigenData)card.dataInstance;
                    if (!EnoughTributes(denData))
                        return;

                    BeginNormalSummon(card);
                }
            }
        }
    }

    public void SelectOnField(Card card)
    {
        if(cardHandleState == CardHandleState.NORMAL)
        {
            if (IsCardMine(card))
                BeginTarget(card);
        }
        else if(cardHandleState == CardHandleState.TRIBUTE)
        {
            if (!tributedCards.Contains(card))
            {
                tributedCards.Add(card);
                ConfirmNormalSummon();
            }
        }
        else if (cardHandleState == CardHandleState.TARGET)
        {
            // check that we're not targeting ourselves
            if (card.player != this)
                BeginAttack(card);
        }
    }

    void BeginNormalSummon(Card summon)
    {
        cardHandleState = CardHandleState.TRIBUTE;
        cardToSummon = summon;
        tributedCards = new List<Card>();
        ConfirmNormalSummon();
    }

    public void ConfirmNormalSummon()
    {
        if(tributedCards.Count < cardToSummon.Stars-1)
        {
            tributeMessage.gameObject.SetActive(true);
            tributeMessage.text = "Select cards for tribute.";
        }
        else
        {
            tributeMessage.gameObject.SetActive(true);
            readyToNormalSummon = true;
            tributeMessage.text = "Up arrow = summon faceup\nDown Arrow = summon facedown";
        }
    }

    void Summon(bool facedown)
    {
        foreach (var c in tributedCards)
            //DiscardFromField(c);
            c.Discard();
        //FromHandToField(cardToSummon, facedown);
        cardToSummon.Summon(facedown);

        if (readyToNormalSummon)
        {
            readyToNormalSummon = false;
            normalSummonsPerformed++;
        }
        cardToSummon = null;
        tributedCards.Clear();
        tributeMessage.gameObject.SetActive(false);
    }
    
    void BeginTarget(Card card)
    {
        //// ANOTHER PLAYER SELECTS YOUR CARD
        //Player attacker = null;        
        //if (OpponentIsTargeting(ref attacker))
        //{
        //    attacker.BeginAttack(card);
        //}

        // WHEN YOU SELECT YOUR OWN CARD
        if (OpponentTargetsExist() && !card.hasAttacked)
        {
            currentAttacker = card;
            cardHandleState = CardHandleState.TARGET;
            tributeMessage.text = "Select target";
            tributeMessage.gameObject.SetActive(true);
        }
    }

    bool OpponentTargetsExist()
    {
        foreach(var p in GameControl.control.playerList)
        {
            if (p == this) continue;

            if (p.field.Count > 0) return true;
        }
        return false;
    }

    //bool OpponentIsTargeting(ref Player attacker)
    //{
    //    foreach (var p in GameControl.control.playerList)
    //    {
    //        if (p == this) continue;

    //        if (p.cardHandleState == CardHandleState.TARGET)
    //        {
    //            attacker = p;
    //            return true;
    //        }
    //    }
    //    return false;
    //}

    public void BeginAttack(Card target)
    {        
        SetTurnState(TurnState.ATTACK);
        cardHandleState = CardHandleState.NORMAL;
        currentTarget = target;

        if (currentAttacker.face == Card.Face.FACEDOWN)
            currentAttacker.FlipCard();
        if (currentTarget.face == Card.Face.FACEDOWN)
            currentTarget.FlipCard();

        //print(currentAttacker.dataInstance.name + " attacks " + currentTarget.dataInstance.name);
        var denAttacker = currentAttacker.dataInstance as DenigenData;
        var denTarget = currentTarget.dataInstance as DenigenData;

        int atk = 0, sh = 0;
        GetTypeStats(denAttacker, denTarget, ref atk, ref sh);
        var diff = atk - sh;

        // if atk is greater, then discard target and player loses pp
        if(diff > 0)
        {
            //print(denTarget.name + " defeated.");
            currentTarget.player.LosePoints(Mathf.Abs(diff));
            currentTarget.Destroy();
            tributeMessage.gameObject.SetActive(false);            
        }
        else
        {
            currentAttacker.player.LosePoints(Mathf.Abs(diff));
            tributeMessage.text = "not stronk enough";
        }

        // after attack
        currentAttacker.hasAttacked = true;

        currentTarget = null;
        currentAttacker = null;

        // check if we go to POST
        bool canStillAttack = false;
        foreach(var c in field)
        {
            if (!c.hasAttacked)
            {
                canStillAttack = true;
                break;
            }
        }
        if (!canStillAttack)
            SetTurnState(TurnState.POST);

    }

    void GetTypeStats(DenigenData attacker, DenigenData target, ref int atk, ref int sh)
    {
        atk = attacker.Atk;
        sh = target.Sh;
                
        foreach (var t in attacker.Types)
        {
            // if target is weak, increase attacker's ATK by attacker's STR
            if (target.Weaknesses.Contains(t))
            {
                atk += attacker.Str;
                return;
            }
            // if target is resistant, increase target's SH by target's STR
            else if(target.Resistances.Contains(t))
            {
                sh += target.Str;
                return;
            }
        }
    }

    public void LosePoints(int points)
    {
        powerpoints = Mathf.Max(powerpoints - points, 0);

        if(powerpoints == 0)
        {
            GameControl.control.CheckForWinner();
        }
    }

    public void EndTurn()
    {
        if (GameControl.control.CurrentPlayer != this) return;       
        cardHandleState = CardHandleState.NORMAL;
        SetTurnState(TurnState.STANDBY);
        GameControl.control.NextTurn();
    }

    private void Update()
    {
        if (turnState == TurnState.DRAW)
        {
            UpdateDraw();
        }
        else if (turnState == TurnState.SUMMON)
        {
            UpdateSummon();
        }
        else if (turnState == TurnState.ATTACK)
        {
            UpdateAttack();
        }
        else if (turnState == TurnState.POST)
        {
            UpdatePost();
        }
        else if(turnState == TurnState.STANDBY)
        {
            UpdateStandby();
        }
        else if (turnState == TurnState.OUTOFGAME)
        {
            UpdateOutOfGame();
        }

        turnStateUI.text = turnState.ToString();
        cardHandleStateUI.text = cardHandleState.ToString();
        normalSummonsUI.text = normalSummonsPerformed.ToString();
        powerpointsUI.text = powerpoints.ToString();
    }

    void UpdateDraw() { }
    void UpdateSummon()
    {
        if (readyToNormalSummon)
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
    void UpdateAttack() { }
    void UpdatePost() { }
    void UpdateStandby() { }
    void UpdateOutOfGame() { }
}
