using UnityEngine;

namespace _Scripts.Guns
{
    public class Machinegun : AbstractGun
    {
        [SerializeField] private float cameraShakeDuration;
        [SerializeField] private float cameraShakeMagnitude;
        protected override void Start()
        {
            gunName = "Machinegun";
            bulletType = "MachinegunBullet";
            gunDamage = 2;
            bulletSpeed = 10;
            maxAmmo = 50;
            ammo = maxAmmo;
            ammoConsumptionPerShot = 1;
            fireDelay = 0.2f;
            gunReloadTime = 5;
            gunFireRadius = 3.5f;
            bulletPiercing = 2;
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
