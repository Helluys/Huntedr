using UnityEngine;

[CreateAssetMenu(fileName = "RepairDrone", menuName = "Game data/Abilities/Repair Drone")]
public class RepairDroneAbility : TriggeredAbility {

    [SerializeField] private SingleTargetPicker targetPicker;

    [SerializeField] private GameObject dronePrefab;
    [SerializeField] private Vector3 spawnOffset;

    public override Instance CreateInstance (Ship caster) {
        return new RepairDroneAbilityInstance(caster, this);
    }

    private class RepairDroneAbilityInstance : TriggeredAbilityInstance {
        public override string name { get { return "Repair Drone"; } }

        new private RepairDroneAbility model;
        private SingleTargetPicker singleTargetPicker;

        private GameObject droneInstance;

        public RepairDroneAbilityInstance (Ship caster, RepairDroneAbility ability) : base(caster, ability) {
            singleTargetPicker = Instantiate(ability.targetPicker);
            model = ability;
        }

        protected override void Trigger () {
            singleTargetPicker.OnTargetPicked += SingleTargetPicker_OnTargetPicked;
            singleTargetPicker.StartPicking(caster);
        }

        private void SingleTargetPicker_OnTargetPicked (object sender, Ship target) {
            if (droneInstance == null)
                droneInstance = Instantiate(model.dronePrefab, caster.transform.position + model.spawnOffset, caster.transform.rotation);
            droneInstance.GetComponent<RepairDrone>().target = target;
        }
    }
}
