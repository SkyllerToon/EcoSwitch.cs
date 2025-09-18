using Godot;
using System.Collections.Generic;

public partial class GroundSpawner : Node
{
    [Export] public Godot.Collections.Array<PackedScene> GroundScenes { get; set; } = new();

    [Export] public int PoolSize = 8;
    [Export] public float SegmentLength = 16f;
    [Export] private Node2D Player;

    // Sequência da ordem das cenas (índices de GroundScenes)
    [Export] public Godot.Collections.Array<int> SceneOrder { get; set; } = new();

    // Ativar/desativar embaralhamento
    [Export] public bool RandomizeOrder = false;

    private List<Node2D> _segments = new List<Node2D>();
    private List<int> _currentOrder = new List<int>();
    private int _orderIndex = 0;

    public override void _Ready()
    {
        if (GroundScenes.Count == 0)
        {
            GD.PrintErr("⚠ Nenhuma cena adicionada em GroundScenes!");
            return;
        }

        // Se não definir SceneOrder no editor, cria ordem padrão (0,1,2,...)
        if (SceneOrder.Count == 0)
        {
            for (int i = 0; i < GroundScenes.Count; i++)
                SceneOrder.Add(i);
        }

        ResetOrder();

        // Inicializa os primeiros segmentos
        for (int i = 0; i < PoolSize; i++)
        {
            var ground = InstantiateNextScene();
            ground.Position = new Vector2(i * SegmentLength, 0);
            AddChild(ground);
            _segments.Add(ground);
        }
    }

    public override void _Process(double delta)
    {
        var first = _segments[0];
        if (Player.GlobalPosition.X - first.GlobalPosition.X > SegmentLength)
        {
            _segments.RemoveAt(0);
            first.QueueFree();

            var newGround = InstantiateNextScene();
            newGround.Position = new Vector2(_segments[^1].Position.X + SegmentLength, 0);
            AddChild(newGround);
            _segments.Add(newGround);
        }
    }

    private Node2D InstantiateNextScene()
    {
        if (_orderIndex >= _currentOrder.Count)
        {
            ResetOrder();
        }

        int sceneIndex = _currentOrder[_orderIndex];
        _orderIndex++;

        return GroundScenes[sceneIndex].Instantiate<Node2D>();
    }

    private void ResetOrder()
    {
        _currentOrder.Clear();
        foreach (var idx in SceneOrder)
            _currentOrder.Add(idx);

        if (RandomizeOrder)
            Shuffle(_currentOrder);

        _orderIndex = 0;
    }

    private void Shuffle(List<int> list)
    {
        var rng = new RandomNumberGenerator();
        rng.Randomize();

        for (int i = list.Count - 1; i > 0; i--)
        {
            int j = rng.RandiRange(0, i);
            (list[i], list[j]) = (list[j], list[i]);
        }
    }
}