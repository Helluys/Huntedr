using UnityEngine;

public abstract class WeaponSystem : ScriptableObject {

    public abstract IInstance CreateInstance (Transform weaponTransform, Ship holder);
    
    public interface IInstance {

        void Shoot ();

    }
    
}
