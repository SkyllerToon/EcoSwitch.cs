using Godot;

public partial class Hud_manager : CanvasLayer
{
    // === Configurações ===
    private const int SPEED = 1;

    // === Estados ===
    private enum State { IDLE, CHECK_REDUCTION, REDUCING, FINISH_REDUCTION }
    private State _state = State.IDLE;
    private int _counter = 0;
    private int _target = 0;
    private int _begin = 0;
    private int totem = 10;
    private double _scoreTimer = 0.0;
    [Export] private Label _scoreCounter;

    // === Nós ===
    [Export] private Label _noteCounter;
    [Export] private AnimatedSprite2D _heart;
    [Export] private AnimatedSprite2D _heart2;
    [Export] private AnimatedSprite2D _heart3;
    [Export] private Label _flyesCounter;
    [Export] private Label _owlcounter;
    [Export] private Sprite2D _noteHud;
    [Export] private Sprite2D _lifeHud;
    [Export] private BoxContainer owlHud;
    [Export] private Timer _flyesTimer;
    [Export] private Timer _noteTimer;
    [Export] private Timer _owlTimer;
    [Export] private AudioStreamPlayer _counterSfx;
    [Export] private AudioStreamPlayer _awardSfx;

    public override void _Ready()
    {
        _counter = Globals.note;
        UpdateLabels();
    }

    public override void _Process(double delta)
    {         
        if (Globals.totem && _owlTimer.IsStopped())
        {
            owlHud.Visible = true;
            _owlTimer.Start();
        }

        if (_counter != Globals.note)
            Globals.notehud = true;

        HandleHudVisibility();
        UpdateHearts();

        if (!Globals.pause && !Globals.lockFlag)
            _scoreTimer += delta;

        if (_scoreTimer >= 0.1)
        {
            _scoreTimer -= 0.1;
            Globals.score += 1;
            UpdateLabels();
        }

        switch (_state)
        {
            case State.IDLE:
                StateIdle();
                break;
            case State.CHECK_REDUCTION:
                StateCheckReduction();
                break;
            case State.REDUCING:
                StateReducing();
                break;
            case State.FINISH_REDUCTION:
                StateFinishReduction();
                break;
        }
    }

    // === State Handlers ===
    private void StateIdle()
    {
        _counter = Globals.note;
        UpdateLabels();

        if (Globals.note >= 100)
        {
            _begin = _counter;
            _target = _counter - 100;
            _state = State.CHECK_REDUCTION;
        }
    }

    private void StateCheckReduction()
    {
        PlayCounterSound();
        _state = State.REDUCING;
    }

    private void StateReducing()
    {
        Globals.notehud = true;
        Globals.lifehud = true;
        _counter -= SPEED;

        if (_counter <= _target)
        {
            _counter = _target;
            Globals.note = _target;
            _state = State.FINISH_REDUCTION;
        }

        UpdateLabels();
    }

    private void StateFinishReduction()
    {
        StopCounterSound();

        if (Globals.sound)
            _awardSfx.Play();

        Globals.flyes += 1;
        _state = State.IDLE;
    }

    // === Helpers ===
    private void HandleHudVisibility()
    {
        if (Globals.notehud || Globals.pause)
        {
            _noteHud.Visible = true;
            _noteCounter.Visible = true;
            _noteTimer.Start();
            Globals.notehud = false;
        }

        if (Globals.lifehud || Globals.pause)
        {
            _lifeHud.Visible = true;
            _flyesCounter.Visible = true;
            _flyesTimer.Start();
            Globals.lifehud = false;
        }
    }

    private void PlayCounterSound()
    {
        if (Globals.sound)
        {
            if (!_counterSfx.Playing)
                _counterSfx.Play();
        }
        else
        {
            if (_counterSfx.Playing)
                _counterSfx.Stop();
        }
    }

    private void StopCounterSound()
    {
        if (_counterSfx.Playing)
            _counterSfx.Stop();
    }

    private void UpdateLabels()
    {
        _noteCounter.Text = _counter.ToString("D2");
        _flyesCounter.Text = Globals.flyes.ToString("D2");
        _scoreCounter.Text = Globals.score.ToString("D4");
        _owlcounter.Text = totem.ToString("D2");
    }

    private void UpdateHearts()
    {
        switch (Globals.life)
        {
            case 2:
                _heart.Play("Full");
                _heart2.Play("Full");
                _heart3.Play("Full");
                break;
            case 1:
                _heart.Play("Full");
                _heart2.Play("Full");
                _heart3.Play("Transition");
                break;
            case 0:
                _heart.Play("Full");
                _heart2.Play("Transition");
                _heart3.Play("Void");
                break;
            default:
                if (Globals.life < 0)
                {
                    _heart.Play("Transition");
                    _heart2.Play("Void");
                    _heart3.Play("Void");
                }
                break;
        }
    }

    // === Signals ===
    private void OnNoteTimerTimeout()
    {
        _noteHud.Visible = false;
        _noteCounter.Visible = false;
    }

    private void OnFlyesTimerTimeout()
    {
        _lifeHud.Visible = false;
        _flyesCounter.Visible = false;
    }

    private void OnOwlTimerTimeout()
    {
        totem -= 1;
        if (totem <= 0)
        {
            owlHud.Visible = false;
            totem = 10;
            Globals.totem = false;
            _owlTimer.Stop();
        }
        UpdateLabels();          
    }
}

