using System;
using System.Collections.Generic;
using UnityEngine;

public class AITactical {
    private readonly AIPersonality personality;

    public AITactical (AIPersonality personality) {
        this.personality = personality;
    }

    public void GiveOrders (Dictionary<HighLevelObjective, List<Ship>> allocatedObjectives) {
        foreach (KeyValuePair<HighLevelObjective, List<Ship>> objectiveShips in allocatedObjectives) {
            foreach (Ship ship in objectiveShips.Value) {
                // Give order only to AI ships
                if (ship.controller.instance is AIControllerModel.AIControllerInstance) {
                    AIControllerModel.AIControllerInstance aiControllerInstance = ship.controller.instance as AIControllerModel.AIControllerInstance;
                    aiControllerInstance.SetObjective(DeriveLowLevelObjective(ship, objectiveShips.Key, objectiveShips.Value));
                }
            }
        }
    }

    private LowLevelObjective DeriveLowLevelObjective (Ship ship, HighLevelObjective highLevelObjective, List<Ship> shipsWithSameObjective) {
        LowLevelObjective objective;

        switch (highLevelObjective.type) {
            case HighLevelObjective.Type.DefendTarget:
                if (CanSee(ship, highLevelObjective.target)) {
                    List<Ship> closeEnemyShips = GameManager.instance.FilterShips(
                        s => !Faction.AreFriendly(s.faction, ship.faction)
                        && (s.transform.position - highLevelObjective.target.transform.position).magnitude < this.personality.aggressionRange);

                    if (closeEnemyShips.Count > 0) {
                        GameObject target = GameObjectUtils.GetClosest(ship.gameObject, closeEnemyShips.ConvertAll(s => s.gameObject));
                        objective = LowLevelObjective.TargetShip(target.GetComponent<Ship>());
                    } else
                        objective = LowLevelObjective.MoveToPoint(highLevelObjective.target.transform.position);
                } else {
                    objective = LowLevelObjective.MoveToPoint(highLevelObjective.target.transform.position);
                }
                break;
            case HighLevelObjective.Type.AttackTarget:
                if (CanSee(ship, highLevelObjective.target)) {
                    objective = LowLevelObjective.TargetObject(highLevelObjective.target);
                } else {
                    objective = LowLevelObjective.MoveToPoint(highLevelObjective.target.transform.position);
                }
                break;
            case HighLevelObjective.Type.Scout: {
                Transform target = GameManager.GetPathNodesHolder().transform.GetChild(Mathf.FloorToInt(UnityEngine.Random.value * GameManager.GetPathNodesHolder().transform.childCount));
                objective = LowLevelObjective.MoveToPoint(target.position);
            }
            break;
            case HighLevelObjective.Type.Retreat:
                objective = LowLevelObjective.MoveToPoint(GameManager.GetSpawningZone(ship.faction).transform.position);
                break;
            default:
                throw new ArgumentException("Unknown HighLevelObjective Type : " + highLevelObjective.type);
        }

        return objective;
    }

    private static bool CanSee (Ship ship, GameObject target) {
        return !Physics.Raycast(ship.transform.position, (target.transform.position - ship.transform.position).normalized, Mathf.Infinity);
    }
}
