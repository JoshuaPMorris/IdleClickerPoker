using Godot;
using System;

public partial class EnemyBase : Area2D
{
    [Export] private float health = 50;
    [Export] public float damage = 10;
	
	public void TakeDamage(float _damage){
		health -= _damage;
        GD.Print("Ow!" + health);
    }

    public override void _InputEvent(Viewport viewport, InputEvent @event, int shapeIdx)
    {
        base._InputEvent(viewport, @event, shapeIdx);
		if (Input.IsActionPressed("Click")) TakeDamage(damage);
    }
}
