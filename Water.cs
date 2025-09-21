using Godot;
using System;

public partial class Water : Area2D
{
    private bool playerSwim = false;

    private AudioStreamPlayer2D waterSfx;
    private AudioStreamPlayer swin;
    private AudioStreamPlayer splash;

    public override void _Ready()
    {
        waterSfx = GetNode<AudioStreamPlayer2D>("Water_sfx");
        swin = GetNode<AudioStreamPlayer>("Swin");
        splash = GetNode<AudioStreamPlayer>("Splash");

        BodyEntered += OnBodyEntered;
        BodyExited += OnBodyExited;
    }

    public override void _PhysicsProcess(double delta)
    {
        if (!Globals.sound)
        {
            waterSfx.Stop();
        }
        else if (!waterSfx.Playing)
        {
            waterSfx.Play();
        }

        if (swin.Playing && !Globals.sound)
        {
            swin.Stop();
        }
        else if (!swin.Playing && playerSwim)
        {
            swin.Play();
        }
    }

    private void OnBodyEntered(Node2D body)
    {
        if (!body.IsInGroup("Player")) return;

        if (Globals.sound)
        {
            splash.Play();
            swin.Play();
        }

        body.Set("water", true);
        playerSwim = true;
    }

    private void OnBodyExited(Node2D body)
    {
        if (!body.IsInGroup("Player")) return;

        if (Globals.sound && IsInsideTree())
        {
            splash.Play();
        }

        swin.Stop();
        body.Set("water", false);
        playerSwim = false;
    }
}

