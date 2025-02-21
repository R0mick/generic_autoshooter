namespace _Scripts.Guns
{
    public class Uzi : AbstractGun
    {
        protected override void Start()
        {
            gunName = "Uzi";
            bulletType = "TracerBullet";
            gunDamage = 1;
            bulletSpeed = 12;
            maxAmmo = 24;
            ammo = maxAmmo;
            ammoConsumptionPerShot = 1;
            fireDelay = 0.1f;
            gunReloadTime = 3;
            gunFireRadius = 3f;
            bulletPiercing = 0;
            verticalSpread=7;
            base.Start();

        }
        

    }
}
