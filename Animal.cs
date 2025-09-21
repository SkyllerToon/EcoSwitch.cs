using Godot;
using System.Threading.Tasks;

public partial class Animal : AnimatedSprite2D
{
    private Node2D _box;
    private AudioStreamPlayer _speakSquirrel;

    public override void _Ready()
    {
        _box = GetNode<Node2D>("box");
        _speakSquirrel = GetNode<AudioStreamPlayer>("Speak_squirrel");

        var area = GetNode<Area2D>("Area");
        area.BodyEntered += OnAreaBodyEntered;
    }

    private async void OnAreaBodyEntered(Node2D body)
    {
        if (body.IsInGroup("Player"))
        {
            Globals.unlock = 2;
            _box.Visible = false;

            if (Globals.sound)
                _speakSquirrel.Play();
            Play("Entered");
            await ToSignal(this, AnimatedSprite2D.SignalName.AnimationFinished);

            QueueFree();
        }
    }
}

