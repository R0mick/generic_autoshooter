using System;
using System.Collections;
using System.Collections.Generic;
using _Scripts.Bullets;
using UnityEngine;
using UnityEngine.Serialization;

namespace _Scripts.EnemiesScripts
{
    public abstract class AbstractEnemy_bck : MonoBehaviour
    {
        private GameObject _player;
        private BoxCollider2D _boxCollider;
        
        public string enemyName = "AbstractEnemy";
        public  int enemyHealth = 1;
        public  float enemySpeed = 1;
        public  int enemyDamage = 1;
        
        public bool isMinionSpawner = false;
        public int spawnMinionCount = 0;
        public float spawnMinionRate = 0;
        public bool isSpawnMinionAfterDeath = false;
        public int spawnAfterDeathCount = 0;
        public string spawnMinionName = "";
        
        
        private bool _isImmune = false;

        public bool isChasing;

        protected List<GameObject> DamagedBullets;
        
        
        public event Action <GameObject> OnDeathCurrentEnemy;
        public event Action <GameObject> OnMinionSpawn;


        protected void Awake()
        {
            StartCoroutine(InvincibilityTimer());
        }
        protected virtual void Start()
        {
            _boxCollider = GetComponent<BoxCollider2D>();
            _player = GameObject.Find("Player");
            isChasing = true;
            DamagedBullets = new List<GameObject>();
            
            if (isMinionSpawner)
            {
                Debug.Log(gameObject.name);
                StartCoroutine(SpawnMinion());
            }
        }


        protected virtual void FixedUpdate()
        {
            if (isChasing)
            {
                ChasePlayer();
            }

            CheckAndHandleBulletHit();
            
            //check this go subscribers
            //if (OnDeathCurrentEnemy != null) Debug.Log("enemy "+gameObject.GetInstanceID()+" "+OnDeathCurrentEnemy.GetInvocationList().Length);
        }

        /*private Collider2D[] CheckForBulletCollision()
        {
            Collider2D[] hits =
                Physics2D.OverlapBoxAll(transform.position, _boxCollider.size, 0f, LayerMask.GetMask("Bullet"));
            return hits;
        }*/
        
        private Collider2D[] GetCollidedObjectsByMask(params string[] layerMasks)
        {
            Collider2D[] hits =
                Physics2D.OverlapBoxAll(transform.position, _boxCollider.size, 0f, LayerMask.GetMask(layerMasks));
            return hits;
        }

        
        protected virtual void TakeDamage(int damage)
        {
            if (!_isImmune)
            {
                //AudioManager.Instance.PlayEnemyTakesDamageClip(enemyName);
                enemyHealth -= damage;
                if (enemyHealth <= 0)
                {
                    Death();
                }
            }
        }

        protected virtual void Death()
        {
            OnDeathCurrentEnemy?.Invoke(this.gameObject);
            //Debug.Log("Enemy's "+gameObject.GetInstanceID()+" ID death method activated");
            AudioManager.Instance.PlayEnemyDeathClip(enemyName);
            Destroy(gameObject);
        }

        protected virtual void CheckAndHandleBulletHit()
        {
            Collider2D[] hits = GetCollidedObjectsByMask("Bullet");
            foreach (Collider2D hit in hits)
            {
                if (!DamagedBullets.Contains(hit.gameObject))
                {
                    TakeDamage(hit.gameObject.GetComponent<AbstractBullet>().damage);
                    if (hit.gameObject.GetComponent<AbstractBullet>().piercing > 0)
                    {
                        hit.gameObject.GetComponent<AbstractBullet>().piercing--;
                        DamagedBullets.Add(hit.gameObject);
                    }
                    else
                    {
                        Destroy(hit.gameObject);
                    }
                }
                /*else
                {
                    Debug.Log("Must be ignored for "+gameObject.name);
                    Debug.Log(DamagedBullets.Count);
                }*/
            }
        }

        protected virtual void ChasePlayer() //change to transform.translate and handle collision
        {

            Collider2D[] hits = GetCollidedObjectsByMask("Player");


            transform.position =
                Vector3.MoveTowards(transform.position, _player.transform.position, Time.fixedDeltaTime * enemySpeed);
            
            var distance = Vector3.Distance(transform.position, _player.transform.position);
            
            if (hits.Length > 0)
            {
                //Debug.Log(player.gameObject.name);
                _player.GetComponent<PlayerBehaviour>().TakeDamage(enemyDamage);

            }
        }

        protected virtual IEnumerator SpawnMinion()
        {
            while (true)
            {
                
                yield return new WaitForSeconds(spawnMinionRate);
                //Debug.Log("Spawning minion in enemy script");
                OnMinionSpawn?.Invoke(this.gameObject);
            }
        }
        
        protected virtual IEnumerator InvincibilityTimer()
        {
            _isImmune = true;
    
            yield return new WaitForSeconds(0.1f);
    
            _isImmune = false;
        }
    }
}

