using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName ="AnimatedDestruction", menuName ="Game data/Destruction sequence/Animated destruction sequence")]
public class AnimatedDestructionSequence : DestructionSequence {

    public string triggerName;

    public bool DestroyOnAnimationEnd;
    public float DestroyDelay;

    public override IEnumerator DestructionCoroutine (Destructible destroyedObject) {
        Animator animator = destroyedObject.GetComponent<Animator>();
        if (animator == null)
            throw new MissingComponentException("Missing Animator on Destructible with AnimatedDestructionSequence");

        animator.SetTrigger(triggerName);
        Destroy(destroyedObject.gameObject, DestroyDelay);

        yield break;
    }
}