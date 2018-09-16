using System.Collections.Generic;
using UnityEngine;

public class AIResourceAllocation {
    private readonly Team team;
    private readonly AIPersonality personality;

    public AIResourceAllocation (Team team, AIPersonality aIPersonality) {
        this.team = team;
        this.personality = aIPersonality;
    }

    public Dictionary<HighLevelObjective, List<Ship>> AllocateObjectives(Dictionary<HighLevelObjective, float> objectives) {

        float maxValue = Mathf.NegativeInfinity;
        HighLevelObjective prioritizedObjective = null;
        foreach(KeyValuePair<HighLevelObjective, float> kvp in objectives) {
            if (kvp.Value > maxValue) {
                maxValue = kvp.Value;
                prioritizedObjective = kvp.Key;
            }
        }

        return new Dictionary<HighLevelObjective, List<Ship>> {
            { prioritizedObjective, new List<Ship>(this.team.GetShips()) }
        };
    }

}
