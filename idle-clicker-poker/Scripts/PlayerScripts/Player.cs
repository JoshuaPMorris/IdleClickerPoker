using Godot;
using System;

public partial class Player : Node
{
    private float damage = 1;

    public float GetDamage()
    {
        return damage;
    }
}
