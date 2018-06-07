using UnityEngine;

[CreateAssetMenu(menuName = "Game data/Ships/Ship Weapons/Machine Gun", fileName = "MachineGun")]
public class MachineGun : WeaponSystem {

    #region shared state
    [SerializeField] private GameObject bulletPrefab;

    [SerializeField] private float shootRate = 1f;
    [SerializeField] private float bulletVelocity = 500f;
    #endregion

    public override IInstance CreateInstance (Transform weaponTransform, Ship holder) {
        return new Instance(this, holder, weaponTransform);
    }

    private class Instance : IInstance {

        #region not shared state
        private MachineGun model;
        private float allowedShootTime;
        private Ship ship;
        private Transform shootBulletPoint;
        #endregion

        public Instance (MachineGun machineGun, Ship holder, Transform weaponTransform) {
            model = machineGun;
            ship = holder;
            shootBulletPoint = weaponTransform;
        }

        public void Shoot () {
            if (CanShoot()) {
                GameObject bullet = Instantiate(model.bulletPrefab, shootBulletPoint.position, shootBulletPoint.rotation * Quaternion.Euler(90f, 0f, 0f));
                bullet.GetComponent<Rigidbody>().velocity = shootBulletPoint.transform.forward * model.bulletVelocity;
                allowedShootTime = Time.time + 1f / model.shootRate;
            }
        }

        private bool CanShoot () {
            return Time.time > allowedShootTime && ship.status.TryUseAmmunition(1);
        }
    }
}
