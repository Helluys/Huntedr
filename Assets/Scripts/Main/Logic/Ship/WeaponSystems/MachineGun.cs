using System.Collections.Generic;
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

        #region unshared state
        private MachineGun model;
        private Ship ship;
        private Transform shootBulletPoint;

        private GameObject bulletPrefab;
        private FloatStatistic shootRate;
        private FloatStatistic bulletVelocity;

        private float allowedShootTime;
        private Dictionary<WeaponBuffEffect, FloatStatistic.Modifier> effectModifiers = new Dictionary<WeaponBuffEffect, FloatStatistic.Modifier>();
        #endregion

        public Instance (MachineGun machineGun, Ship holder, Transform weaponTransform) {
            model = machineGun;
            ship = holder;
            shootBulletPoint = weaponTransform;

            bulletPrefab = model.bulletPrefab;
            shootRate = new FloatStatistic(model.shootRate);
            bulletVelocity = new FloatStatistic(model.bulletVelocity);
        }

        public void Shoot () {
            if (!CanShoot()) return;

            var bullet = Instantiate(bulletPrefab, shootBulletPoint.position, shootBulletPoint.rotation * Quaternion.Euler(90f, 0f, 0f));
            bullet.GetComponent<Rigidbody>().velocity = shootBulletPoint.transform.forward * bulletVelocity;
            allowedShootTime = Time.time + 1f / shootRate;
        }

        private bool CanShoot () {
            return Time.time > allowedShootTime && ship.status.TryUseAmmunition(1);
        }

        public void Buff (WeaponBuffEffect effect) {
            Debug.Log("Buff");
            FloatStatistic.Modifier modifier = new FloatStatistic.Modifier(FloatStatistic.Modifier.Type.Factor, effect.buffRatio);
            effectModifiers.Add(effect, modifier);

            this.shootRate.AddModifier(modifier);
            this.bulletVelocity.AddModifier(modifier);
        }

        public void Debuff (WeaponBuffEffect effect) {
            Debug.Log("Debuff");
            if (!effectModifiers.ContainsKey(effect))
                throw new KeyNotFoundException("Inexistant weapon effect in Debuff");

            this.shootRate.RemoveModifier(effectModifiers[effect]);
            this.bulletVelocity.RemoveModifier(effectModifiers[effect]);

            effectModifiers.Remove(effect);
        }
    }
}
