using Godot;
using System;
using System.Threading.Tasks;

public partial class Dialog : MarginContainer
{
    // === Nós ===
    private Label _label;
    private Timer _timer;

    // === Constantes ===
    private const float MAX_WIDTH = 256f;

    // === Estados ===
    private string _text = "";
    private int _letter = 0;

    private float _letterTimer = 0.02f;
    private float _spaceTimer = 0.02f;
    private float _pontTimer = 0.02f;

    // === Sinais ===
    [Signal]
    public delegate void TextFinishedEventHandler();

    public override void _Ready()
    {
        _label = GetNode<Label>("Label_margin/Label");
        _timer = GetNode<Timer>("Timer");

        // Conecta o sinal do timer
        _timer.Timeout += OnTimerTimeout;
    }

    public override void _PhysicsProcess(double delta)
    {
        // Para o som da árvore se sound estiver desligado
        if (!Globals.sound)
        {
            var tree = GetNode<AudioStreamPlayer>("Tree");
            tree?.Stop();
        }
    }

    // === Exibe o texto completo ===
    public async void DisplayText(string textDisplay)
    {
        _text = textDisplay;
        _letter = 0;
        _label.Text = textDisplay;

        await ToSignal(this, "resized");

        CustomMinimumSize = new Vector2(Mathf.Min(Size.X, MAX_WIDTH), CustomMinimumSize.Y);

        if (Size.X > MAX_WIDTH)
        {
            _label.AutowrapMode = TextServer.AutowrapMode.Word;
            await ToSignal(this, "resized");
            await ToSignal(this, "resized");
            CustomMinimumSize = new Vector2(CustomMinimumSize.X, Size.Y);
        }

        GlobalPosition = new Vector2(GlobalPosition.X - Size.X / 2, GlobalPosition.Y - Size.Y - 24);

        _label.Text = "";
        DisplayLetter();
    }

    // === Exibe uma letra por vez ===
    private void DisplayLetter()
    {
        if (_letter >= _text.Length)
        {
            DialogManager.Stop = true;
            EmitSignal(SignalName.TextFinished);
            return;
        }

        char currentChar = _text[_letter];
        _label.Text += currentChar;

        // Define o timer de acordo com o caractere
        switch (currentChar)
        {
            case '!':
            case '?':
            case '.':
            case ',':
                _timer.WaitTime = _pontTimer;
                break;
            case ' ':
                _timer.WaitTime = _spaceTimer;
                break;
            default:
                _timer.WaitTime = _letterTimer;
                break;
        }

        _letter++;
        _timer.Start();
    }

    private void OnTimerTimeout()
    {
        DisplayLetter();
    }
}
