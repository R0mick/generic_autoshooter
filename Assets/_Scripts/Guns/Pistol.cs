namespace _Scripts.Guns
{
    public class Pistol : AbstractGun
    {
        protected override void Start()
        {
            gunName = "Pistol";
            bulletType = "TracerBullet";
            gunDamage = 2;
            bulletSpeed = 10;
            maxAmmo = 10;
            ammo = maxAmmo;
            ammoConsumptionPerShot = 1;
            fireDelay = 0.7f;
            gunReloadTime = 2;
            gunFireRadius = 2.5f;
            bulletPiercing = 0;
            base.Start();

        }
    }
}
