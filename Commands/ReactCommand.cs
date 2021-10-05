using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ReactCommand : ICommand {

    private PlayerController controller = null;
    private ElementType from;

    private ElementType to;

    private Tilemap layer = null;

    private TileBase tile;

    private Vector3Int position;

    public ReactCommand(PlayerController con, ElementType t1, ElementType t2) {
        controller = con;
        from = t1; to = t2;
    }

    public ReactCommand(Tilemap ly, TileBase t, Vector3Int pos) {
        layer = ly; tile = t; position = pos;
    }

    public ReactCommand() {}

    public void Execute() {
        if (layer != null) {
            layer.SetTile(position, null);
        }

        if (controller != null) {
            controller.element = to;
        }
    }

    public void Redo() {
        if (layer != null) {
            layer.SetTile(position, tile);
        }

        if (controller != null) {
            controller.element = from;
        }
    }
}
