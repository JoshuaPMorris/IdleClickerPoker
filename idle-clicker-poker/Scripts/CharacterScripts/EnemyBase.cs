using Godot;
using System;

public partial class EnemyBase : Area2D
{
    public static float maxArmour = 100;

    [Export] private Sprite2D sprite;
    private int animFrame = 0;

    [Export] private float health;
    [Export] private float armour;

    // This function is called once when the node is in the scene and is ready to be used
    public override void _Ready()
    {
        base._Ready();
    }

    public bool TakeDamage(float _damage)
    {
        float damage = _damage - (_damage / (maxArmour - armour));
        health -= damage;

        if (health <= 0) 
        {
            sprite.Frame = 1;
            return true;
        }
        return false;
    }
}
