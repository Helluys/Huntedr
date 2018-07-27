using System.Collections;
using UnityEngine;

[CreateAssetMenu(menuName = "Game data/Ships/Ship Controllers/Dummy Ship Controller", fileName = "DummyShipController")]
public class DummyShipController : ShipController {

    [SerializeField] private bool shoot = false;
    [SerializeField] private bool move = false;

    public override IInstance CreateInstance (Ship holder) {
        return new Instance(holder, this);
    }

    private class Instance : IInstance {

        private Ship ship;
        private DummyShipController model;
        private WaitForSeconds waitOneSecond = new WaitForSeconds(1f);

        public Instance (Ship ship, DummyShipController model) {
            this.ship = ship;
            this.model = model;
        }

        public void OnStart () {
            ship.StartCoroutine(UpdateInput());
        }

        public void OnUpdate () {
            if (model.shoot)
                ship.weaponSystems[0].GetComponent<WeaponSystem>()?.Shoot();
        }

        private IEnumerator UpdateInput () {
            if (model.move) {
                ship.engine.inputThrust = new Vector3().RandomRange(-1f, 1f);
                ship.engine.inputTorque = new Vector3().RandomRange(-1f, 1f);
            }
            yield return waitOneSecond;
        }
    }
}
