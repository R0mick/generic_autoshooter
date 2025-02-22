using System.Collections;
using _Scripts.Bullets;
using UnityEngine;

namespace _Scripts.Guns
{
    public class Shotgun : AbstractGun
    {

        [SerializeField] private int bulletsPerShot;
        [SerializeField] private float horizontalSpread;
        [SerializeField] private float cameraShakeDuration;
        [SerializeField] private float cameraShakeMagnitude;

        protected override void Start()
        {
            GunName = "Shotgun";
            BulletType = "ShotgunBullet";
            GunDamage = 1;
            BulletSpeed = 5;
            MaxAmmo = 2;
            Ammo = MaxAmmo;
            AmmoConsumptionPerShot = 1;
            FireDelay = 2f;
            GunReloadTime = 2f;
            GunFireRadius = 2.5f;
            BulletPiercing = 1;
            verticalSpread = 15;
            bulletsPerShot = 8;
            horizontalSpread = 0.001f;
            base.Start();

            cameraShakeDuration = 0.01f;
            cameraShakeMagnitude = 0.03f;
        }



        protected override void FireBullet(Vector3 enemyPosition)
        {
            StartCoroutine(ShotgunShot(enemyPosition));
            CameraBehaviour.Instance.cameraShakeOnShoot(cameraShakeDuration, cameraShakeMagnitude);
        }


        private IEnumerator ShotgunShot(Vector3 enemyPosition)
        {
            for (int i = 0; i < bulletsPerShot; i++)
            {
                GameObject bulletInstance =
                    Instantiate(BulletPrefab, FirePoint.transform.position, Quaternion.identity);

                AbstractBullet abstractBulletScript = bulletInstance.GetComponent<AbstractBullet>();

                abstractBulletScript.SetStats(GunDamage, BulletSpeed, BulletPiercing, verticalSpread);
                abstractBulletScript.SetDirection(enemyPosition);
                
                yield return new WaitForSeconds(horizontalSpread);
            }
            
        }

    }
}

