using Godot;
using System;
using System.Collections.Generic;

public partial class DialogManager : Node
{
    // === Nó e cena de diálogo ===
    private PackedScene _dialogScene = GD.Load<PackedScene>("res://Áreas/dialog.tscn");
    private Dialog _dialog;

    // === Controle de mensagens ===
    private List<string> _messageLines = new List<string>();
    private int _currentLine = 0;
    private Vector2 _dialogPosition = Vector2.Zero;

    public static bool Stop = false;
    private bool _messageActive = false;
    private bool _canAdvance = false;

    // === Inicia a mensagem ===
    public void StartMessage(Vector2 position, List<string> lines)
    {
        if (_dialog != null)
            return;

        _messageLines = lines;
        _dialogPosition = position;
        _currentLine = 0;
        ShowText();
        _messageActive = true;
        Stop = false;
    }

    // === Mostra a linha atual ===
    private void ShowText()
    {
        _dialog = _dialogScene.Instantiate<Dialog>();
        _dialog.TextFinished += AllText;

        GetTree().Root.AddChild(_dialog);
        _dialog.GlobalPosition = _dialogPosition;
        _dialog.DisplayText(_messageLines[_currentLine]);

        _canAdvance = false;
    }

    // === Sinal de término de texto ===
    private void AllText()
    {
        _canAdvance = true;
    }

    // === Captura input não tratado ===
    public override void _UnhandledInput(InputEvent @event)
    {
        if (@event.IsActionPressed("ui_up") && _messageActive && _canAdvance)
        {
            if (_dialog != null)
            {
                _dialog.QueueFree();
                _dialog = null;
            }

            _currentLine++;

            if (_currentLine >= _messageLines.Count)
            {
                _messageActive = false;
                _currentLine = 0;
                return;
            }

            ShowText();
        }
    }

    // === Encerra a mensagem ===
    public void EndMessage()
    {
        if (_dialog != null && _dialog.GlobalPosition == _dialogPosition)
        {
            _messageActive = false;
            _currentLine = 0;
            _dialogPosition = Vector2.Zero;
        }

        if (_dialog != null)
        {
            _dialog.QueueFree();
            _dialog = null;
        }
    }
}

