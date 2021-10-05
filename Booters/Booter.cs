using UnityEngine;
using UnityEngine.EventSystems;

public class Booter : MonoBehaviour {
    [SerializeField] private int startSceneIndex;

    [SerializeField] private GameObject[] scenes = null;

    public static int levelIndex = 0;

    public static Booter instance = null;

    private IBooter currentBooter = null;
    private GameObject currentRoot = null;

    [SerializeField] private EventSystem eventSystem;

    [SerializeField] private Levels levels;

    private void Awake() {
        instance = this;
        DontDestroyOnLoad(transform.gameObject);
        DontDestroyOnLoad(Camera.main);
        DontDestroyOnLoad(eventSystem);
    }

    private void Start() {
        Change(startSceneIndex);
    }

    public void Change(int index) {
        if (scenes == null || startSceneIndex >= scenes.Length || startSceneIndex < 0) {
            Debug.LogErrorFormat("Wrong Scene Index {0}", startSceneIndex);
            return;
        }

        if (currentRoot != null) {
            currentBooter.Exit();
            currentBooter = null;

            DestroyImmediate(currentRoot, true);
            currentRoot = null;
        }

        if (index == 1 && (levelIndex < 0 || levelIndex >= levels.levelPrefabs.Length)) {
            levelIndex = 0;
            index = 4;
        }

        currentRoot = Instantiate(scenes[index]);
        currentRoot.transform.parent = transform;

        currentBooter = currentRoot.GetComponent<IBooter>();
        if (currentBooter == null) {
            Debug.LogError("Can't find Booter Script.");
            return;
        }

        currentBooter.StartUp(Camera.main);
    }

    private void OnDestroy() {
        if (currentRoot != null) {
            currentBooter.Exit();
            currentBooter = null;
        }
    }
}
