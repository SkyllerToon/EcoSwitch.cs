using Godot;
using System;

public partial class Squirrel_floor : StaticBody2D
{
    public override void _Process(double delta)
    {
        if (Globals.animal == 2)
        {
            SetCollisionLayerValue(8, true);
        }
        else
        {
            SetCollisionLayerValue(8, false);
        }
    }
}

