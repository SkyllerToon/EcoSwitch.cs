using Godot;

public partial class Help_screen : Area2D
{
    private bool _helpActive = false;
    private AudioStreamPlayer _open;
    private AudioStreamPlayer _pressed;
    private Control _helpScreen;

    public override void _Ready()
    {
        _open = GetNode<AudioStreamPlayer>("Open_sfx");
        _pressed = GetNode<AudioStreamPlayer>("Pressed_sfx");
        _helpScreen = GetNode<Control>("Help_screen");

        _helpScreen.Visible = false;

        BodyEntered += OnBodyEntered;
    }

    public override void _Process(double delta)
    {
        if (Input.IsActionJustPressed("ui_accept") && _helpActive)
        {
            _helpActive = false;
            Globals.help = false;
            QueueFree();
        }
    }

    private void OnBodyEntered(Node2D body)
    {
        if (body.Name == "Player")
        {
            _helpScreen.Visible = true;
            _helpActive = true;
            Globals.help = true;
            GetTree().Paused = true;
        }
    }
}

