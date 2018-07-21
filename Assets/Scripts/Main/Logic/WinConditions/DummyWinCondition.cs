using UnityEngine;

[CreateAssetMenu(fileName = "NoWin", menuName = "Game data/Win Conditions/No Win")]
public class DummyWinCondition : WinCondition {
    
    public override Faction GetWinner () {
        return null;
    }

    public override bool HasLost (Faction faction) {
        return false;
    }
    
    public override void Setup () {

    }
}
