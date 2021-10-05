using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class EffectManager : MonoBehaviour {
    [SerializeField] private Tilemap objects;

    [SerializeField] private ElementsTable table;

    [SerializeField] private GameObject lightPrefab;

    [SerializeField] private GameObject effectPrefab;

    [SerializeField] private Sprite[] spillSprites;

    public static EffectManager instance;

    private System.Random random = new System.Random();

    private Dictionary<Vector2Int, GameObject> lights = new Dictionary<Vector2Int, GameObject>();

    private Grid grid;

    private void Awake() {
        instance = this;
        grid = GetComponent<Grid>();
    }

    private void Start() {
        for (int i = -100; i <= 100; ++i) {
            for (int j = -100; j <= 100; ++j) {
                Vector2Int key = new Vector2Int(i, j);
                Vector3Int pos = new Vector3Int(i, j, 0);
                Sprite sprite = objects.GetSprite(pos);
                if (sprite != null) {
                    Color color = Color.white;
                    foreach (var ele in table.elements) {
                        if (ele.collectableSprite == sprite) {
                            color = ele.color;
                            break;
                        } 
                    }

                    if (color == Color.white) {
                        continue;
                    } 

                    GameObject light = Instantiate(lightPrefab);
                    LightController controller = light.GetComponentInChildren<LightController>();
                    controller.SetColor(color);
                    light.transform.parent = transform;
                    light.transform.position = TilePosToWorldPos(pos);
                    light.name = "light";

                    lights.Add(key, light);
                } 
            }
        }
    }

    private void Update() {
        for (int i = -100; i <= 100; ++i) {
            for (int j = -100; j <= 100; ++j) {
                Vector2Int key = new Vector2Int(i, j);
                Vector3Int pos = new Vector3Int(i, j, 0);
                Sprite sprite = objects.GetSprite(pos);
                if (sprite == null && lights.ContainsKey(key)) {
                    GameObject light = lights[key];
                    lights.Remove(key);
                    LightController controller = light.GetComponentInChildren<LightController>();
                    controller.TurnOff();
                } 
            }
        }
    }
    
    public void Spill(Vector3 pos, ElementType type) {
        GameObject spillObject = Instantiate(effectPrefab);
        Color color = Color.white;
        foreach (var ele in table.elements) {
            if (ele.type == type) {
                color = ele.color;
                break;
            } 
        }

        var ec = spillObject.GetComponent<EffectTileController>();
        spillObject.transform.position = pos;
        spillObject.transform.parent = transform;
        spillObject.name = "spill";
        spillObject.GetComponent<SpriteRenderer>().sortingOrder = 1;
        ec.StartUp(spillSprites[random.Next(0, spillSprites.Length)], color);
    }

    public void Erode(Vector3 pos, Sprite sprite) {
        GameObject dummyObject = Instantiate(effectPrefab);

        var ec2 = dummyObject.GetComponent<EffectTileController>();
        dummyObject.transform.position = pos;
        dummyObject.transform.parent = transform;
        dummyObject.name = "dummy";
        dummyObject.GetComponent<SpriteRenderer>().sortingOrder = 0;
        ec2.StartUp(sprite, Color.white);
    }

    private Vector3 TilePosToWorldPos(Vector3Int pos) {
        Vector3 worldPos = grid.CellToWorld(pos);
        return worldPos;
    }
}
