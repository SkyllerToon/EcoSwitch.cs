using Godot;
using System;
using System.Collections.Generic;

public partial class Sswitch : CanvasLayer
{
    private AudioStreamPlayer pressedSfx;
    [Export] TextureRect SkillButton;
    [Export] TextureRect SkillButton2;
    [Export] TextureRect SkillButton3;
    [Export] TextureRect SkillButton4;

    public override void _Ready()
    {
        pressedSfx = GetNodeOrNull<AudioStreamPlayer>("Pressed_sfx");
    }

    public override void _PhysicsProcess(double delta)
    {
        if (Globals.animal == 1 || Globals.released)
            SkillButton.Modulate = new Color(0.6f, 0.6f, 0.6f, 1f);
        else
            SkillButton.Modulate = new Color(1f, 1f, 1f, 1f);

        if (Globals.animal == 2 || Globals.released)
            SkillButton2.Modulate = new Color(0.6f, 0.6f, 0.6f, 1f);
        else
            SkillButton2.Modulate = new Color(1f, 1f, 1f, 1f);

        if (Globals.animal == 3 || Globals.released)
            SkillButton3.Modulate = new Color(0.6f, 0.6f, 0.6f, 1f);
        else
            SkillButton3.Modulate = new Color(1f, 1f, 1f, 1f);

        if (Globals.animal == 4 || Globals.released)
            SkillButton4.Modulate = new Color(0.6f, 0.6f, 0.6f, 1f);
        else
            SkillButton4.Modulate = new Color(1f, 1f, 1f, 1f);
    }

    public override void _UnhandledInput(InputEvent @event)
    {
        if (@event.IsActionPressed("action_a") && !Globals.pause && Globals.unlock > 1 && !Globals.lockFlag && !Globals.released)
        {
            if (Globals.animal != 1)
            {
                if (Globals.sound)
                    pressedSfx.Play();
                Globals.animal = 1;
                Globals.transform = true;
            }
        }
        else if (@event.IsActionPressed("action_z") && !Globals.pause && Globals.unlock > 1 && !Globals.lockFlag && !Globals.released)
        {
            if (Globals.animal != 2)
            {
                if (Globals.sound)
                    pressedSfx.Play();
                Globals.animal = 2;
                Globals.transform = true;
            }
        }
        else if (@event.IsActionPressed("action_x") && !Globals.pause && Globals.unlock > 1 && !Globals.lockFlag && !Globals.released)
        {
            if (Globals.animal != 3)
            {
                if (Globals.sound)
                    pressedSfx.Play();
                Globals.animal = 3;
                Globals.transform = true;
            }
        }
        else if (@event.IsActionPressed("action_s") && !Globals.pause && Globals.unlock > 1 && !Globals.lockFlag && !Globals.released)
        {
            if (Globals.animal != 4)
            {
                if (Globals.sound)
                    pressedSfx.Play();
                Globals.animal = 4;
                Globals.transform = true;
            }
        }
    }
}
