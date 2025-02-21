using System;
using System.Collections;
using System.Collections.Generic;
using _Scripts.Bullets;
using UnityEngine;
using Random = UnityEngine.Random;


namespace _Scripts.EnemiesScripts
{
    public abstract class AbstractEnemy : MonoBehaviour
    {
        private GameObject _player;
        private BoxCollider2D _boxCollider;
        private Animator _enemyAnimator;
        
        public string enemyName = "AbstractEnemy";
        public  float enemyMaxHealth = 1;
        public  float enemyHealth = 1;
        public  float enemySpeed = 1;
        private float _randomSpeedModifier;
        public  int enemyDamage = 1;
        protected int EnemyScore = 1;

        
        public bool isMinionSpawner = false;
        public int spawnMinionCount = 0;
        public float spawnMinionRate = 0;
        public string spawnMinionName = "";
        
        public bool isSpawnMinionAfterDeath = false;
        public int spawnAfterDeathCount = 0;
        public string spawnMinionAfterDeathName = "";
        
        public bool onAim = false;
        
        private bool _isImmune = false;
        private bool _isDead = false;

        public bool isChasing;

        protected List<GameObject> DamagedBullets;
        
        
        public event Action <GameObject> OnDeathCurrentEnemy;
        public static Action <GameObject> OnDeathAbstractEnemy;
        public event Action <GameObject> OnMinionSpawn;
        public static Action <float> OnGiveScore;


        protected void Awake()
        {
            StartCoroutine(InvincibilityTimer());
        }
        protected virtual void Start()
        {
            _enemyAnimator = GetComponent<Animator>();
            _boxCollider = GetComponent<BoxCollider2D>();
            _player = GameObject.Find("Player");
            isChasing = true;
            DamagedBullets = new List<GameObject>();
            _randomSpeedModifier =  Random.Range(0f, 0.1f);
            
            if (isMinionSpawner)
            {
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

            //gameObject.GetComponent<SpriteRenderer>().color = onAim ? Color.red : Color.white;
            
            //check this go subscribers
            //if (OnDeathCurrentEnemy != null) Debug.Log("enemy "+gameObject.GetInstanceID()+" "+OnDeathCurrentEnemy.GetInvocationList().Length);
        }
        
        
        private Collider2D[] GetCollidedObjectsByMask(params string[] layerMasks)
        {
            Collider2D[] hits =
                Physics2D.OverlapBoxAll(transform.position, _boxCollider.size, 0f, LayerMask.GetMask(layerMasks));
            return hits;
        }

        
        protected virtual void TakeDamage(int damage)
        {
            if (!_isImmune && !_isDead)
            {
                //AudioManager.Instance.PlayEnemyTakesDamageClip(enemyName);
                _enemyAnimator.SetTrigger("IsDamaged");
                enemyHealth -= damage;
                ChangeHpBar(damage);
                if (enemyHealth <= 0)
                {
                    _isDead = true;
                    Death();
                }
            }
        }

        protected virtual void ChangeHpBar(float damageValue)
        {
            
        }
        
        protected virtual void Death()
        {
            OnDeathAbstractEnemy?.Invoke(gameObject);
            OnDeathCurrentEnemy?.Invoke(this.gameObject); //obj info for spawner (to spawn minions)
            OnGiveScore?.Invoke(EnemyScore);
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
            if (isChasing)
            {
                _enemyAnimator.SetBool("IsWalking",true);
                transform.position = Vector3.MoveTowards(transform.position, _player.transform.position, Time.fixedDeltaTime * (enemySpeed+_randomSpeedModifier));

                //var distance = Vector3.Distance(transform.position, _player.transform.position);
            }

            if (hits.Length > 0)
            {
                
                _player.GetComponent<PlayerBehaviour>().TakeDamage(enemyDamage);

            }
        }

        protected virtual IEnumerator SpawnMinion()
        {
            while (true)
            {
                
                //Debug.Log("Spawning minion in enemy script");
                OnMinionSpawn?.Invoke(this.gameObject);
                yield return new WaitForSeconds(spawnMinionRate);
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

