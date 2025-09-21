using Godot;
using System.Threading.Tasks;

public partial class Titlescreen : Control
{
    [Export] private AudioStreamPlayer titlesfx;
    [Export] private AudioStreamPlayer pressed;
    [Export] private AnimationPlayer anim;
    [Export] private VBoxContainer holder;
    [Export] private VBoxContainer holder2;
    [Export] private Button start;
    [Export] private Button options;
    [Export] private Button sound;
    [Export] private Button back;
    [Export] private Button quit;


    public override void _Ready()
    {
        sound.Text = Globals.sound ? "SOUND ON" : "SOUND OFF";
        Globals.checkpoint = false;
        start.GrabFocus();
    }

    public override void _PhysicsProcess(double delta)
    {
        if (!Globals.sound)
            titlesfx.Stop();
        else
        {
            if (!titlesfx.Playing)
                titlesfx.Play();
        }
    }

    public async void OnStartPressed()
    {
        Globals.score = 0;
        Globals.note = 0;
        Globals.life = 2;
        Globals.animal = 1;

        _ = PlayPressedSfxAsync();

        anim.Play("Close");
        await ToSignal(anim, "animation_finished");

        if (IsInsideTree())
            GetTree().ChangeSceneToFile("res://Training3.tscn");
    }

    public async void OnOptionsPressed()
    {
        _ = PlayPressedSfxAsync();
        holder.Visible = false;
        anim.Play("Open_2");
        await ToSignal(GetTree().CreateTimer(0.1f), "timeout");
        holder2.Visible = true;
        await ToSignal(anim, "animation_finished");
        sound.GrabFocus();
    }

    public void OnSoundPressed()
    {
        if (Globals.sound)
        {
            Globals.sound = false;
            sound.Text = "SOUND OFF";
        }
        else
        {
            Globals.sound = true;
            sound.Text = "SOUND ON";
            _ = PlayPressedSfxAsync();
        }
    }

    public async void OnBackPressed()
    {
        _ = PlayPressedSfxAsync();
        holder2.Visible = false;
        anim.Play("Open_3");
        await ToSignal(GetTree().CreateTimer(0.1f), "timeout");
        holder.Visible = true;
        await ToSignal(anim, "animation_finished");
        start.GrabFocus();
    }

    public void OnExitPressed()
    {
        _ = PlayPressedSfxAsync();
        GetTree().Quit();
    }

    private async Task PlayPressedSfxAsync()
    {
        if (Globals.sound && pressed != null)
        {
            pressed.Play();
            if (pressed.Playing)
                await ToSignal(pressed, "finished");
        }
    }
}

