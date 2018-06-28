using UnityEngine;

public class MachineGun : WeaponSystem {

    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform shootBulletPoint;

    [SerializeField] private FloatStatistic shootRate = new FloatStatistic(5);
    [SerializeField] private FloatStatistic bulletVelocity = new FloatStatistic(150);

    private Ship ship;
    private float allowedShootTime;

    public override void Initialize (Ship holder) {
        ship = holder;
    }

    public override void Shoot () {
        if (!CanShoot()) return;

        var bullet = Instantiate(bulletPrefab, shootBulletPoint.position, shootBulletPoint.rotation * Quaternion.Euler(90f, 0f, 0f));
        bullet.GetComponent<Rigidbody>().velocity = shootBulletPoint.transform.forward * bulletVelocity;
        allowedShootTime = Time.time + 1f / shootRate;
    }

    private bool CanShoot () {
        return Time.time > allowedShootTime && ship.status.TryUseAmmunition(1);
    }

    protected override void ApplyModifier (FloatStatistic.Modifier modifier) {
        this.shootRate.AddModifier(modifier);
        this.bulletVelocity.AddModifier(modifier);
    }

    protected override void RemoveModifier (FloatStatistic.Modifier modifier) {
        this.shootRate.RemoveModifier(modifier);
        this.bulletVelocity.RemoveModifier(modifier);
    }
}
