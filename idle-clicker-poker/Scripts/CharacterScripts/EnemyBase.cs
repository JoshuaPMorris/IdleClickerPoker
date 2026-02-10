using Godot;
using System;

public partial class EnemyBase : Area2D
{
    private Node playerObject;
    private Player playerScript;

    private Sprite2D sprite;
    private int animFrame = 0;

    [Export] private float health = 20;
    [Export] public float damage = 10;

    private bool isDead;

    // This function is called once when the node is in the scene and is ready to be used
    public override void _Ready()
    {
        base._Ready();
        GetPlayer();
        GetSprite();
    }

    // Gets and stores the Player node
    // Might replace this once there is a Manager/Spawner/Singleton for enemies
    private void GetPlayer()
    {
        // Get the Player node which stores the player related data
        playerObject = GetNode("../PlayerObject");
        playerScript = GetNode<Player>("../PlayerObject");

        // Test case to ensure we are getting the right Node
        if (playerObject != null) GD.Print("Sucessfully found " + playerObject.Name); 
        else GD.Print("Get Node Failed!");
    }

    // Gets and stores the Sprite node
    private void GetSprite()
    {
        sprite = GetChild<Sprite2D>(1);

        if (sprite != null) GD.Print("Sucessfully found " + sprite.Name);
        else GD.Print("Get Sprite Failed!");
    }

    public void TakeDamage(float _damage){
		health -= _damage;
        GD.Print("Ow! Health: " + health);

        if (health <= 0) Die();
    }

    private void Die()
    {
        health = 0;

        sprite.Frame = 1;
        isDead = true;
    }
}
