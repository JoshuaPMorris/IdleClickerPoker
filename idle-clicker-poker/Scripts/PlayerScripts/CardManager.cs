using Godot;
using System;
using System.Collections.Generic;
using static System.Formats.Asn1.AsnWriter;

public partial class CardManager : Node2D
{
    // Singleton
    public static CardManager singleton;
    
    public static RandomNumberGenerator RNG = new RandomNumberGenerator();

    private static int totalCardSprites = 52;

    private List<int> deck = new List<int>();

    private List<CardObj> playerHand = new List<CardObj>();
    public List<CardObj> selectedHand = new List<CardObj>();

    private List<Card> last5Cards = new List<Card>();

    [Export] private PackedScene cardScene;
    [Export] private PackedScene cardGhostScene;

    [ExportGroup("Deal Settings")]
    [Export] public Node2D selectNode;
    [Export] private float defaultSpacing = 20;
    [Export] private float maxCap = 10;
    [Export] private int maxCards = 20;
    [Export] private int dealSize = 5;
    [Export] private float cardSize = 63;
    [Export] public float raiseHeight = 10;
    [Export] private int playSize = 5;
    [Export] private int numTimesToShuffle = 52;

    [ExportGroup("Hand Multipliers")]
    // Damage multipliers for each hand
    [Export] private float pair = 1;
    [Export] private float threeOfAKind = 1;
    [Export] private float straight = 1;
    [Export] private float flush = 1;
    [Export] private float fullHouse = 1;
    [Export] private float fourOfAKind = 1;
    [Export] private float royalFlush = 1;


    public override void _Input(InputEvent @event)
    {
        base._Input(@event);

        if (Input.IsActionJustPressed("Random"))
        {
            for (int i = 0; i < maxCards; i++)
            {
                DrawCard(i, false);
            }
        } else if (Input.IsActionJustPressed("Draw"))
        {
            DrawFromDeck();
        }
    }

    public override void _Ready()
    {
        base._Ready();

        if (singleton == null) singleton = this;

        // Fills the deck
        for (int i = 0; i < totalCardSprites; i++) deck.Add(i);
        ShuffleDeck(52);

        // Preload scenes
        cardScene = GD.Load<PackedScene>("res://Scenes/Card.tscn");
        cardGhostScene = GD.Load<PackedScene>("res://Scenes/CardGhost.tscn");
    }

    // Draws a card from the limited deck, ensuring no duplicate cards are drawn
    public void DrawFromDeck()
    {
        //int selectedCardID = deck[RNG.RandiRange(0, deck.Count - 1)];
        int selectedCardID = deck[deck.Count - 1];
        DrawCard(selectedCardID, false);
    }
    private void AddToDeck(int cardID, float whereToAdd)
    {
        int index = RNG.RandiRange(0, (int)((float)deck.Count / whereToAdd));
        deck.Insert(index, cardID);
    }

    private void ShuffleDeck(int numShuffles)
    {
        for(int i = 0; i < numShuffles; i++)
        {
            for(int x = 0; x < deck.Count; x++)
            {
                int ID = deck[x];
                deck.RemoveAt(x);
                AddToDeck(ID, 1);
            }
        }
    }

    private void DrawCard(int cardToDraw, bool skipSpacing = false)
    {
        if (playerHand.Count + selectedHand.Count >= maxCards) return;

        playerHand.Add(CreateCardObject(cardToDraw, Vector2.Zero));
        deck.Remove(cardToDraw);

        if (skipSpacing) return;

        SpaceCards(ref playerHand, defaultSpacing, maxCap, GlobalPosition);
    }

    private void RemoveCard(int _cardIndex)
    {
        playerHand.RemoveAt(_cardIndex);

        SpaceCards(ref selectedHand, 25, 4, selectNode.GlobalPosition);
        SpaceCards(ref playerHand, defaultSpacing, maxCap, GlobalPosition);
    }
    private void RemoveCard(CardObj _card)
    {
        playerHand.Remove(_card);

        SpaceCards(ref selectedHand, (float)(defaultSpacing * 1.65), 4, selectNode.GlobalPosition);
        SpaceCards(ref playerHand, defaultSpacing, maxCap, GlobalPosition);
    }

    private void SelectCard(CardObj _card)
    {
        // If there are already 4 cards selected do nothing
        if (selectedHand.Count >= playSize) 
            return;

        if (_card.IsQueuedForDeletion()) return;

        _card.isSelected = true;

        _card.baseHeight = selectNode.Position.Y;

        selectedHand.Add(_card);
        RemoveCard(_card);

        _card.GetParent<Node2D>().Reparent(selectNode);
    }

    private void PlayCard(CardObj _card)
    {
        last5Cards.Add(_card.card);
        if (last5Cards.Count > 5)
        {
            // Add the now 6th most recently played card back to the deck and remove it from the tally
            AddToDeck(last5Cards[0].cardID, 2);
            last5Cards.RemoveAt(0);
        }

        // Calculate the damage
        float damage = _card.GetCardDamageValue();
        float multi = GetHandTypeMulti();
        GD.Print(multi);

        // Attack the enemy
        EnemyManager.singleton.Attack(damage * multi);

        selectedHand.Remove(_card);

        // Destroy the card object
        _card.GetParent<CardGhost>().QueueFree();
        _card.QueueFree();
    }

    public void PlayOrSelectCard(CardObj _card)
    {
        // If the card is selected then play the card
        if (selectedHand.Contains(_card))
        {
            PlayCard(_card);
            return;
        }

        // Otherwise select that card
        SelectCard(_card);
    }

    private float GetHandTypeMulti()
    {
        float multiplier = 1;

        int sameSuit = 0;
        int sameRank = 0;

        int fullHouseOtherRank = 20;
        int numLikeOtherCard = 0;

        // Loop through the last cards played and compair it to the most recent card played
        for (int i = 0; i < last5Cards.Count - 1; i++)
        {
            // Skip if it's on the last card. This allows 
            if (i == last5Cards.Count - 1) continue;

            if (last5Cards[i].GetRank() == last5Cards[last5Cards.Count - 1].GetRank()) sameRank++;
            if (last5Cards[i].GetSuit() == last5Cards[last5Cards.Count - 1].GetSuit()) sameSuit++;

            if (last5Cards[i].GetRank() != last5Cards[last5Cards.Count - 1].GetRank())
            {
                // If fullHouseOtherRank is the same as the rank of the most recent card then it has not be 'assigned'
                if (fullHouseOtherRank == 20)
                    fullHouseOtherRank = last5Cards[i].GetRank();

                if (last5Cards[i].GetRank() == fullHouseOtherRank) numLikeOtherCard++;
            }
        }
        if (last5Cards.Count == 5)
        {
            // Check if Straight is played
            if (CheckStraight(out bool isRoyal))
            {
                multiplier = straight;
                GD.Print("STRAIGHT");

                // If the hand is a Straight check to see if it is also a Straight Flush
                if (sameSuit == 4)
                {
                    multiplier *= flush;
                    GD.Print("STRAIGHT FLUSH");

                    // If the hand is a Straight Flush check if it is also a Royal Flush
                    if (isRoyal)
                    {
                        multiplier *= royalFlush;
                        GD.Print("ROYAL FLUSH");
                    }
                }
                // Can return here because if is a Straight and not a Flush it can't be anything else
                return multiplier;
            }
            // Check if Four of a Kind is Played
            if (sameRank == 3)
            {
                GD.Print("FOUR OF A KIND");
                return fourOfAKind;
            }
            // Check if Full House is played
            if (sameRank == 1 && numLikeOtherCard == 3)
            {
                GD.Print("FULL HOUSE");
                return fullHouse;
            }
            if (sameRank == 2 && numLikeOtherCard == 2)
            {
                GD.Print("FULL HOUSE");
                return fullHouse;
            }
            // Check if Flush is played
            if (sameSuit == 4)
            {
                GD.Print("FLUSH");
                return flush;
            }
        }
        // Check if Four of a Kind is Played
        if (sameRank == 3) 
        {
            GD.Print("FOUR OF A KIND");
            return fourOfAKind; 
        }
        // Check if Three of a Kind is played
        if (sameRank == 2) 
        {
            GD.Print("THREE OF A KIND");
            return threeOfAKind; 
        }
        // Check if Pair is played
        if (sameRank == 1)
        {
            if (numLikeOtherCard == 2) 
            {
                GD.Print("TWO PAIR");
                return pair * pair; 
            }
            GD.Print("PAIR");
            return pair;
        }
        
        return multiplier;
    }

    // Comparison function to allow the Card struct to be sorted
    private static int CompareCards(Card x, Card y)
    {
        // Compare x and y by rank first then by suit if it is the same rank
        if (x.GetRank() > y.GetRank())
            return -1;
        else if (x.GetRank() < y.GetRank())
            return 1;
        else
        {
            // Need to do the inverse because the card sprite sheet is in reverse order
            if (x.GetSuit() < y.GetSuit()) 
                return -1;
            else if (x.GetSuit() > y.GetSuit())
                return 1;

            // The cards must be exactly identical
            else return 0;
        }
    }

    private bool CheckStraight(out bool isRoyal)
    {
        isRoyal = false;
        Card[] cards = new Card[5];

        last5Cards.CopyTo(cards);

        // Sort the cards in Decending order
        Array.Sort(cards, CompareCards);

        if (cards[0].GetRank() == cards[1].GetRank() + 1 &&
            cards[1].GetRank() == cards[2].GetRank() + 1 &&
            cards[2].GetRank() == cards[3].GetRank() + 1 &&
            cards[3].GetRank() == cards[4].GetRank() + 1)
        {
            // If the highest card is an Ace then it is Royal
            if (cards[0].GetRank() == 12) isRoyal = true;
            return true;
        }

        return false;
    }

    // NOT IN USE
    private void DealHand(int _numCards)
    {
        for (int i = playerHand.Count; i < _numCards; i++)
        {
            DrawCard(-1, true);
        }

        SpaceCards(ref playerHand, defaultSpacing, maxCap, GlobalPosition);
    }

    public void SpaceCards(ref List<CardObj> _hand, float _spacing, float _maxCapacity, Vector2 _startPoint, float _yPos = 0)
    {
        float spacing = _spacing;

        // If there are more cards than can be displayed decrease the spacing to fit all cards
        if (_hand.Count > maxCap)
            spacing = (_spacing * _maxCapacity) / _hand.Count;

        for (int i = 0; i < _hand.Count; i++) {
            Vector2 pos = _startPoint;
            pos.X = _startPoint.X + (spacing * i);
            pos.Y = _startPoint.Y + _yPos;

            _hand[i].GetParent<Node2D>().GlobalPosition = pos;
        }
    }

    private CardObj CreateCardObject(int _cardID, Vector2 positon)
    {
        // Instantiate Card Ghost and Card
        CardGhost ghostInstance = cardGhostScene.Instantiate<CardGhost>();
        CardObj instance = cardScene.Instantiate<CardObj>();

        ghostInstance.subject = instance;

        instance.Position = positon;

        instance.card.SetID(_cardID);

        // Spawn nodes then Reparent the card to its ghost
        AddChild(ghostInstance);
        AddChild(instance);
        instance.Reparent(ghostInstance);

        return instance;
    }
}
