using Godot;
using System.Threading.Tasks;

public partial class End_game : Area2D
{
    private AnimationPlayer _anim;

    public override void _Ready()
    {
        _anim = GetNode<AnimationPlayer>("../../HUD/Anim");
        BodyEntered += OnBodyEntered;
    }

    private async void OnBodyEntered(Node2D body)
    {
        if (body.IsInGroup("Player"))
        {
            Globals.lockFlag = true;
            Globals.control = false;
            Globals.checkpoint = false;

            _anim.Play("Close");

            await ToSignal(_anim, AnimationPlayer.SignalName.AnimationFinished);

            GetTree().ChangeSceneToFile("res://Training2.tscn");
        }
    }
}

