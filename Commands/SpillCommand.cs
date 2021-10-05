using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class SpillCommand : ICommand {
    private Tilemap layer = null;
    private Vector3Int position = Vector3Int.zero;    

    private TileBase tile = null;

    public ICommand secondCommand;

    public SpillCommand(Tilemap ly, TileBase t, Vector3Int pos) {
        layer = ly;
        position = pos;
        tile = t;
    }

    public SpillCommand() {}

    public void Execute() {
        if (layer != null) {
            layer.SetTile(position, null);
        }
    }

    public void Redo() {
        if (layer != null) {
            layer.SetTile(position, tile);
        }

        if (secondCommand != null) {
            secondCommand.Redo();
        }
    }
}
