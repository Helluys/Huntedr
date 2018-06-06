using UnityEngine;

public abstract class ShipController : ScriptableObject {

    abstract public IInstance CreateInstance (Ship holder);

    public interface IInstance {

        void OnUpdate ();

    }
}
