using Godot;

public partial class Plataform : Node2D
{
    private const float WAIT_DURATION = 1.0f;

    [Export] public float MoveSpeed { get; set; } = 1.0f;
    [Export] public float Distance { get; set; } = 192;
    [Export] public bool MoveHorizontal { get; set; } = true;

    private AnimatableBody2D _platform;
    private Vector2 _follow = Vector2.Zero;
    private float _platformCenter = 48;

    public override void _Ready()
    {
        _platform = GetNode<AnimatableBody2D>("Plataform");
    }

    public override void _PhysicsProcess(double delta)
    {
        _platform.Position = _platform.Position.Lerp(_follow, 0.5f);
    }

    private void MovePlatform()
    {
        Vector2 moveDirection = MoveHorizontal ? Vector2.Right * Distance : Vector2.Up * Distance;
        float duration = moveDirection.Length() / (MoveSpeed * _platformCenter);

        Tween platformTween = CreateTween()
            .SetLoops()
            .SetTrans(Tween.TransitionType.Linear)
            .SetEase(Tween.EaseType.InOut);

        platformTween.TweenProperty(this, "Follow", moveDirection, duration);
        platformTween.TweenProperty(this, "Follow", Vector2.Zero, duration).SetDelay(WAIT_DURATION);
    }

    private void _on_active_body_entered(Node2D body)
    {
        if (body.Name == "Player")
        {
            MovePlatform();
        }
    }

    // Propriedade para o tween acessar
    public Vector2 Follow
    {
        get => _follow;
        set => _follow = value;
    }
}

