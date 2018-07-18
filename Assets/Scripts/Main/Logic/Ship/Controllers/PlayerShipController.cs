using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Game data/Ships/Ship Controllers/Player Ship Controller", fileName = "PlayerShipController")]
public class PlayerShipController : ShipController {

    #region shared state
    [SerializeField] private float mouseSensitivity;
    #endregion

    #region unshared state
    public override IInstance CreateInstance (Ship holder) {
        return new Instance(this, holder);
    }

    [Serializable]
    private class Instance : IInstance {

        private PlayerShipController model;
        private Ship ship;
        private WeaponSystem primaryWeapon { get { return ship.weaponSystems[0].GetComponent<WeaponSystem>(); } }
        private WeaponSystem secondaryWeapon { get { return ship.weaponSystems[1].GetComponent<WeaponSystem>(); } }

        public Instance (PlayerShipController playerController, Ship holder) {
            Cursor.lockState = CursorLockMode.Locked;
            model = playerController;
            ship = holder;
        }

        public void OnUpdate () {
            ship.engine.inputThrust =
                Input.GetAxis("HorizontalThrust") * Vector3.right +
                Input.GetAxis("VerticalThrust") * Vector3.up +
                Input.GetAxis("Thrust") * Vector3.forward;

            Vector3 cursorPosition = (-Input.GetAxis("Pitch") * Vector3.right + Input.GetAxis("Yaw") * Vector3.up) * model.mouseSensitivity;
            ship.engine.inputTorque = cursorPosition + Input.GetAxis("Roll") * Vector3.forward;

            ship.engine.inputCushion = Input.GetAxis("Cushion");
            ship.engine.inputStabilize = Input.GetAxis("Stabilize");

            if (primaryWeapon != null && Input.GetButton("ShootPrimary"))
                primaryWeapon.Shoot();

            if (secondaryWeapon != null && Input.GetButton("ShootSecondary"))
                secondaryWeapon.Shoot();
        }
    }
    #endregion unshared state
}
