using System.Collections;

using UnityEngine;

[CreateAssetMenu(menuName = "Game data/Ships/Ship Controllers/Dummy Ship Controller", fileName = "DummyShipController")]
public class DummyShipControllerModel : ShipControllerModel {

    [SerializeField] private bool shoot = false;
    [SerializeField] private bool move = false;
    [SerializeField] private float updateDelay = 1f;
    
    public override Instance CreateInstance (Ship holder) {
        return new DummyShipControllerInstance(holder, this);
    }

    [System.Serializable]
    private class DummyShipControllerInstance : Instance {

        private Ship ship;
        private DummyShipControllerModel model;
        private WaitForSeconds waiter = new WaitForSeconds(1f);

        public DummyShipControllerInstance (Ship ship, DummyShipControllerModel model) {
            this.ship = ship;
            this.model = model;
            this.waiter = new WaitForSeconds(model.updateDelay);
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
                }

                yield return waiter;
            }
        }
    }
}
