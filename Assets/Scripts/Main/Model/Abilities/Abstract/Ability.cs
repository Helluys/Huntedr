using UnityEngine;

public abstract class Ability : ScriptableObject {

    public enum CastType {
        Triggered, Held
    }

    public abstract Instance CreateInstance (Ship caster);

    public abstract class Instance {

        [SerializeField] protected Cooldown cooldown = new Cooldown();
        public abstract string name { get; }

        public abstract CastType castType { get; }
        public virtual bool isAvailable { get { return cooldown.isAvailable; } }
        public float remainingCooldown { get { return cooldown.remainingTime; } }

        public abstract void Use ();
        public abstract void Release ();

    }

}