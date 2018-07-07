using UnityEngine;

public abstract class ShipController : ScriptableObject {

    new public string name;

    abstract public IInstance CreateInstance (Ship holder);

    public interface IInstance {
        void OnUpdate ();
    }
}
