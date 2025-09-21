using Godot;
using System;

public partial class Enemie : CharacterBody2D
{
    // === Configurations ===
    private const float SPEED = 150f;
    private const float JUMP_VELOCITY = -150f;

    // === States ===
    private int direction = -1;
    private bool dead = false;
    private bool poof = false;

    // === Nodes ===
    [Export] private CollisionShape2D collision;
    [Export] private CollisionShape2D collision2;
    [Export] private CollisionShape2D collision3;
    [Export] private Sprite2D sprite;
    [Export] private AnimationPlayer anim;
    [Export] private AudioStreamPlayer hurtSfx;
    [Export] private AudioStreamPlayer2D walkSfx;

    // === Lifecycle ===
    public override void _PhysicsProcess(double delta)
    {
        if (poof)
        {
            sprite.Position += new Vector2(-60, -60) * (float)delta; 
        }

        HandleSound();
    }

    // === Helpers ===
    private void HandleSound()
    {
        if (!Globals.sound)
        {
            walkSfx.Stop();
        }
        else if (!walkSfx.Playing)
        {
            walkSfx.Play();
        }
    }

    // === Animation Events ===
    private void _OnAnimAnimationStarted(StringName animName)
    {
        if (animName == "Poof")
        {
            poof = true;
        }
        if (animName == "IBee-hurt")
            {
                Globals.score += 40;
                Die();
            }
    }

    private void _OnAnimAnimationFinished(StringName animName)
    {
        if (animName == "IBee-hurt")
        {
            hurtSfx.Stop();
            QueueFree();
        }
    }

    // === Actions ===
    private void Die()
    {
        PlayHurt();
        DisableEnemy();
    }

    private void PlayHurt()
    {
        if (Globals.sound && hurtSfx != null)
            hurtSfx.Play();
    }

    private void DisableEnemy()
    {
        direction = 0;
        dead = true;

        foreach (var col in new[] { collision, collision2, collision3 })
        {
            if (col != null)
                col.QueueFree();
        }
    }
}

