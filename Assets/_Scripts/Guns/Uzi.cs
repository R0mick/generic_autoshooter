namespace _Scripts.Guns
{
    public class Uzi : AbstractGun
    {
        protected override void Start()
        {
            GunName = "Uzi";
            BulletType = "TracerBullet";
            GunDamage = 1;
            BulletSpeed = 12;
            MaxAmmo = 24;
            Ammo = MaxAmmo;
            AmmoConsumptionPerShot = 1;
            FireDelay = 0.1f;
            GunReloadTime = 3;
            GunFireRadius = 3f;
            BulletPiercing = 0;
            verticalSpread=7;
            base.Start();

        }
        

    }
}
