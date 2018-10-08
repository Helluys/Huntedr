using System.Collections;

using UnityEngine;

public class TeamAI {
    private readonly Team team;
    public AIAnalysis analysis { get; }
    public AIResourceAllocation resourceAllocation { get; }
    public AITactical tactical { get; }
    public AIPersonality personality { get; }

    private readonly WaitForSeconds waitDelay;
    private Coroutine aiCoroutine;
    private Coroutine tacticalCoroutine;

    public TeamAI (Team team, AIPersonality aiPersonality) {
        this.team = team;
        this.personality = aiPersonality;

        this.analysis = new AIAnalysis(this.team, GameManager.instance.winConditions.GetObjectives(this.team.faction), this.personality);
        this.resourceAllocation = new AIResourceAllocation(this.team, this.personality);
        this.tactical = new AITactical(this.team, this.personality);

        this.waitDelay = new WaitForSeconds(this.personality.updateDelay);
    }

    public void Start () {
        this.aiCoroutine = GameManager.instance.StartCoroutine(AILoop());
        this.tacticalCoroutine = GameManager.instance.StartCoroutine(TacticalLoop());
    }

    public void Stop () {
        GameManager.instance.StopCoroutine(this.aiCoroutine);
        GameManager.instance.StopCoroutine(this.tacticalCoroutine);
    }

    private IEnumerator AILoop () {
        while (!GameManager.instance.winConditions.HasLost(this.team.faction)) {
            UpdateAI();

            yield return this.waitDelay;
        }
    }

    private IEnumerator TacticalLoop () {
        while (!GameManager.instance.winConditions.HasLost(this.team.faction)) {
            this.tactical.UpdateOrders();

            yield return null;
        }
    }

    public void UpdateAI () {
        this.analysis.UpdateObjectivesPriorities();
        this.resourceAllocation.UpdateAllocatedObjectives();
    }
}
