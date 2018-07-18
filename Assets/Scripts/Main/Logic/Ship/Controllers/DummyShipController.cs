using UnityEngine;

[CreateAssetMenu(menuName = "Game data/Ships/Ship Controllers/Dummy Ship Controller", fileName = "DummyShipController")]
public class DummyShipController : ShipController {

    public override IInstance CreateInstance (Ship holder) {
        return new Instance(holder);
    }

    private class Instance : IInstance {

        private Ship ship;

        public Instance(Ship ship) {
            this.ship = ship;
        }

        public void OnUpdate () {
            ship.engine.inputThrust = Vector3.forward;
        }
    }
}
