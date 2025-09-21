using Godot;

public partial class Lava : Area2D
{
    private void OnBodyEntered(Node2D body)
    {
        body.Set("dmg", true);
    }

    private void OnBodyExited(Node2D body)
    {
        body.Set("dmg", false);
    }
}
