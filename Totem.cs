using Godot;
using System;

public partial class Totem : Area2D
{
    private bool collected = false;
    [Export] private CollisionShape2D collision;

    private void OnBodyEntered(Node2D body)
    {
        if (collected || !body.IsInGroup("Player"))
            return;

        collected = true;
        Visible = false;

        collision.CallDeferred("queue_free");

        Globals.totem = true;
        QueueFree();
    }
}
