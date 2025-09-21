using Godot;
using System.Threading.Tasks;

public partial class Menu : CanvasLayer
{
    [Export] private VBoxContainer holder;
    [Export] private VBoxContainer holder2;
    [Export] private AudioStreamPlayer opensfx;
    [Export] private AudioStreamPlayer pressedsfx;
    [Export] private Button resume;
    [Export] private Button options;
    [Export] private Button sound;
    [Export] private Button back;
    [Export] private Button quit;

    public override void _Ready()
    {
        if (Globals.sound)
            sound.Text = "SOUND ON";
        else
            sound.Text = "SOUND OFF";
    }

    public override void _UnhandledInput(InputEvent @event)
    {
        if (@event.IsActionPressed("ui_cancel") && !Globals.help)
        {
            if (!Visible)
                ShowMenu();
            else
                OnResumePressedAsync();
        }
    }

    private void ShowMenu()
    {
        Globals.pause = true;

        if (Globals.sound)
            opensfx?.Play();

        Visible = true;

        resume.GrabFocus();

        GetTree().Paused = true;
    }

    private async void OnResumePressedAsync()
    {
        Globals.pause = false;

        await PlayPressedSfxAsync();

        GetTree().Paused = false;
        Visible = false;
    }

    private async void OnOptionsPressedAsync()
    {
        await PlayPressedSfxAsync();

        if (holder2 != null)
        {
            holder2.Visible = true;
            sound.GrabFocus();
        }

        holder.Visible = false;
    }

    private async void OnSoundPressed()
    {
        if (Globals.sound)
        {
            Globals.sound = false;
            sound.Text = "SOUND OFF";
        }
        else
        {
            Globals.sound = true;
            await PlayPressedSfxAsync();
            sound.Text = "SOUND ON";
        }
    }

    private async void OnBackPressedAsync()
    {
        await PlayPressedSfxAsync();

        if (holder2 != null)
            holder2.Visible = false;

        holder.Visible = true;
        resume.GrabFocus();
    }

    private async void OnQuitPressedAsync()
    {
        await PlayPressedSfxAsync();

        GetTree().Paused = false;
        Globals.control = false;
        Globals.pause = false;

        GetTree().ChangeSceneToFile("res://titlescreen.tscn");
    }

    private async Task PlayPressedSfxAsync()
    {
        if (Globals.sound && pressedsfx != null)
        {
            pressedsfx.Play();
            if (pressedsfx.Playing)
                await ToSignal(pressedsfx, "finished");
        }
    }
}
