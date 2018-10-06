using System.Collections;

using UnityEngine;

public class TeamAI {
    private readonly Team team;
    public AIAnalysis analysis { get; }
    public AIResourceAllocation resourceAllocation { get; }
    public AITactical tactical { get; }
    public AIPersonality personality { get; }

    private readonly WaitForSeconds waitDelay;
    private Coroutine coroutine;

    public TeamAI (Team team, AIPersonality aiPersonality) {
        this.team = team;
        this.personality = aiPersonality;

        this.analysis = new AIAnalysis(this.team, GameManager.instance.winConditions.GetObjectives(this.team.faction), this.personality);
        this.resourceAllocation = new AIResourceAllocation(this.team, this.personality);
        this.tactical = new AITactical(this.team, this.personality);

        this.waitDelay = new WaitForSeconds(this.personality.updateDelay);
    }

    public void Start () {
        this.coroutine = GameManager.instance.StartCoroutine(AILoop());
    }

    public void Stop () {
        GameManager.instance.StopCoroutine(this.coroutine);
    }

    private IEnumerator AILoop () {
        while (!GameManager.instance.winConditions.HasLost(this.team.faction)) {
            UpdateAI();

            yield return this.waitDelay;
        }
    }

    public void UpdateAI () {
        this.analysis.UpdateObjectivesPriorities();
        this.resourceAllocation.UpdateAllocatedObjectives();
        this.tactical.UpdateOrders();
    }
}
