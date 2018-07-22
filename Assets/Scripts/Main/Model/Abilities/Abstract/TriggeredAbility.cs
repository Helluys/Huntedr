using System.Collections;

using UnityEngine;

public abstract class TriggeredAbility : Ability {

    [SerializeField] private float triggerDelay;
    [SerializeField] private float usageEnergyConsumption = 1f;

    protected abstract class TriggeredAbilityInstance : Instance {

        [SerializeField] protected FloatStatistic triggerDelay = new FloatStatistic(1f);
        protected Ship caster;
        protected TriggeredAbility model;

        private WaitForSeconds delayYield = new WaitForSeconds(0f);

        public override sealed CastType castType { get { return CastType.Triggered; } }
        public override bool isAvailable { get { return base.isAvailable && caster.status.GetEnergy() > model.usageEnergyConsumption; } }

        public TriggeredAbilityInstance (Ship casterShip, TriggeredAbility model) {
            caster = casterShip;
            this.model = model;

            this.triggerDelay = new FloatStatistic(model.triggerDelay);
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
