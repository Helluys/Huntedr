using UnityEngine;

[CreateAssetMenu(menuName ="Game data/Faction", fileName ="Faction")]
public class Faction : ScriptableObject {

    new public string name;

    public Color primaryColor;

    public Color secondaryColor;

    public Texture2D logo;

}