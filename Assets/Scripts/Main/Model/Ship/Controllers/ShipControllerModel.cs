using UnityEngine;

public abstract class ShipControllerModel : ScriptableObject {

    new public string name;

    abstract public Instance CreateInstance (Ship holder);

    public abstract class Instance {
        public abstract void OnStart ();
        public abstract void OnUpdate ();
    }
}
