using System;
using UnityEngine;

public abstract class WinCondition : ScriptableObject {
    
    /// <summary>
    /// Callback triggered when a faction has won.
    /// Needs SetupObservers to be called to work.
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

    protected void OnWin (Faction faction) {
        if (faction != null && OnFactionWon != null)
            OnFactionWon(this, faction);
    }
}