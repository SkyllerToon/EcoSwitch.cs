using Godot;

public partial class Hitbox : Area2D
{
    private void OnBodyEntered(Node2D body)
    {
        if (body is CharacterBody2D player && player.IsInGroup("Player") && Globals.animal == 1)
        {
            if (!player.IsOnFloor())
                player.Velocity = new Vector2(player.Velocity.X, -200);

            GetNode<Node2D>("../Sprite2").Visible = true;
            GetNode<AnimationPlayer>("../Anim2").Play("Poof");
            GetNode<AnimationPlayer>("../Anim").Play("IBee-hurt");
        }
    }
}

