namespace _Scripts.Guns
{
    public class Pistol : AbstractGun
    {
        protected override void Start()
        {
            GunName = "Pistol";
            BulletType = "TracerBullet";
            GunDamage = 2;
            BulletSpeed = 10;
            MaxAmmo = 10;
            Ammo = MaxAmmo;
            AmmoConsumptionPerShot = 1;
            FireDelay = 0.7f;
            GunReloadTime = 2;
            GunFireRadius = 2.5f;
            BulletPiercing = 0;
            base.Start();

        }
    }
}
