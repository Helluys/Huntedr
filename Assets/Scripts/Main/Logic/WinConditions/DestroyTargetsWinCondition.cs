using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="DestroyTargets", menuName ="Game data/Win Conditions/Destroy Targets")]
public class DestroyTargetsWinCondition : WinCondition {

    [Serializable]
    private class FactionDestructibles {
        public Faction faction;
        public List<Destructible> destructibles;
    }

    [SerializeField] private List<FactionDestructibles> factionDestructiblesList;

    public override Faction GetWinner () {
        List<Faction> aliveFactions = new List<Faction>();
    
        // Loop through factions
        foreach (FactionDestructibles factionDestructibles in factionDestructiblesList) {
            // A faction has lost if all its destructibles are destroyed
            if (!HasLost(factionDestructibles))
                aliveFactions.Add(factionDestructibles.faction);
        }

        // A faction has won if it is the last one standing
        return aliveFactions.Count == 1 ? aliveFactions[0] : null;
    }

    public override bool HasLost (Faction faction) {
        // Find FactionDestructible 
        FactionDestructibles factionDestructibles = factionDestructiblesList.Find(e => { return e.faction.Equals(faction); });
        if (factionDestructibles == null)
            throw new ArgumentOutOfRangeException("Faction " + faction + " is not part of the win conditions");

        return HasLost(factionDestructibles);
    }

    private bool HasLost (FactionDestructibles factionDestructibles) {
        return factionDestructibles.destructibles.TrueForAll(destructible => { return destructible.isDestroyed; });
    }

    public override void Setup () {
        foreach (FactionDestructibles factionDestructibles in factionDestructiblesList) {
            foreach(IDestructible destructible in factionDestructibles.destructibles) {
                destructible.OnDestruction += CheckWinCondition;

                Component component = destructible as Component;
                if(component != null)
                    GameObjectUtils.SetColorRecursive(component.transform, factionDestructibles.faction.primaryColor, factionDestructibles.faction.secondaryColor);
            }
        }
    }

    private void CheckWinCondition (object sender, IDestructible destroyedTarget) {
        OnWin(GetWinner());
    }
}
