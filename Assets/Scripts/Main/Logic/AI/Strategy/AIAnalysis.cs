using System.Collections.Generic;

/// <summary>
/// Uses the known data about the current game state to derive the prioritized strategic goals for the AI team
/// </summary>
public class AIAnalysis {

    private readonly GameManager gameManager;
    private readonly Team team;
    private readonly List<HighLevelObjective> objectives;
    private readonly AIPersonality personality;

    private readonly Dictionary<HighLevelObjective, float> _priorizedObjectives = new Dictionary<HighLevelObjective, float>();
    public IReadOnlyDictionary<HighLevelObjective, float> priorizedObjectives { get { return this._priorizedObjectives; } }

    public AIAnalysis (Team team, List<HighLevelObjective> objectives, AIPersonality personality) {
        this.gameManager = GameManager.instance;
        this.team = team;
        this.objectives = objectives;
        this.personality = personality;

        foreach (HighLevelObjective objective in objectives) {
            IDestructible destructible = objective.target?.GetComponent<IDestructible>();
            if (destructible != null) {
                destructible.OnDestruction += OnObjectiveDestruction;
            }
        }
    }

    private void OnObjectiveDestruction (object sender, IDestructible e) {
        // Remove destroyed objective
        this.objectives.RemoveAt(this.objectives.FindIndex(obj => obj.target.Equals(e.gameObject)));

        this.team.ai.UpdateAI();

        // Unregister as observer
        e.OnDestruction -= OnObjectiveDestruction;
    }

    public void UpdateObjectivesPriorities () {
        this._priorizedObjectives.Clear();

        foreach (HighLevelObjective objective in this.objectives) {
            this._priorizedObjectives.Add(objective, objective.type == HighLevelObjective.Type.AttackTarget ? 1f : 0f);
        }
    }
}