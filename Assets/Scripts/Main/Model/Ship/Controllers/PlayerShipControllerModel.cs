using UnityEngine;

[CreateAssetMenu(menuName = "Game data/Ships/Ship Controllers/Player Ship Controller", fileName = "PlayerShipController")]
public class PlayerShipControllerModel : ShipControllerModel {

    #region shared state
    [SerializeField] private float mouseSensitivity;
    #endregion

    #region unshared state
    public override Instance CreateInstance (Ship holder) {
        return new PlayerShipControllerInstance(this, holder);
    }

    [System.Serializable]
    private class PlayerShipControllerInstance : Instance {

        private PlayerShipControllerModel model;
        private Ship ship;
        private WeaponSystem primaryWeapon { get { return ship.weaponSystems[0].GetComponent<WeaponSystem>(); } }
        private WeaponSystem secondaryWeapon { get { return ship.weaponSystems[1].GetComponent<WeaponSystem>(); } }

        public PlayerShipControllerInstance (PlayerShipControllerModel playerController, Ship holder) {
            Cursor.lockState = CursorLockMode.Locked;
            model = playerController;
            ship = holder;
        }

        public override void OnStart() {
            // Nothing to initialize
        }

        public override void OnUpdate () {
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

            if (Input.GetButtonDown("Ability1"))
                ship.abilities.UseAbility(0);
            if (Input.GetButtonUp("Ability1"))
                ship.abilities.ReleaseAbility(0);
        }
    }
    #endregion unshared state
}
