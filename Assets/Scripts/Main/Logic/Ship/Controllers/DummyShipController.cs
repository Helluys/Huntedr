using System.Collections;
using UnityEngine;

[CreateAssetMenu(menuName = "Game data/Ships/Ship Controllers/Dummy Ship Controller", fileName = "DummyShipController")]
public class DummyShipController : ShipController {

    public override IInstance CreateInstance (Ship holder) {
        return new Instance(holder);
    }

    private class Instance : IInstance {

        private Ship ship;
        private WaitForSeconds waitOneSecond = new WaitForSeconds(1f);

        public Instance(Ship ship) {
            this.ship = ship;
        }

        public void OnStart () {
            ship.StartCoroutine(UpdateInput());
        }

        public void OnUpdate () {
            // Everything is done in coroutine
            ship.weaponSystems[0].GetComponent<WeaponSystem>()?.Shoot();
        }

        private IEnumerator UpdateInput () {
            //ship.engine.inputThrust = new Vector3().RandomRange(-1f, 1f);
            //ship.engine.inputTorque= new Vector3().RandomRange(-1f, 1f);
            yield return waitOneSecond;
        }
    }
}
