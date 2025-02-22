namespace _Scripts.Guns
{
    public class Revolver : AbstractGun
    {
        protected override void Start()
        {
            GunName = "Revolver";
            BulletType = "TracerBullet";
            GunDamage = 1;
            BulletSpeed = 8;
            MaxAmmo = 6;
            Ammo = MaxAmmo;
            AmmoConsumptionPerShot = 1;
            FireDelay = 1.2f;
            GunReloadTime = 2;
            GunFireRadius = 2f;
            BulletPiercing = 1;
            base.Start();

        }


    }
}
