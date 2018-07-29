using System.Collections;

using UnityEngine;

[CreateAssetMenu(menuName = "Game data/Ships/Ship Controllers/Dummy Ship Controller", fileName = "DummyShipController")]
public class DummyShipController : ShipControllerModel {

    [SerializeField] private bool shoot = false;
    [SerializeField] private bool move = false;
    
    public override Instance CreateInstance (Ship holder) {
        return new DummyShipControllerInstance(holder, this);
    }

    [System.Serializable]
    private class DummyShipControllerInstance : Instance {

        private Ship ship;
        private DummyShipController model;
        private WaitForSeconds waitOneSecond = new WaitForSeconds(1f);

        public DummyShipControllerInstance (Ship ship, DummyShipController model) {
            this.ship = ship;
            this.model = model;
        }

        public override void OnStart () {
            ship.StartCoroutine(UpdateInput());
        }

        public override void OnUpdate () {
            if (model.shoot)
                ship.weaponSystems[0].GetComponent<WeaponSystem>()?.Shoot();
        }

        private IEnumerator UpdateInput () {
            for (; ; ) {
                if (model.move) {
                    ship.engine.inputThrust = new Vector3().RandomRange(-1f, 1f);
                    ship.engine.inputTorque = new Vector3().RandomRange(-1f, 1f);
                    Debug.Log("Update inputs");
                    Debug.Log(ship.engine.inputThrust);
                    Debug.Log(ship.engine.inputTorque);
                }

                yield return waitOneSecond;
            }
        }
    }
}
