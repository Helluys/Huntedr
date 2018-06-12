using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class VictoryPanel : MonoBehaviour {
    
    [SerializeField] private Text victoryText;

    void Start () {
        GameManager.instance.winConditions.OnFactionWon += DisplayPanel;
        gameObject.SetActive(false);
    }

    private void DisplayPanel (object sender, Faction winner) {
        victoryText.text = winner.name + " won!";
        victoryText.color = winner.primaryColor;
        GetComponent<Image>().color = winner.secondaryColor;

        gameObject.SetActive(true);
    }
}
