using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Assigns the team ships to HighLevelObjectives
/// </summary>
public class AIResourceAllocation {
    private readonly Team team;
    private readonly AIPersonality personality;

    private readonly Dictionary<HighLevelObjective, List<Ship>> _assignedObjectives = new Dictionary<HighLevelObjective, List<Ship>>();
    public IReadOnlyDictionary<HighLevelObjective, IReadOnlyList<Ship>> assignedObjectives {
        get {
            Dictionary<HighLevelObjective, IReadOnlyList<Ship>> d = new Dictionary<HighLevelObjective, IReadOnlyList<Ship>>();
            foreach (KeyValuePair<HighLevelObjective, List<Ship>> kvp in _assignedObjectives)
                d.Add(kvp.Key, kvp.Value);
            
            return d;
        }
    }

    public AIResourceAllocation (Team team, AIPersonality aIPersonality) {
        this.team = team;
        this.personality = aIPersonality;
    }

    public void UpdateAllocatedObjectives () {
        this._assignedObjectives.Clear();

        float maxValue = Mathf.NegativeInfinity;
        HighLevelObjective prioritizedObjective = null;
        foreach (KeyValuePair<HighLevelObjective, float> kvp in this.team.ai.analysis.priorizedObjectives) {
            if (kvp.Value > maxValue) {
                maxValue = kvp.Value;
                prioritizedObjective = kvp.Key;
            }
        }

        this._assignedObjectives.Add(prioritizedObjective, new List<Ship>(this.team.GetShips()));
    }
}
