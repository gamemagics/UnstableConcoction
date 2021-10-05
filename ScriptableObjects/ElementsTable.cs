using System;
using UnityEngine;

public enum ElementType {
    Arcane,
    Blight,
    Crystal,
    Diffusal,
    Essence,
    Force
}

[Serializable]
public struct Element {
    public ElementType type;
    public Sprite collectableSprite;

    public Sprite tileSprite;

    public Color color;
}

[Serializable]
public struct Rule {
    public ElementType e1;
    public ElementType e2;
    public ElementType res;
}

[CreateAssetMenu(menuName = "Unstable Concoction/Create Elements Table Asset")]
public class ElementsTable : ScriptableObject {
    public Element[] elements;
    public Rule[] rules; 
}
