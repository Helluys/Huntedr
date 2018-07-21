using System.Collections;

using UnityEngine;

public abstract class TriggeredAbility : Ability {

    [SerializeField] private float triggerDelay;

    protected abstract class TriggeredAbilityInstance : Instance {

        [SerializeField] private FloatStatistic triggerDelay = new FloatStatistic(1f);
        private WaitForSeconds delayYield = new WaitForSeconds(0f);
        private Ship caster;

        public override sealed CastType castType { get { return CastType.Triggered; } }

        public TriggeredAbilityInstance (Ship casterShip, float delay) {
            caster = casterShip;
            this.triggerDelay = new FloatStatistic(delay);
            delayYield = new WaitForSeconds(triggerDelay.value);
            triggerDelay.OnValueChanged += UpdateDelayYield;
        }

        public sealed override void Use () {
            if (isAvailable)
                caster.StartCoroutine(DelayedTrigger());
        }

        public override void Release () {
            // Nothing to do
        }

        private IEnumerator DelayedTrigger () {
            yield return delayYield;
            Trigger();
        }

        protected abstract void Trigger ();

        private void UpdateDelayYield (object sender, float e) {
            delayYield = new WaitForSeconds(e);
        }
    }
}
