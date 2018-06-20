using UnityEngine;

public abstract class WeaponSystem : ScriptableObject {

    public abstract IInstance CreateInstance (Transform weaponTransform, Ship holder);
    
    public interface IInstance {

        void Shoot ();

        void Buff (WeaponBuffEffect effect);

        void Debuff (WeaponBuffEffect effect);

    }
    
}
