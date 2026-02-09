using Godot;
using System;
using System.Collections.Generic;
using static System.Formats.Asn1.AsnWriter;

public partial class CardManager : Node
{
    public static RandomNumberGenerator RNG = new RandomNumberGenerator();

    private static int totalCardSprites = 52;

    private List<CardObj> playerHand = new List<CardObj>();

    PackedScene cardScene;
    [Export] float spacing = 40;
    [Export] float cardSize = 63;

    public override void _Input(InputEvent @event)
    {
        base._Input(@event);

        if (Input.IsActionJustPressed("Random"))
        {
            Deal(2);
        }
    }

    public override void _Ready()
    {
        base._Ready();

        // Preload the card scene
        cardScene = GD.Load<PackedScene>("res://Scenes/Card.tscn");
    }

    private void Deal(int _numCards)
    {
        // If no card objects exist create them
        if (playerHand.Count == 0)
        {
            for (int i = 0; i < _numCards; i++)
            {
                playerHand.Add(CreateCardObject(RNG.RandiRange(0, totalCardSprites - 1), Vector2.Zero));
                GD.Print("Spawned card object");
            }
            SpaceCards();
            return;
        }

        for (int i = 0; i < _numCards; i++) {
            if (playerHand[i] != null)
            {
                playerHand[i].SetCardValues(RNG.RandiRange(0, totalCardSprites - 1));
                GD.Print("Set random values");
            }
        }
        SpaceCards();
    }

    private void SpaceCards()
    {
        for (int i = 0; i < playerHand.Count; i++) {
            Vector2 pos = Vector2.Zero;
            pos.X = (spacing / 2) * (i % 2 == 0 ? 1 : -1);

            playerHand[i].Position = pos;

            GD.Print("Card " + i + ": " + playerHand[i].Position);
        }
    }

    private CardObj CreateCardObject(int _cardID, Vector2 positon)
    {
        // Spawn a copy of the cardScene
        CardObj instance = cardScene.Instantiate<CardObj>();
        instance.Position = positon;

        instance.card.SetValues(_cardID);

        AddChild(instance);
        return instance;
    }
}
