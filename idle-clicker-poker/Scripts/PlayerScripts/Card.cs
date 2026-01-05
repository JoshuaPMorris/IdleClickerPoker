using Godot;
using System;
using System.Data;
using static System.Net.Mime.MediaTypeNames;

public partial class Card : Sprite2D
{
    private int numberID;
    private int suitID;

    public override void _Input(InputEvent @event)
    {
        base._Input(@event);
        if (Input.IsActionPressed("Increase"))
            numberID++;
        else if (Input.IsActionPressed("Decrease"))
            numberID--;

        if (numberID < 0)
            numberID = 0;
        else if (numberID > 12)
            numberID = 12;

        UpdateSprite();
    }

    private int GetCard()
    {
        return (suitID * 13) + numberID;
    }

    private void UpdateSprite()
    {
        Frame = GetCard();
    }

    public float GetCardValue()
    {
        int cardID = GetCard();
        return cardID;
    }
}
