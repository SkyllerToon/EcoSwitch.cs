using Godot;
using System;

public partial class Respawn : Area2D
{
    private PackedScene enemyScene = ResourceLoader.Load<PackedScene>("res://Actors/Enemie.tscn");
    private Node2D enemy;                 
    private Vector2 startPosition;      

    public override void _Ready()
    {
        BodyExited += OnBodyExited;

        enemy = GetNodeOrNull<Node2D>("Enemie");
        if (enemy != null)
        {
            startPosition = enemy.GlobalPosition;
            enemy.TreeExiting += OnEnemyDied;
        }
    }

    private void OnEnemyDied()
    {
        enemy = null;
    }

    private void SpawnEnemy()
    {
        if (enemy != null)
            return;

        enemy = enemyScene.Instantiate<Node2D>();
        enemy.GlobalPosition = startPosition;

        GetParent().AddChild(enemy);
        QueueFree();
    }

    private void OnBodyExited(Node body)
    {
        if (body.IsInGroup("Player"))
        {
            if (enemy == null)
                CallDeferred(nameof(SpawnEnemy));
        }
    }
}
