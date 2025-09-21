using Godot;
using System;

public partial class FrogFloor : StaticBody2D
{
     public override void _Process(double delta)
    {
        if (Globals.animal == 4)
        {
            SetCollisionLayerValue(9, true);
        }
        else
        {
            SetCollisionLayerValue(9, false);
        }
    }
}
