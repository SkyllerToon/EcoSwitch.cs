using Godot;
using System.Threading.Tasks;

public partial class Checkpoint : Area2D
{
    private bool check = false;
    private AnimatedSprite2D anim;
    private AudioStreamPlayer bearSfx;

    public override void _Ready()
    {
        anim = GetNode<AnimatedSprite2D>("Sprite");
        bearSfx = GetNode<AudioStreamPlayer>("Bear_sfx");

        BodyEntered += OnBodyEntered;
    }

    private async void OnBodyEntered(Node2D body)
    {
        if (body.Name == "Player" && !check)
        {
            Globals.checkpoint = true;
            check = true;

            Globals.save.SalvarCenario();

            if (Globals.sound)
                bearSfx.Play();

            anim.Play("Transition");

            await ToSignal(anim, AnimatedSprite2D.SignalName.AnimationFinished);

            anim.Play("On");
        }
    }
}

