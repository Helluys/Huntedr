using UnityEngine;

[CreateAssetMenu(menuName = "Game data/Faction", fileName = "Faction")]
public class Faction : ScriptableObject {

    new public string name;

    public Color primaryColor;

    public Color secondaryColor;

    public Texture2D logo;

    public int index {
        get {
            return GameManager.instance.gameConfiguration.teams.FindIndex(team => team.faction.Equals(this));
        }
    }

    public static Faction FromIndex(int index) {
        return GameManager.instance.gameConfiguration.teams[index].faction;
    }

}