using UnityEngine;

[CreateAssetMenu(menuName = "Game data/Ships/Ship Controllers/Dummy Ship Controller", fileName = "DummyShipController")]
public class DummyShipController : ShipController {

    public override IInstance CreateInstance (Ship holder) {
        return new Instance();
    }

    private class Instance : IInstance {
        public void OnUpdate () {}
    }
}
