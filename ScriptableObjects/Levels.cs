using UnityEngine;
using UnityEditor;

[CreateAssetMenu(menuName = "Unstable Concoction/Create Levels Asset")]
public class Levels : ScriptableObject {
    public GameObject[] levelPrefabs;
}
