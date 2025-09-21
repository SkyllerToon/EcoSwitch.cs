using Godot;

public partial class Hitbox2 : Area2D
{
    private void OnAreaEntered(Area2D area)
    {
        if (area.IsInGroup("Attack"))
        {
            GetNode<Node2D>("../Sprite2").Visible = true;
            GetNode<AnimationPlayer>("../Anim2").Play("Poof");
            GetNode<AnimationPlayer>("../Anim").Play("IBee-hurt");
        }
    }
}
