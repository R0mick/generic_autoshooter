namespace _Scripts.Guns
{
    public class Revolver : AbstractGun
    {
        protected override void Start()
        {
            gunName = "Revolver";
            bulletType = "TracerBullet";
            gunDamage = 1;
            bulletSpeed = 8;
            maxAmmo = 6;
            ammo = maxAmmo;
            ammoConsumptionPerShot = 1;
            fireDelay = 1.2f;
            gunReloadTime = 2;
            gunFireRadius = 2f;
            bulletPiercing = 1;
            base.Start();

        }


    }
}
