using Godot;
using System;
using System.Collections.Generic;

public partial class CardManager : Node
{
    public static RandomNumberGenerator RNG = new RandomNumberGenerator();

    private static int totalCardSprites = 52;

    private List<CardObj> playerHand = new List<CardObj>();


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
    }

    private void Deal(int _numCards)
    {
        if (playerHand.Count == 0)
        {
            for (int i = 0; i < _numCards; i++)
            {
                playerHand.Add(CreateCardObject(RNG.RandiRange(0, totalCardSprites - 1)));
            }
            return;
        }

        for (int i = 0; i < _numCards; i++) {
            if (playerHand[i] != null)
            {
                playerHand[i].card.SetValues(RNG.RandiRange(0, totalCardSprites - 1));
            }
        }        
    }

    private CardObj CreateCardObject(int _cardID)
    {
        CardObj cardObj = new CardObj(_cardID);
        AddChild(cardObj);
        return cardObj;
    }
}
