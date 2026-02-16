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
        suit = _cardID / 13;
    }

    public int GetID()
    {
        return (suit * 13) + rank;
    }
}

public partial class CardObj : Area2D
{
    public static int callCounter = 0;

    private static int SPRITE_WIDTH = 63;
    private static int SPRITE_HEIGHT = 88;

    private static int totalCardSprites;

    private Sprite2D sprite;

    [Export] private int rank;
    [Export] private int suit;

    public Card card;

    public float baseHeight;
    public bool isSelected;

    public bool isHovered;

    // Something keeps asking for this but it only seems to be needed for that
    public CardObj()
    {}

    public CardObj(int cardID)
    {
        // Initiallise the card
        card = new Card(cardID);
    }

    public override void _Ready()
    {
        base._Ready();
        sprite = GetChild<Sprite2D>(1);

        GetSpriteCount();

        UpdateSprite();

        rank = card.rank;
        suit = card.suit;

        MouseEntered += OnMouseEntered;
        MouseExited += OnMouseExited;
    }

    private void GetSpriteCount()
    {
        int x = sprite.Texture.GetWidth() / SPRITE_WIDTH;
        int y = sprite.Texture.GetHeight() / SPRITE_HEIGHT;

        totalCardSprites = (x * y);
    }

    public void SetCardValues(int _cardID)
    {
        card.SetValues(_cardID);
        UpdateSprite();
    }

    private void UpdateSprite()
    {
        sprite.Frame = card.GetID();
    }

    public float GetCardDamageValue()
    {
        // Set the correct multiplier for the suits, the order of the cards is backwards and I dont wanna fix it ATM
        int suitMulti = card.suit;
        suitMulti += card.suit >= 2 ? 1 : 0;
        suitMulti -= 2;
        suitMulti *= -1;
        
        // Modify the damage based on the suit and rank of the card
        float sMulti = 1 + (((suitMulti) * 3) / 100f);
        float damage = sMulti * (card.rank + 2) + 0.05f;

        return damage;
    }

    public static Vector2 GetCardsMidPoint(List<CardObj> _cards)
    {
        Vector2 point = Vector2.Zero;

        for (int i = 0; i < _cards.Count; i++)
        {
            point += _cards[i].Position;
        }

        point.X /= _cards.Count;
        point.Y /= _cards.Count;

        return point;
    }

    public override void _InputEvent(Viewport viewport, InputEvent @event, int shapeIdx)
    {
        base._InputEvent(viewport, @event, shapeIdx);

        // Destroy the card if it is clicked
        if (Input.IsActionJustPressed("Click"))
        {
            CardManager.singleton.SelectPlayCard(this);
        }
    }

    // Hover effect
    private void OnMouseEntered()
    {
        isHovered = true;
    }
    private void OnMouseExited()
    {
        isHovered = false;

        if (GetParent<CardGhost>().hoverSwitch) Position = Vector2.Zero;
    }
}
