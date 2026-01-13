using Godot;
using System;
using System.Collections.Generic;
using System.Data;
using static System.Net.Mime.MediaTypeNames;

public struct Card
{
    public Card(int _cardID)
    {
        SetValues(_cardID);
    }
    public Card(int _rank, int _suit)
    {
        rank = _rank;
        suit = _suit;
    }

    public int rank { get; private set; }
    public int suit { get; private set; }

    public void SetValues(int _cardID)
    {
        rank = _cardID % 13;
        suit = _cardID % 4;
    }

    public int GetID()
    {
        return (suit * 13) + rank;
    }
}

public partial class CardObj : Sprite2D
{
    public static int callCounter = 0;

    private static int SPRITE_WIDTH = 63;
    private static int SPRITE_HEIGHT = 88;

    private static int totalCardSprites;

    public Card card;

    public CardObj(int cardID)
    {
        // Initiallise the card
        card = new Card(cardID);
    }

    public override void _Ready()
    {
        base._Ready();
        GetSpriteCount();

        UpdateSprite();
    }

    private void GetSpriteCount()
    {
        int x = Texture.GetWidth() / SPRITE_WIDTH;
        int y = Texture.GetHeight() / SPRITE_HEIGHT;

        totalCardSprites = (x * y);
    }

    private void UpdateSprite()
    {
        Frame = card.GetID();
    }

    public float GetCardValue()
    {
        return card.GetID();
    }
}
