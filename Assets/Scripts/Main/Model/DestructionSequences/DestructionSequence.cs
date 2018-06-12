using System.Collections;
using UnityEngine;

public abstract class DestructionSequence : ScriptableObject {

    public abstract IEnumerator DestructionCoroutine (Destructible destroyedObject);

}
