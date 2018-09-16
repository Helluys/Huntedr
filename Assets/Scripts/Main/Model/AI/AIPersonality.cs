using UnityEngine;

/// <summary>
/// Set of parameters that affects all AI decisions
/// </summary>
[CreateAssetMenu(fileName = "AI Personality", menuName = "Game Data/AI/Personality")]
public class AIPersonality : ScriptableObject {
    public float updateDelay;
    public float aggressionRange;
}