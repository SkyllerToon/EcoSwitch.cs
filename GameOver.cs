using Godot;
using System.Threading.Tasks;

public partial class GameOver : CanvasLayer
{
    [Export] private AudioStreamPlayer _gameoverSfx;
    [Export] private AudioStreamPlayer _pressed;
    [Export] private Button restart;
    [Export] private Button exit;
    private bool time = false;

    public override void _PhysicsProcess(double delta)
    {
        if (!Globals.forget)
        {
            _gameoverSfx.Stop();
        }
        else if (!_gameoverSfx.Playing)
        {
            _gameoverSfx.Play();
        }

        if (time)
        {
            _ = timeover();
            time = false;
        }

        if (Globals.gameover)
        {
            time = true;
            Globals.forget = true;
        }          
    }

    public async Task timeover()
    {
        await ToSignal(GetTree().CreateTimer(1f), "timeout");
        Visible = true;
        restart.GrabFocus();
    }

    // === Botão Restart ===
    public async void OnRestartPressed()
    {
        if (Globals.sound)
            _pressed.Play();

        if (_pressed.Playing)
            await ToSignal(_pressed, "finished");

        Globals.forget = false;
        Globals.score = 0;
        Globals.note = 0;
        Globals.life = 2;
        GetTree().ChangeSceneToFile("res://Training3.tscn");
    }

    // === Botão Quit ===
    public async void OnQuitPressed()
    {
        if (Globals.sound)
            _pressed.Play();

        if (_pressed.Playing)
            await ToSignal(_pressed, "finished");
        
        Globals.forget = false;
        Globals.score = 0;
        Globals.note = 0;
        Globals.life = 2;
        GetTree().ChangeSceneToFile("res://titlescreen.tscn");
    }
}

