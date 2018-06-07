using UnityEngine;

[CreateAssetMenu(menuName ="Game data/Faction", fileName ="Faction")]
public class Faction : ScriptableObject {

    new public string name;

    public Color color;

    public Texture2D logo;

}