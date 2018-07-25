using UnityEngine;

public abstract class Ability : ScriptableObject {

    public enum CastType {
        Triggered, Held
    }

    public abstract Instance CreateInstance (Ship caster);

    public abstract class Instance {

        [SerializeField] protected Cooldown cooldown = new Cooldown();
        public abstract string name { get; }
        public Ship caster { get; private set; }

        public abstract CastType castType { get; }
        public virtual bool isAvailable { get { return cooldown.isAvailable; } }
        public float remainingCooldown { get { return cooldown.remainingTime; } }

        protected Instance(Ship caster) {
            this.caster = caster;
        }

        public abstract void Use ();
        public abstract void Release ();

    }

}
