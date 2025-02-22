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

    public abstract class AbstractGun : MonoBehaviour
    {

        protected GameObject BulletPrefab;
        protected GameObject FirePoint;

        [SerializeField] protected string GunName;
        [SerializeField] protected string BulletType;
        [SerializeField] protected int GunDamage = 0;
        [SerializeField] protected int BulletSpeed = 0;
        [SerializeField] protected int MaxAmmo = 0;
        [SerializeField] protected int Ammo = 0;
        [SerializeField] protected int AmmoConsumptionPerShot = 0;
        [SerializeField] protected float FireDelay = 0;
        [SerializeField] protected float GunReloadTime = 0;
        [SerializeField] protected float GunFireRadius = 0; //1 = 100 pix
        [SerializeField] private float _gunRotateRadius = 0; //1 = 100 pix
        [SerializeField] private float _aimRadius = 0; //1 = 100 pix
        [SerializeField] protected int BulletPiercing = 0;
        [FormerlySerializedAs("spread")] public int verticalSpread = 0;

        private GameObject _closestEnemyGameObjectPast; //todo check usage
        private GameObject _closestEnemyGameObjectCurrent;

        
        private bool _isReloading = false;
        private readonly float _switchingEnemyRate = 0.01f;
        private Coroutine _shootingCoroutine;
        
//Weapon setup
        protected virtual void Start()
        {
            AudioManager.Instance.DrawWeaponClip();
            
            _gunRotateRadius = GunFireRadius + 1.5f;
            _aimRadius = GunFireRadius + 2;
            
            ChangeBulletType(BulletType);
            SetFirePoint();
            
            _shootingCoroutine = StartCoroutine(Shooting());
        }


        void Update()
        {

            AimTargetEnemy();

            if (!_isReloading)
            {
                RotateGunToEnemy();
            }
            else
            {
                ReloadRotation(GunReloadTime);
            }
        }

        

        private void OnDisable()
        {
            StopShooting();
        }


        protected virtual void ChangeBulletType(string type)
        {
            BulletPrefab = Resources.Load<GameObject>("BulletTypePrefabs/" + type);
        }

        //bullets creates and fly from here
        protected virtual void SetFirePoint()
        {
            FirePoint = transform.GetChild(0).gameObject;
        }

        protected virtual void FireBullet(Vector3 enemyPosition)
        {
            GameObject bulletInstance = Instantiate(BulletPrefab, FirePoint.transform.position, Quaternion.identity);

            AbstractBullet abstractBulletScript = bulletInstance.GetComponent<AbstractBullet>();

            abstractBulletScript.SetStats(GunDamage, BulletSpeed, BulletPiercing,verticalSpread);
            abstractBulletScript.SetDirection(enemyPosition);
        }


        //auto shooting sequence
        private IEnumerator Shooting()
        {
            while (true)
            {
                //handle gunfire radius (uses targetPlayer from aim methods)
                Transform localEnemyTarget = null;
                if (_closestEnemyGameObjectCurrent != null)
                {
                    if (Vector3.Distance(transform.position, _closestEnemyGameObjectCurrent.transform.position) <=
                        GunFireRadius)
                    {
                        localEnemyTarget = _closestEnemyGameObjectCurrent.transform;
                    }
                }
                
                //Shooting if there is a targetPlayer in gunfire radius
                if (localEnemyTarget != null)
                {
                    //Debug.Log("ammo before shot = "+ammo);
                    FireBullet(_closestEnemyGameObjectCurrent.transform.position);
                    AudioManager.Instance.PlayGunShootClip(GunName);
                    Ammo -= AmmoConsumptionPerShot;
                    //Debug.Log("ammo after shot = "+ammo);
                    
                    //reload
                    if (Ammo <= 0)
                    {
                        //Debug.Log("reloading");
                        AudioManager.Instance.PlayGunReloadClip(GunName);
                        _isReloading = true;
                        yield return new WaitForSeconds(GunReloadTime);
                        Ammo = MaxAmmo;
                        _isReloading = false;
                    }
                    //or continue firing using delay
                    else
                    {
                        yield return new WaitForSeconds(FireDelay);
                        //Debug.Log("fire delaty");
                    }
                }

                yield return new WaitForSeconds(_switchingEnemyRate);
                
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
            //handle aim radius for animation
            Transform enemyTransform = null;
            if (_closestEnemyGameObjectCurrent != null)
            {
                if (Vector3.Distance(transform.position, _closestEnemyGameObjectCurrent.transform.position) <=
                    _gunRotateRadius)
                {
                    enemyTransform = _closestEnemyGameObjectCurrent.transform;
                }
            }


            if (enemyTransform != null)
            {
                Vector3 directionToTarget = enemyTransform.position - transform.position;

                // rotation angle calc
                float angle = Mathf.Atan2(directionToTarget.y, directionToTarget.x) * Mathf.Rad2Deg;

                // apply rotation to Z
                transform.rotation = Quaternion.Euler(0f, 0f, angle);

                // check on which side is enemy
                bool isOnLeftSide = directionToTarget.x > 0;

                // mirroring
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
        

        void AimTargetEnemy()
        {
            
        //Handle blink switch targetPlayer    
        float proximityThreshold = 0.1f;  
        // search for closest not aimed enemy
        GameObject unaimedEnemy = FindClosestUnaimedEnemy(_aimRadius);

        // new targetPlayer
        if (unaimedEnemy != null)
        {
            // compare with old targetPlayer
            if (_closestEnemyGameObjectCurrent != null)
            {
                float distanceToNewEnemy = Vector3.Distance(transform.position, unaimedEnemy.transform.position);
                float distanceToOldEnemy = Vector3.Distance(transform.position, _closestEnemyGameObjectCurrent.transform.position);

                // switch if new targetPlayer closer then old
                if (distanceToNewEnemy < distanceToOldEnemy - proximityThreshold)
                {
                    _closestEnemyGameObjectCurrent.GetComponent<AbstractEnemy>().onAim = false;
                    _closestEnemyGameObjectCurrent = unaimedEnemy;
                    _closestEnemyGameObjectCurrent.GetComponent<AbstractEnemy>().onAim = true;
                }
            }
            else
            {
                // if no targetPlayer aim new
                _closestEnemyGameObjectCurrent = unaimedEnemy;
                _closestEnemyGameObjectCurrent.GetComponent<AbstractEnemy>().onAim = true;
            }
        }
        else
        {
            // if no targetPlayer but was old, aim it
            if (_closestEnemyGameObjectCurrent != null)
            {
                _closestEnemyGameObjectCurrent.GetComponent<AbstractEnemy>().onAim = true;
            }
            else
            {
                // if no anaimed and current targetPlayer, search for single aimed (handle multi guns to one targetPlayer)
                _closestEnemyGameObjectCurrent = FindSingleAimedEnemy(_aimRadius);
                if (_closestEnemyGameObjectCurrent != null)
                {
                    _closestEnemyGameObjectCurrent.GetComponent<AbstractEnemy>().onAim = true;
                }
            }
        }
        
        _closestEnemyGameObjectPast = _closestEnemyGameObjectCurrent;
    }
        
        
        protected virtual GameObject FindClosestUnaimedEnemy(float radius)
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
        
        
        protected virtual GameObject FindSingleAimedEnemy(float radius)
        {
            Collider2D[] enemiesInRange = Physics2D.OverlapCircleAll(transform.position, radius, LayerMask.GetMask("Enemy"));

            if (enemiesInRange.Length > 0)
            {
                foreach (Collider2D col in enemiesInRange)
                {
                    AbstractEnemy enemy = col.GetComponent<AbstractEnemy>();
                    if (enemy.onAim)
                    {
                        return col.gameObject;
                    }
                }
            }

            return null;
        }
        
        
    }
}