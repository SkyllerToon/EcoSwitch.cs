using Godot;
using System.Threading.Tasks;

public partial class Note : Area2D
{
    // === Configurações ===
    [Export] public int note2 { get; set; } = 5;

    // === Estados ===
    private bool collected = false;
    private int incrementIndex = 0;

    // === Nós ===
    [Export] private AnimationPlayer anim;
    [Export] private AudioStreamPlayer trashSfx;
    [Export] private AudioStreamPlayer counterSfx;
    [Export] private CollisionShape2D collisionShape;
    [Export] private Timer incrementTimer;

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

