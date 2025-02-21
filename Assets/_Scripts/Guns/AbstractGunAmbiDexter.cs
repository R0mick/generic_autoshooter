using System;
using System.Collections;
using System.Collections.Generic;
using _Scripts.Bullets;
using _Scripts.EnemiesScripts;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

namespace _Scripts.Guns
{
    public abstract class AbstractGunAmbi : MonoBehaviour
    {

        public GameObject bulletPrefab;
        public Transform enemyTarget;
        public string pathToBullet = "Assets/Prefabs/BulletTypes/";
        public GameObject firePoint;

        public string gunName;
        public string bulletType;
        public int gunDamage = 0;
        public int bulletSpeed = 0;
        public int maxAmmo = 0;
        public int ammo = 0;
        public int ammoConsumptionPerShot = 0;
        public float fireDelay = 0;
        public float gunReloadTime = 0;
        public float aimRadius = 0; //1 = 100 pix
        public float gunFireRadius = 0; //1 = 100 pix
        public int bulletPiercing = 0;

        public GameObject closestEnemyGameObjectPast;
        public GameObject closestEnemyGameObjectCurrent;
        /*public event Action <GameObject> OnEquip;
        public event Action <GameObject> OnUnEquip;
        public event Action <string> OnGunShoot;
        public event Action <string> OnGunReload;
        */
        
        private bool _isReloading = false;
        private readonly float _scanForEnemiesDelay = 0.1f;
        private Coroutine _shootingCoroutine;


        //public static List<Transform> enemyTransformList;

        protected virtual void OnEnable()
        {

            _shootingCoroutine = StartCoroutine(Shooting());

        }

        protected virtual void Start()
        {
            ChangeBulletType(bulletType);
            SetFirePoint();
        }


        void Update()
        {

            Highlite();

            if (!_isReloading)
            {
                RotateGunToEnemy();
            }
            else
            {
                ReloadRotation(gunReloadTime);
            }
        }


        private void OnDisable()
        {
            StopShooting();
        }


        protected virtual void ChangeBulletType(string type)
        {
            //Debug.Log(pathToBullet + type + ".prefab");
            //bulletPrefab = AssetDatabase.LoadAssetAtPath<GameObject>(pathToBullet + type + ".prefab");
        }

        protected virtual void SetFirePoint()
        {
            firePoint = transform.GetChild(0).gameObject;
        }

        protected virtual void FireBullet(Vector3 enemyPosition)
        {
            GameObject bulletInstance = Instantiate(bulletPrefab, firePoint.transform.position, Quaternion.identity);

            AbstractBullet abstractBulletScript = bulletInstance.GetComponent<AbstractBullet>();

            abstractBulletScript.SetStats(gunDamage, bulletSpeed, bulletPiercing,0);
            abstractBulletScript.SetDirection(enemyPosition);

        }


        protected IEnumerator Shooting()
        {
            while (true)
            {
                Transform enemyTarget = null;

                if (closestEnemyGameObjectCurrent != null)
                {
                    if (Vector3.Distance(transform.position, closestEnemyGameObjectCurrent.transform.position) <=
                        gunFireRadius)
                    {
                        enemyTarget = closestEnemyGameObjectCurrent.transform;
                    }
                }
                
                if (enemyTarget != null)
                {
                    //Debug.Log("ammo before shot = "+ammo);
                    FireBullet(enemyTarget.position);
                    AudioManager.Instance.PlayGunShootClip(gunName);
                    ammo -= ammoConsumptionPerShot;
                    //Debug.Log("ammo after shot = "+ammo);
                    if (ammo <= 0)
                    {
                        //Debug.Log("reloading");
                        AudioManager.Instance.PlayGunReloadClip(gunName);
                        _isReloading = true;
                        yield return new WaitForSeconds(gunReloadTime);
                        ammo = maxAmmo;
                        _isReloading = false;
                    }
                    else
                    {
                        yield return new WaitForSeconds(fireDelay);
                        //Debug.Log("fire delaty");
                    }
                }

                yield return new WaitForSeconds(_scanForEnemiesDelay); //scan rate
                //Debug.Log("scanning");
            }
        }


        private void StopShooting()
        {
            if (_shootingCoroutine != null)
            {
                StopCoroutine(_shootingCoroutine);

                _shootingCoroutine = null;
            }
        }

        private void ReloadRotation(float reloadTime)
        {
            var rotationSpeed = 360f / reloadTime;
            //transform.localScale = new Vector3(1.2f, 1.2f, 0f);
            if (transform.localScale.y > 0)
            {
                transform.Rotate(Vector3.forward * Time.deltaTime * rotationSpeed);
            }
            else
            {
                transform.Rotate(Vector3.forward * Time.deltaTime * -rotationSpeed);
            }
                

        }

        protected virtual void RotateGunToEnemy()
        {
            Transform enemyTransform = null;

            if (closestEnemyGameObjectCurrent != null)
            {
                if (Vector3.Distance(transform.position, closestEnemyGameObjectCurrent.transform.position) <=
                    aimRadius)
                {
                    enemyTransform = closestEnemyGameObjectCurrent.transform;
                }
            }


            if (enemyTransform != null)
            {
                Vector3 directionToTarget = enemyTransform.position - transform.position;

                // Вычисление угла поворота
                float angle = Mathf.Atan2(directionToTarget.y, directionToTarget.x) * Mathf.Rad2Deg;

                // Применение поворота по оси Z
                transform.rotation = Quaternion.Euler(0f, 0f, angle);

                // Проверяем, где находится цель относительно оружия
                bool isOnLeftSide = directionToTarget.x > 0;

                // Масштабирование для зеркального отражения
                if (isOnLeftSide)
                {
                    transform.localScale = new Vector3(-1.2f, -1.2f, 0f);
                }
                else
                {
                    transform.localScale = new Vector3(-1.2f, 1.2f, 0f);
                }
            }
            else
            {
                //check right or left gun slot for idle
                if (gameObject.transform.parent.name.Contains("R"))
                {
                    transform.localScale = new Vector3(1.2f, 1.2f, 0f);
                    transform.rotation = Quaternion.Euler(0f, 0f, 180f);
                }

                if (gameObject.transform.parent.name.Contains("L"))
                {
                    transform.localScale = new Vector3(-1.2f, 1.2f, 0f);
                    transform.rotation = Quaternion.Euler(0f, 0f, 180f);
                }
            }

        }
        

        void Highlite()
        {
            float proximityThreshold = 0.1f;
            // Поиск ближайшего неподсвеченного врага
            GameObject unaimedEnemy = HighliteClosestUnaimedEnemy(aimRadius);

            // Если найдена новая цель
            if (unaimedEnemy != null)
            {
                // Если была старая цель, сравниваем расстояние
                if (closestEnemyGameObjectCurrent != null)
                {
                    float distanceToNewEnemy = Vector3.Distance(transform.position, unaimedEnemy.transform.position);
                    float distanceToOldEnemy = Vector3.Distance(transform.position, closestEnemyGameObjectCurrent.transform.position);

                    // Если новый враг ближе чем старый на пороговое значение, переключаемся
                    if (distanceToNewEnemy < distanceToOldEnemy - proximityThreshold)
                    {
                        closestEnemyGameObjectCurrent.GetComponent<AbstractEnemy>().onAim = false;
                        closestEnemyGameObjectCurrent = unaimedEnemy;
                        closestEnemyGameObjectCurrent.GetComponent<AbstractEnemy>().onAim = true;
                    }
                }
                else
                {
                    // Если старой цели не было, просто назначаем новую
                    closestEnemyGameObjectCurrent = unaimedEnemy;
                    closestEnemyGameObjectCurrent.GetComponent<AbstractEnemy>().onAim = true;
                }
            }
            else
            {
                // Если новой цели нет, но была старая, оставляем её подсвеченной
                if (closestEnemyGameObjectCurrent != null)
                {
                    closestEnemyGameObjectCurrent.GetComponent<AbstractEnemy>().onAim = true;
                }
            }

            // Сохраняем старую цель для следующей итерации
            closestEnemyGameObjectPast = closestEnemyGameObjectCurrent;
        }
        
        
        protected virtual GameObject HighliteClosestUnaimedEnemy(float radius)
        {
            Collider2D[] enemiesInRange = Physics2D.OverlapCircleAll(transform.position, radius, LayerMask.GetMask("Enemy"));

            if (enemiesInRange.Length > 0)
            {
                GameObject closestUnaimedEnemy = null;
                float shortestDistance = Mathf.Infinity;

                foreach (Collider2D col in enemiesInRange)
                {
                    AbstractEnemy enemy = col.GetComponent<AbstractEnemy>();
                    if (!enemy.onAim)
                    {
                        float distanceToEnemy = Vector3.Distance(transform.position, col.transform.position);
                        if (distanceToEnemy < shortestDistance)
                        {
                            closestUnaimedEnemy = col.gameObject;
                            shortestDistance = distanceToEnemy;
                        }
                    }
                }

                return closestUnaimedEnemy;
            }

            return null;
        }
        
        
    }
}