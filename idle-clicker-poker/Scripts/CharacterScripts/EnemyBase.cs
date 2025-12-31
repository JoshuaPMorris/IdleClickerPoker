using Godot;
using System;

public partial class EnemyBase : Area2D
{
    [Export] private float health = 50;
    private Node playerObject;
    [Export] public float damage = 10;

    // This function is called once when the node is in the scene and is ready to be used
    public override void _Ready()
    {
        base._Ready();
        SetPlayerObject();
    }

    // Gets and stores the Player node
    // Might replace this once there is a Manager/Spawner/Singleton for enemies
    private void SetPlayerObject()
    {
        // Get the Player node which stores the player related data
        playerObject = GetNode("../Player");

        // Test case to ensure we are getting the right Node
        if (playerObject != null) GD.Print("Sucessfully found " + playerObject.Name); 
        else GD.Print("Get Node Failed!");
    }

	public void TakeDamage(float _damage){
		health -= _damage;
        GD.Print("Ow! Health: " + health);
    }

    public override void _InputEvent(Viewport viewport, InputEvent @event, int shapeIdx)
    {
        base._InputEvent(viewport, @event, shapeIdx);
        // If the Character is clicked then interact with it
		if (Input.IsActionPressed("Click")) TakeDamage(damage);
    }
}
