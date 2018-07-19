using UnityEngine;

public class MachineGun : WeaponSystem {
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform shootBulletPoint;

    [SerializeField] private FloatStatistic shootRate = new FloatStatistic(5);
    [SerializeField] private FloatStatistic bulletVelocity = new FloatStatistic(150);
    [SerializeField] private uint ammunitionPerUse = 1;
    [SerializeField] private float energyPerUse = 0.1f;

    private Ship ship;
    private float allowedShootTime;

    public override void Initialize (Ship holder) {
        ship = holder;
    }

    public override void Shoot () {
        if (!CanShoot()) return;

        ship.status.TryUseAmmunition(ammunitionPerUse);
        ship.status.TryUseEnergy(energyPerUse);

        var bullet = Instantiate(bulletPrefab, shootBulletPoint.position, shootBulletPoint.rotation * Quaternion.Euler(90f, 0f, 0f));
        bullet.GetComponent<Rigidbody>().velocity = ship.GetComponent<Rigidbody>().GetPointVelocity(shootBulletPoint.position)
                                                    + shootBulletPoint.transform.forward * bulletVelocity;
        allowedShootTime = Time.time + 1f / shootRate;
    }

    private bool CanShoot () {
        return Time.time > allowedShootTime 
            && ship.status.GetAmmunition() > ammunitionPerUse 
            && ship.status.GetEnergy() > energyPerUse;
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
