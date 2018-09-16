using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DestroyTargets", menuName = "Game data/Win Conditions/Destroy Targets")]
public class DestroyTargetsWinCondition : WinCondition {

    [Serializable]
    private class FactionDestructibles {
        public int factionIndex;
        public List<Destructible> destructibles;

        public Faction faction {
            get {
                return GameManager.instance.gameConfiguration.teams[this.factionIndex].faction;
            }
        }
    }

    [SerializeField] private List<FactionDestructibles> factionDestructiblesList;

    public override Faction GetWinner () {
        List<Faction> aliveFactions = new List<Faction>();

        // Loop through factions
        foreach (FactionDestructibles factionDestructibles in this.factionDestructiblesList) {
            // A faction has lost if all its destructibles are destroyed
            if (!HasLost(factionDestructibles))
                aliveFactions.Add(factionDestructibles.faction);
        }

        // A faction has won if it is the last one standing
        return aliveFactions.Count == 1 ? aliveFactions[0] : null;
    }

    public override bool HasLost (Faction faction) {
        // Find FactionDestructible 
        FactionDestructibles factionDestructibles = this.factionDestructiblesList.Find(e => { return e.faction.Equals(faction); });
        if (factionDestructibles == null)
            throw new ArgumentOutOfRangeException("Faction " + faction + " is not part of the win conditions");

        return HasLost(factionDestructibles);
    }

    private bool HasLost (FactionDestructibles factionDestructibles) {
        return factionDestructibles.destructibles.TrueForAll(destructible => { return destructible.isDestroyed; });
    }

    public override void Setup () {
        foreach (FactionDestructibles factionDestructibles in this.factionDestructiblesList) {
            foreach (IDestructible destructible in factionDestructibles.destructibles) {
                destructible.OnDestruction += CheckWinCondition;

                Component component = destructible as Component;
                if (component != null)
                    GameObjectUtils.SetColorRecursive(component.transform, factionDestructibles.faction.primaryColor, factionDestructibles.faction.secondaryColor);
            }
        }
    }

    protected override List<HighLevelObjective> GetMapObjectives (Faction faction) {
        List<HighLevelObjective> objectives = new List<HighLevelObjective>();

        foreach (FactionDestructibles factionDestructibles in this.factionDestructiblesList) {
            HighLevelObjective.Type currentTargetType =
                Faction.AreFriendly(faction, factionDestructibles.faction) ?
                    HighLevelObjective.Type.DefendTarget : HighLevelObjective.Type.AttackTarget;

            foreach (IDestructible destructible in factionDestructibles.destructibles) {
                objectives.Add(new HighLevelObjective(currentTargetType, destructible.gameObject));
            }
        }

        return objectives;
    }

    private void CheckWinCondition (object sender, IDestructible destroyedTarget) {
        OnWin(GetWinner());
    }
}
