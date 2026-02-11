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

    // Temporary thing
    private EnemyBase enemy;

    private List<CardObj> playerHand = new List<CardObj>();
    public List<CardObj> selectedHand = new List<CardObj>();

    [Export] private PackedScene cardScene;
    [Export] private PackedScene cardGhostScene;

    [Export] public Node2D selectNode;
    [Export] private float defaultSpacing = 20;
    [Export] private float maxCap = 10;
    [Export] private int maxCards = 20;
    [Export] private int dealSize = 5;
    [Export] private float cardSize = 63;
    [Export] public float raiseHeight = 10;

    public override void _Input(InputEvent @event)
    {
        base._Input(@event);

        if (Input.IsActionJustPressed("Random"))
        {
            DealHand(dealSize);
        } else if (Input.IsActionJustPressed("Draw"))
        {
            DrawCard();
        }
    }

    public override void _Ready()
    {
        base._Ready();
        enemy = GetNode<EnemyBase>("../../Enemy");

        if (singleton == null) singleton = this;

        // Preload scenes
        cardScene = GD.Load<PackedScene>("res://Scenes/Card.tscn");
        cardGhostScene = GD.Load<PackedScene>("res://Scenes/CardGhost.tscn");
    }

    private void DrawCard(bool skipSpacing = false)
    {
        if (playerHand.Count + selectedHand.Count >= maxCards) return;

        playerHand.Add(CreateCardObject(RNG.RandiRange(0, totalCardSprites - 1), Vector2.Zero));
        GD.Print("Spawned card object");

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

    public void SelectCard(CardObj _card)
    {
        // If there are already 4 cards selected do nothing
        if (selectedHand.Count >= 4) 
            return;

        if (_card.IsQueuedForDeletion()) return;

        _card.isSelected = true;

        _card.baseHeight = selectNode.Position.Y;


        selectedHand.Add(_card);
        RemoveCard(_card);

        _card.GetParent<Node2D>().Reparent(selectNode);
    }
    public void SelectPlayCard(CardObj _card)
    {
        // If the card is selected then play the card
        if (selectedHand.Contains(_card))
        {
            EnemyManager.singleton.Attack(_card.GetCardDamageValue());
            //enemy.TakeDamage(_card.GetCardDamageValue());

            selectedHand.Remove(_card);

            _card.GetParent<CardGhost>().QueueFree();
            _card.QueueFree();

            return;
        }

        SelectCard(_card);
    }

    private void DealHand(int _numCards)
    {
        for (int i = playerHand.Count; i < _numCards; i++)
        {
            DrawCard(true);
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

            GD.Print("Card " + i + ": " + _hand[i].GetParent<CardGhost>().Position);
        }
    }

    private CardObj CreateCardObject(int _cardID, Vector2 positon)
    {
        // Instantiate Card Ghost and Card
        CardGhost ghostInstance = cardGhostScene.Instantiate<CardGhost>();
        CardObj instance = cardScene.Instantiate<CardObj>(); ;

        ghostInstance.subject = instance;

        instance.Position = positon;

        instance.card.SetValues(_cardID);

        // Spawn nodes then Reparent the card to its ghost
        AddChild(ghostInstance);
        AddChild(instance);
        instance.Reparent(ghostInstance);

        return instance;
    }
}
