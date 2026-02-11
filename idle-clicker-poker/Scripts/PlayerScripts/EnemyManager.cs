using Godot;
using System;
using System.Threading.Tasks;

public partial class EnemyManager : Node
{
    // Singleton
    public static EnemyManager singleton;

    [Export] private PackedScene[] enemyScenes;

    [Export] private Node2D enemyLocation;

    private EnemyBase currentEnemy;

    public override void _Ready()
    {
        base._Ready();

        if (singleton == null) singleton = this;

        //enemyScene = GD.Load<PackedScene>("res://Scenes/Characters/Enemies/BasicEnemy.tcsn");
        SpawnEnemy();
    }

    public async void Attack(float _baseDamage)
    {
        float damage = _baseDamage;
        if (currentEnemy.TakeDamage(damage))
        {
            // if the enemy dies wait a third of a second then spawn another enemy
            await Task.Delay(TimeSpan.FromMilliseconds(2000));
            currentEnemy.QueueFree();
            SpawnEnemy();
        }
    }

    private void SpawnEnemy()
    {
        EnemyBase enemy = CreateEnemyObject(0);
        currentEnemy = enemy;
    }

    private EnemyBase CreateEnemyObject(int _enemyType)
    {
        EnemyBase enemy = enemyScenes[_enemyType].Instantiate<EnemyBase>();
        AddChild(enemy);

        return enemy;
    }
}
