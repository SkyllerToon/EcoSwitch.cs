using Godot;
using System;

public partial class Wind : Area2D
{
    private AudioStreamPlayer windSfx;

    public override void _Ready()
    {
        windSfx = GetNode<AudioStreamPlayer>("Wind_sfx");
    }

    public override void _PhysicsProcess(double delta)
    {
        if (!Globals.sound)
        {
            windSfx.Stop();
        }
        else if (!windSfx.Playing)
        {
            windSfx.Play();
        }
    }

    private void _on_body_entered(Node2D body)
    {
        if (body.Name == "Player")
        {
            body.Set("wind", true);
        }
    }

    private void _on_body_exited(Node2D body)
    {
        if (body.Name == "Player")
        {
            body.Set("wind", false);
            body.Set("wind_out", true);
        }
    }
}

