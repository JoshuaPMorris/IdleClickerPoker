using Godot;
using System;
using System.Data;
using static System.Net.Mime.MediaTypeNames;

public partial class Card : Sprite2D
{
    private static int SPRITE_WIDTH = 63;
    private static int SPRITE_HEIGHT = 88;

    private int totalCardSprites;

    private int numberID;
    private int suitID;

    public override void _Ready()
    {
        base._Ready();

        GetSpriteCount();
    }

    private void GetSpriteCount()
    {
        float x = Texture.GetWidth() / SPRITE_WIDTH;
        float y = Texture.GetHeight() / SPRITE_HEIGHT;

        totalCardSprites = (int)x * (int)y;
    }

    public override void _Input(InputEvent @event)
    {
        base._Input(@event);
        if (Input.IsActionPressed("Increase"))
            numberID++;
        else if (Input.IsActionPressed("Decrease"))
            numberID--;

        if (numberID < 0)
            numberID = 0;
        else if (numberID > totalCardSprites - 1)
            numberID = totalCardSprites - 1;

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
