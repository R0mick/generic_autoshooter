using UnityEngine;

namespace _Scripts.Guns
{
    public class Machinegun : AbstractGun
    {
        [SerializeField] private float cameraShakeDuration;
        [SerializeField] private float cameraShakeMagnitude;
        protected override void Start()
        {
            GunName = "Machinegun";
            BulletType = "MachinegunBullet";
            GunDamage = 2;
            BulletSpeed = 10;
            MaxAmmo = 50;
            Ammo = MaxAmmo;
            AmmoConsumptionPerShot = 1;
            FireDelay = 0.2f;
            GunReloadTime = 5;
            GunFireRadius = 3.5f;
            BulletPiercing = 2;
            verticalSpread=3;
            cameraShakeDuration = 0.02f;
            cameraShakeMagnitude = 0.07f;
            
            base.Start();

        }
        
        
        protected override void FireBullet(Vector3 enemyPosition)
        {
            base.FireBullet(enemyPosition);
            CameraBehaviour.Instance.cameraShakeOnShoot(cameraShakeDuration, cameraShakeMagnitude);
            
        }
    
    }
}
