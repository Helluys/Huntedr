using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class WinCondition : ScriptableObject {
    
    /// <summary>
    /// Callback triggered when a faction has won.
    /// Needs Setup to be called to work.
    /// </summary>
    public event EventHandler<Faction> OnFactionWon;

    /// <summary>
    /// Returns the faction that has won, or null if none is the winner yet.
    /// </summary>
    /// <returns>The winning faction, or null if there is none.</returns>
    public abstract Faction GetWinner ();

    /// <summary>
    /// Returns true if the faction has won.
    /// </summary>
    /// <param name="faction">The faction to check winning conditions</param>
    /// <returns>True if faction is the winner, false otherwise.</returns>
    public abstract bool HasLost (Faction faction);

    /// <summary>
    /// Allows the interface to register its observers so that OnWin can be triggered automatically.
    /// Should be called once at the start of the game.
    /// </summary>
    public abstract void Setup ();

    /// <summary>
    /// Retreives the list of High Level Objectives that the given Faction may fulfill.
    /// </summary>
    /// <param name="faction">The faction for which the ibjectives are requested</param>
    /// <returns></returns>
    public List<HighLevelObjective> GetObjectives (Faction faction) {
        List<HighLevelObjective> objectives = GetMapObjectives(faction);
        objectives.Add(new HighLevelObjective(HighLevelObjective.Type.Retreat, null));
        objectives.Add(new HighLevelObjective(HighLevelObjective.Type.Scout, null));

        return objectives;
    }

    /// <summary>
    /// Retreives the list of High Level Objectives that the given Faction may fulfill on the current map.
    /// Does not include generic objectives like Scout or Retreat.
    /// </summary>
    /// <param name="faction">The faction for which the ibjectives are requested</param>
    /// <returns></returns>
    protected abstract List<HighLevelObjective> GetMapObjectives (Faction faction);

    /// <summary>
    /// Callback for derived classes to notify observers of victory.
    /// </summary>
    /// <param name="faction">The faction that won. Event not dispatched if null.</param>
    protected void OnWin (Faction faction) {
        if (faction != null && OnFactionWon != null)
            OnFactionWon(this, faction);
    }
}