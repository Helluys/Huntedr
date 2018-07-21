using UnityEngine;

[CreateAssetMenu(fileName = "ShieldAbility", menuName = "Game data/Abilities/Shield")]
public class ShieldAbility : HeldAbility {

    [SerializeField] GameObject shieldPrefab;
    [SerializeField] Vector3 shieldSpawnOffset;
    [SerializeField] Vector3 shieldSpawnOrientation;
    [SerializeField] float creationEnergyConsumption = 5f;
    [SerializeField] float holdEnergyConsumption = 1f;

    public override Instance CreateInstance (Ship caster) {
        return new ShieldAbilityInstance(caster, this);
    }

    private class ShieldAbilityInstance : HeldAbilityInstance {

        private ShieldAbility model;
        private GameObject shieldInstance;

        public override bool isAvailable {
            get { return base.isAvailable && caster.status.GetEnergy() > model.creationEnergyConsumption; }
        }

        public override string name { get { return "Shield"; } }

        public ShieldAbilityInstance (Ship casterShip, ShieldAbility model) : base(casterShip) {
            caster.OnDestruction += Caster_OnDestruction;
            this.model = model;
        }

        protected override void OnHoldStart () {
            if (caster.status.TryUseEnergy(model.creationEnergyConsumption)) {
                shieldInstance = Instantiate(
                    model.shieldPrefab,
                    caster.transform.TransformPoint(model.shieldSpawnOffset),
                    Quaternion.Euler(model.shieldSpawnOrientation) * caster.transform.rotation);

                shieldInstance.GetComponent<Shield>().SetTrackedTransform(caster.transform, true);
            } else
                Release();
        }

        protected override void OnHoldStay () {
            if (!caster.status.TryUseEnergy(model.holdEnergyConsumption * Time.deltaTime))
                Release();
        }

        protected override void OnHoldStop () {
            if (shieldInstance != null)
                Destroy(shieldInstance);
            shieldInstance = null;
        }

        private void Caster_OnDestruction (object sender, IDestructible e) {
            Release();
        }
    }
}