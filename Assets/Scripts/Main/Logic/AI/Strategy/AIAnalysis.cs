using System.Collections.Generic;

/// <summary>
/// Uses the known data about the current game state to derive the prioritized strategic goals for the AI team
/// </summary>
public class AIAnalysis {

    private readonly GameManager gameManager;
    private readonly Team team;
    private readonly List<HighLevelObjective> objectives;
    private readonly AIPersonality personality;

    public AIAnalysis (Team team, List<HighLevelObjective> objectives, AIPersonality personality) {
        this.gameManager = GameManager.instance;
        this.team = team;
        this.objectives = objectives;
        this.personality = personality;
    }

    public Dictionary<HighLevelObjective, float> ComputeObjectivesPriorities () {
        Dictionary<HighLevelObjective, float> goals = new Dictionary<HighLevelObjective, float>();

        foreach (HighLevelObjective objective in objectives) {
            goals.Add(objective, objective.type == HighLevelObjective.Type.AttackTarget ? 1f : 0f);
        }

        return goals;
    }

}