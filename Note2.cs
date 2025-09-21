using Godot;
using System.Threading.Tasks;

public partial class Note2 : Area2D
{
    // === Configurações ===
    [Export] public int note2 { get; set; } = 5;

    // === Estados ===
    private bool collected = false;
    private int incrementIndex = 0;

    // === Nós ===
    private AnimationPlayer anim;
    private AudioStreamPlayer trashSfx;
    private AudioStreamPlayer counterSfx;
    private CollisionShape2D collisionShape;
    private Timer incrementTimer;

    public override void _Ready()
    {
        anim = GetNode<AnimationPlayer>("Anim");
        trashSfx = GetNode<AudioStreamPlayer>("Trash_sfx");
        counterSfx = GetNode<AudioStreamPlayer>("Counter");
        collisionShape = GetNode<CollisionShape2D>("Collision");
        incrementTimer = GetNode<Timer>("Increment");

        BodyEntered += OnBodyEntered;
        anim.AnimationFinished += OnAnimAnimationFinished;
        incrementTimer.Timeout += OnIncrementTimeout;
    }

    private void OnBodyEntered(Node2D body)
    {
        if (collected || !body.IsInGroup("Player"))
            return;

        collected = true;
        Globals.notehud = true;
        anim.Play("Collect");

        if (Globals.sound)
        {
            trashSfx.Play();
            if (note2 > 1)
                counterSfx.Play();
        }

        collisionShape.CallDeferred("queue_free");

        incrementIndex = 0;
        incrementTimer.Start();
    }

    private void OnAnimAnimationFinished(StringName animName)
    {
        if (animName == "Collect")
            QueueFree();
    }

    private void OnIncrementTimeout()
    {
        if (incrementIndex < note2)
        {
            Globals.note += 1;
            incrementIndex++;
        }
        else
        {
            incrementTimer.Stop();
            if (counterSfx.Playing)
                counterSfx.Stop();
        }
    }

    // === Save / Load ===
    public Godot.Collections.Dictionary Save()
    {
        return new Godot.Collections.Dictionary
        {
            ["position"] = Position,
            ["rotation"] = Rotation,
            ["active"] = !collected
        };
    }

    public void Carregar(Godot.Collections.Dictionary data)
    {
        Position = (Vector2)data["position"];
        Rotation = (float)data["rotation"];
        collected = !(bool)data["active"];

        if (collected)
            QueueFree();
    }
}

