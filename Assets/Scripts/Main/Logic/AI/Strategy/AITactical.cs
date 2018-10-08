using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Assigns LowLevelObjectives to ships according to the HighLevelObjectives assignments.
/// </summary>
public class AITactical {
    private readonly Team team;
    private readonly AIPersonality personality;

    public AITactical (Team team, AIPersonality personality) {
        this.team = team;
        this.personality = personality;
    }

    public void UpdateOrders () {
        foreach (KeyValuePair<HighLevelObjective, IReadOnlyList<Ship>> objectiveShips in this.team.ai.resourceAllocation.assignedObjectives) {
            foreach (Ship ship in objectiveShips.Value) {
                UpdateOrder(objectiveShips, ship);
            }
        }
    }

    public void UpdateOrder (Ship ship) {
        KeyValuePair<HighLevelObjective, IReadOnlyList<Ship>> objectiveShips;
        foreach (KeyValuePair<HighLevelObjective, IReadOnlyList<Ship>> kvp in this.team.ai.resourceAllocation.assignedObjectives) {
            if (kvp.Value.Contains(ship)) {
                objectiveShips = kvp;
                break;
            }
        }

        UpdateOrder(objectiveShips, ship);
    }

    private void UpdateOrder (KeyValuePair<HighLevelObjective, IReadOnlyList<Ship>> objectiveShips, Ship ship) {
        // Give orders only to AI ships
        if (ship.controller.instance is AIControllerModel.AIControllerInstance) {
            AIControllerModel.AIControllerInstance aiControllerInstance = ship.controller.instance as AIControllerModel.AIControllerInstance;
            aiControllerInstance.SetObjective(DeriveLowLevelObjective(ship, objectiveShips.Key, objectiveShips.Value));
        }
    }

    private LowLevelObjective DeriveLowLevelObjective (Ship ship, HighLevelObjective highLevelObjective, IReadOnlyList<Ship> shipsWithSameObjective) {
        LowLevelObjective objective;

        switch (highLevelObjective.type) {
            case HighLevelObjective.Type.DefendTarget:
                objective = DeriveDefendTargetObjective(ship, highLevelObjective);
                break;
            case HighLevelObjective.Type.AttackTarget:
                objective = DeriveAttackTargetObjective(ship, highLevelObjective);
                break;
            case HighLevelObjective.Type.Scout:
                objective = DeriveScoutObjective();
                break;
            case HighLevelObjective.Type.Retreat:
                objective = DeriveRetreatObjective(ship);
                break;
            default:
                throw new ArgumentException("Unknown HighLevelObjective Type : " + highLevelObjective.type);
        }

        return objective;
    }

    private LowLevelObjective DeriveScoutObjective () {
        LowLevelObjective objective;
        Transform target = GameManager.GetPathNodesHolder().transform.GetChild(Mathf.FloorToInt(UnityEngine.Random.value * GameManager.GetPathNodesHolder().transform.childCount));
        objective = LowLevelObjective.MoveToPoint(target.position, 1f);
        return objective;
    }

    private LowLevelObjective DeriveAttackTargetObjective (Ship ship, HighLevelObjective highLevelObjective) {
        LowLevelObjective objective;
        List<Ship> closeEnemyShips = GetEnemyShips(ship, highLevelObjective.target.transform.position);
        if (closeEnemyShips.Count > 0) {
            GameObject target = GameObjectUtils.GetClosest(ship.gameObject, closeEnemyShips.ConvertAll(s => s.gameObject));
            objective = LowLevelObjective.Destroy(target.GetComponent<Ship>());
        } else {
            if (GameObjectUtils.CanSee(ship, highLevelObjective.target)) {
                objective = LowLevelObjective.Destroy(highLevelObjective.target.GetComponent<IDestructible>());
            } else {
                objective = LowLevelObjective.GetInSight(highLevelObjective.target.transform);
            }
        }

        return objective;
    }

    private LowLevelObjective DeriveDefendTargetObjective (Ship ship, HighLevelObjective highLevelObjective) {
        LowLevelObjective objective;
        if (GameObjectUtils.CanSee(ship, highLevelObjective.target)) {
            List<Ship> closeEnemyShips = GetEnemyShips(ship, highLevelObjective.target.transform.position);
            if (closeEnemyShips.Count > 0) {
                GameObject target = GameObjectUtils.GetClosest(ship.gameObject, closeEnemyShips.ConvertAll(s => s.gameObject));
                objective = LowLevelObjective.Destroy(target.GetComponent<Ship>());
            } else
                objective = LowLevelObjective.GetInSight(highLevelObjective.target.transform);
        } else {
            objective = LowLevelObjective.GetInSight(highLevelObjective.target.transform);
        }

        return objective;
    }

    private LowLevelObjective DeriveRetreatObjective (Ship ship) {
        return LowLevelObjective.MoveToPoint(GameManager.GetSpawningZone(ship.team.faction).transform.position, 1f);
    }

    private List<Ship> GetEnemyShips (Ship ship, Vector3 targetPoint) {
        return GameManager.instance.FilterShips(s =>
                !Faction.AreFriendly(s.team.faction, ship.team.faction)
                && (s.transform.position - targetPoint).magnitude < this.personality.aggressionRange
                && GameObjectUtils.CanSee(ship, s.gameObject));
    }
}
