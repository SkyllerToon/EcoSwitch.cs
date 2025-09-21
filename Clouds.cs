using Godot;

public partial class Clouds : TileMapLayer
{
    public override void _PhysicsProcess(double delta)
    {
        var tileSet = TileSet;

        if (tileSet == null)
            return;

        if (Globals.cloud)
        {
            tileSet.SetPhysicsLayerCollisionLayer(0, 10);
        }
        else
        {
            tileSet.SetPhysicsLayerCollisionLayer(0, 0);
        }
    }
}

