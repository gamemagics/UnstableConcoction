using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapManager : MonoBehaviour {
    [SerializeField] private Levels levels;

    [SerializeField] private Grid grid;

    [SerializeField] private Tilemap groundMap;

    [SerializeField] private Tilemap objectMap;

    [SerializeField] private Tilemap decorationMap;

    [SerializeField] private ElementsTable table;

    public static MapManager instance;

    private Stack<ICommand> commandStack = new Stack<ICommand>();

    [HideInInspector] public Vector2Int endPoint;

    [SerializeField] private GameObject finalLightPrefab;

    private void Awake() {
        instance = this;

        GameObject map = Instantiate(levels.levelPrefabs[Booter.levelIndex]);
        // GameObject map = Instantiate(levels.levelPrefabs[1]); // for test
        map.transform.parent = transform;
        grid = map.GetComponentInChildren<Grid>();

        var layers = grid.GetComponentsInChildren<Tilemap>();
        if (layers == null || layers.Length < 3) {
            Debug.LogError("Wrong Map Configuration.");
            return;
        } 

        foreach (var layer in layers) {
            if (layer.gameObject.tag == "Ground") {
                groundMap = layer;
            }
            if (layer.gameObject.tag == "Object") {
                objectMap = layer;
            }
            if (layer.gameObject.tag == "Decoration") {
                decorationMap = layer;
            }
        }

        GameObject player = GameObject.FindWithTag("Player");
        LevelManager levelManager = map.GetComponentInChildren<LevelManager>();
        Vector2 playerPosition = TilePosToPlayerPos(new Vector3Int(levelManager.startPoint.x, levelManager.startPoint.y, 0));
        player.transform.position = new Vector3(playerPosition.x, playerPosition.y, 5);

        endPoint = levelManager.endPoint;
        GameObject finalLight = Instantiate(finalLightPrefab);
        var temp = TilePosToPlayerPos(new Vector3Int(endPoint.x, endPoint.y, 0));
        finalLight.transform.position = new Vector3(temp.x, temp.y - 0.16f, 0);
        finalLight.transform.parent = transform;
    }

    public bool Finish(Vector2 pos) {
        Vector3Int cellPos = PlayerPosToTilePos(pos);
        return ((Vector2Int)cellPos) == endPoint;
    }

    public void Spill(Vector2 pos, ElementType type) {
        Vector3Int cellPos = PlayerPosToTilePos(pos);
        Vector3Int groundPos = new Vector3Int(cellPos.x - 1, cellPos.y - 1, cellPos.z);

        Sprite sprite = groundMap.GetSprite(groundPos);
        bool react = false;
        foreach (var ele in table.elements) {
            if (ele.tileSprite == sprite) {
                react = Reactable(ele.type, type);
                break;
            }
        }

        if (react) {
            EffectManager.instance.Erode(new Vector3(pos.x, pos.y - grid.cellSize.y / 4, 0), sprite);
            TileBase tile = groundMap.GetTile(groundPos);
            SpillCommand command = new SpillCommand(groundMap, tile, groundPos);
            command.Execute();
            commandStack.Push(command);
        }
        else {
            SpillCommand command = new SpillCommand();
            commandStack.Push(command);
        }
    }

    public ElementType Collect(Vector2 pos, ElementType type) {
        Vector3Int cellPos = PlayerPosToTilePos(pos);
        Sprite sprite = objectMap.GetSprite(cellPos);
        
        if (sprite == null) {
            return type;
        }

        Color color = objectMap.GetColor(cellPos);

        bool react = false;
        ElementType another = ElementType.Arcane;
        foreach (var ele in table.elements) {
            if (ele.collectableSprite == sprite || ele.color == color) {
                another = ele.type;
                react = Reactable(another, type);
                break;
            }
        }

        if (react) {
            MusicEventManager.musicEvent.Invoke("collect");

            TileBase tile = objectMap.GetTile(cellPos);
            SpillCommand top = commandStack.Peek() as SpillCommand;
            ReactCommand cmd = new ReactCommand(objectMap, tile, cellPos);
            top.secondCommand = cmd;
            cmd.Execute();
            return React(type, another);
        }

        return type;
    }

    public bool Destroy(Vector2 pos, ElementType type) {
        Vector3Int cellPos = PlayerPosToTilePos(pos);
        Sprite sprite = objectMap.GetSprite(cellPos);

        bool flag = false;
        if (sprite == null) {
            flag = true;
        }
        else {
            Color color = objectMap.GetColor(cellPos);

            foreach (var ele in table.elements) {
                if (ele.collectableSprite == sprite) {
                    flag = true;
                    break;
                }
            
                if (ele.color == color) {
                    if (Reactable(ele.type, type)) {
                        flag = true;
                        break;
                    }
                }
            }
        }

        if (flag) {
            MusicEventManager.musicEvent.Invoke("open");
        }

        return flag; 
    }

    public Vector2 Move(Vector2 pos, Vector2 origin) {
        Vector3Int playerPos = PlayerPosToTilePos(origin);
        Vector3Int cellPos = PlayerPosToTilePos(pos);
        Vector3Int groundPos = new Vector3Int(cellPos.x - 1, cellPos.y - 1, cellPos.z);

        float dis = Vector3Int.Distance(playerPos, cellPos);
        if (dis < 0.01 || dis > 1.01 || groundMap.GetSprite(groundPos) == null) {
            return new Vector2(-1000, -1000);
        }
        
        return TilePosToPlayerPos(cellPos); 
    }

    private Vector3Int PlayerPosToTilePos(Vector2 pos) {
        Vector3Int cellPos = grid.WorldToCell(new Vector3(pos.x, pos.y, 0));
        return cellPos;
    }

    private Vector2 TilePosToPlayerPos(Vector3Int pos) {
        Vector3 playerPos = grid.CellToWorld(pos);
        return (Vector2)playerPos + new Vector2(0, grid.cellSize.y * 0.25f);
    }

    public void Redo() {
        if (commandStack.Count > 0) {
            ICommand cmd = commandStack.Peek();
            cmd.Redo();
            commandStack.Pop();
        }
    }

    private bool Reactable(ElementType t1, ElementType t2) {
        bool react = false;
        foreach (var r in table.rules) {
            if ((r.e1 == t1 && r.e2 == t2) || (r.e2 == t1 && r.e1 == t2)) {
                react = true;
                break;
            }
        }

        return react;
    }

    private ElementType React(ElementType t1, ElementType t2) {
        ElementType res = ElementType.Arcane;
        foreach (var r in table.rules) {
            if ((r.e1 == t1 && r.e2 == t2) || (r.e2 == t1 && r.e1 == t2)) {
                res = r.res;
                break;
            }
        }

        return res;
    }
}
