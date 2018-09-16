using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeamAI {
    private readonly Team team;
    private readonly AIAnalysis analysis;
    private readonly AIResourceAllocation resourceAllocation;
    private readonly AITactical tactical;
    private readonly AIPersonality personality;

    private readonly WaitForSeconds waitDelay;

    public TeamAI (Team team, AIPersonality aiPersonality) {
        this.team = team;
        this.personality = aiPersonality;

        this.analysis = new AIAnalysis(this.team, GameManager.instance.winConditions.GetObjectives(this.team.faction), this.personality);
        this.resourceAllocation = new AIResourceAllocation(this.team, this.personality);
        this.tactical = new AITactical(this.personality);

        this.waitDelay = new WaitForSeconds(this.personality.updateDelay);

        GameManager.instance.StartCoroutine(UpdateAI());
    }

    private IEnumerator UpdateAI () {
        while (!GameManager.instance.winConditions.HasLost(this.team.faction)) {
            Dictionary<HighLevelObjective, float> priorizedObjectives = this.analysis.ComputeObjectivesPriorities();
            Dictionary<HighLevelObjective, List<Ship>> assignedObjectives = this.resourceAllocation.AllocateObjectives(priorizedObjectives);
            this.tactical.GiveOrders(assignedObjectives);

            yield return this.waitDelay;
        }
    }
}
