using Godot;
using System;

public partial class Player : Node
{
    private Card card;
    public override void _Ready()
    {
        base._Ready();
        //card = GetNode<Card>("Card");
    }

    public float GetDamage()
    {
        float damage = 0;

        //damage += card.GetCardValue() + 1;

        return damage;
    }
}
