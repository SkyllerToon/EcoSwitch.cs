using Godot;
using System;
using System.Collections.Generic;

public partial class Training : Node2D
{
    [Export] private AudioStreamPlayer _backSfx;
    [Export] private AudioStreamPlayer _backSfx2;
    private ConfigFile _config = new ConfigFile();
    private Godot.Collections.Array<Node> _nos;
    private Variant _dados;

    public override void _Ready()
    {
        Globals.lockFlag = false;
        Globals.lifehud = true;
        Globals.notehud = true;
        Globals.save = this;
        Globals.control = true;
    }

    public override void _PhysicsProcess(double delta)
    {
        if (_backSfx == null && _backSfx2 == null)
            return;

        if (!Globals.sound || Globals.forget)
        {
            _backSfx.Stop();
            _backSfx2.Stop();
        }
        else
        {
            if (!_backSfx.Playing)
                _backSfx.Play();
            if (!_backSfx2.Playing)
                _backSfx2.Play();
        }
    }

    public void SalvarCenario()
    {
        _nos = GetChildren();
        foreach (Node no in _nos)
        {
            if (no.HasMethod("save"))
            {
                _dados = no.Call("save");
                _config.SetValue("Nos", no.Name, _dados);
            }
        }

        _config.SetValue("Globals", "animal", Globals.animal);

        _config.Save("user://cenario.save");
    }

    public void CarregarCenario()
    {
        if (_config.Load("user://cenario.save") == Error.Ok)
        {
            _nos = GetChildren();
            foreach (Node no in _nos)
            {
                if (no != null && no.HasMethod("carregar"))
                {
                    if (_config.HasSectionKey("Nos", no.Name))
                    {
                        _dados = _config.GetValue("Nos", no.Name);
                        var dict = (Godot.Collections.Dictionary)_dados;

                        if (dict.ContainsKey("active") && (bool)dict["active"])
                            no.Call("carregar", _dados);
                        else
                            no.QueueFree();
                    }
                }
            }

            Globals.animal = (int)_config.GetValue("Globals", "animal", 0);
        }
    }
}
