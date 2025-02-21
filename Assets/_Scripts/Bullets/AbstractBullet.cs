using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;

namespace _Scripts.Bullets
{
    public abstract class
        AbstractBullet : MonoBehaviour //Bullet Gets targets and stats from weapon. Then transmit it to enemies during hit (hit processed by enemy).
    {
        private Vector3 _targetDirection;
        private Vector3 _initialForward;
        public int damage = 0;
        public float speed = 0;
        public int piercing = 0;
        public int spread = 0;

        //protected CircleCollider2D _collider2D;

        protected virtual void Start()
        {
            //_collider2D = GetComponent<CircleCollider2D>();
        }

        protected virtual void FixedUpdate()
        {
            //transform.Translate(_targetDirection * (speed * Time.fixedDeltaTime));
            transform.Translate(_targetDirection * (speed * Time.fixedDeltaTime), Space.World);
            
            DestroyBulletByWall();
        }


        public void SetDirection(Vector3 targetPosition)
        {
            // Calculate the base direction to the target
            Vector3 baseDirection = (targetPosition - transform.position).normalized;

            // Generate a random verticalSpread angle within the specified range
            float randomAngle = Random.Range(-spread, spread);

            // Convert the angle to radians
            float randomRadians = randomAngle * Mathf.Deg2Rad;

            // Create a rotation quaternion for the random angle
            Quaternion randomRotation = Quaternion.Euler(0f, 0f, randomAngle);

            // Apply the random rotation to the base direction
            _targetDirection = randomRotation * baseDirection;

            // Rotate the bullet to face the new direction
            float angle = Mathf.Atan2(_targetDirection.y, _targetDirection.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0f, 0f, angle);
        }
        
        public void SetStats(int gunDamage, float bulletSpeed, int bulletPiercing,int bulletSpread)
        {
            damage = gunDamage;
            speed = bulletSpeed;
            piercing = bulletPiercing;
            spread = bulletSpread;
        }


        private void DestroyBulletByWall()
        {
            var col = Physics2D.OverlapCircle(transform.position, gameObject.GetComponent<CircleCollider2D>().radius, LayerMask.GetMask("Blocking"));
            if (col != null)
            {
                Destroy(gameObject);
            }
        }
        
        
        /*
        void MoveTowardsTarget()
        {
            Vector3 direction = (_target.position - transform.position).normalized;


            transform.Translate(direction * enemySpeed * Time.deltaTime);
        }

        public void SetTarget(Transform newTarget)
        {
            _target = newTarget;
        }*/
    }
    
}


    
    

