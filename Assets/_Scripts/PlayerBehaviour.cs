using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.Serialization;

namespace _Scripts
{
    public class PlayerBehaviour : MonoBehaviour
    {

        public static PlayerBehaviour Instance;
        
        [SerializeField] private float speed = 1.6f;
        [SerializeField] private float maxHealth;
        [SerializeField] private float currentHealth;
    
        //for testing
        [SerializeField] private List<GameObject> guns;
        
        
        private const float HitImmuneDuration = 1f;
        private bool _isImmune;
        private bool _isDead;

        private Vector3 _moveDelta;

        private BoxCollider2D _boxCollider;
        private float _horizontal;
        private float _vertical;
        private RaycastHit2D _hit;

        private Animator _playerAnimator;
     
        public static Action OnPlayerTakeDamage;
        public static Action OnPlayerDeath;
        public static Action<float> OnCurrentHpChanged;
        public static Action<float> OnMaxHpChanged;


        public float CurrentHealth
        {
            get => currentHealth;
            set
            {
                currentHealth = value;
                OnCurrentHpChanged?.Invoke(currentHealth);
            }
        }
        public float MaxHealth
        {
            get => maxHealth;
            set
            {
                maxHealth = value;
                OnMaxHpChanged?.Invoke(maxHealth);
            }
        }

        void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
        }

        void Start()
        {
            _boxCollider = GetComponent<BoxCollider2D>();
            _playerAnimator= GetComponent<Animator>();
            MaxHealth = 15;
            CurrentHealth = 15;
        }
        
        // Update is called once per frame
        void Update()
        {

            _horizontal = Input.GetAxisRaw("Horizontal");
            _vertical = Input.GetAxisRaw("Vertical");


            if (_horizontal != 0 || _vertical != 0)
            {
                _playerAnimator.SetBool("IsWalking", true);
            }
            else
            {
                _playerAnimator.SetBool("IsWalking", false);
            }
        }

        void FixedUpdate()
        {
            MovementAndCollision();
        }

        private void MovementAndCollision()
        {
            _moveDelta = Vector3.zero;
            
            _moveDelta = new Vector3(_horizontal, _vertical, 0f).normalized * speed;
        
            //Project Settings -> Physics 2D -> Queries Start In Colliders (uncheck)
            _hit = Physics2D.BoxCast(transform.position, _boxCollider.size, 0, new Vector2(0, _moveDelta.y),
                Mathf.Abs(_moveDelta.y * Time.fixedDeltaTime), LayerMask.GetMask("Blocking", "Actor"));

            if (_hit.collider == null&& !_isDead)
            {
                //movement
                transform.Translate(0, _moveDelta.y* Time.fixedDeltaTime, 0);
            }

            _hit = Physics2D.BoxCast(transform.position, _boxCollider.size, 0, new Vector2(_moveDelta.x, 0),
                Mathf.Abs(_moveDelta.x * Time.fixedDeltaTime), LayerMask.GetMask("Blocking", "Actor"));

            if (_hit.collider == null && !_isDead)
            {
                //movement
                transform.Translate(_moveDelta.x * Time.fixedDeltaTime, 0, 0);
            }
        }

        public void TakeDamage(int damage)
        {
            if (!_isImmune && !_isDead)
            {
                CurrentHealth -= damage; 
                Debug.LogFormat("Player takes {0} damage",damage);
                AudioManager.Instance.PlayHeroDamagedClip();
                _playerAnimator.SetTrigger("IsDamagedTrigger");
                if (currentHealth <= 0)
                {
                    Death();
                }
                else
                {
                    OnPlayerTakeDamage?.Invoke();
                    StartCoroutine(InvincibilityTimer());
                }
                
            }
        }
    
        private IEnumerator InvincibilityTimer()
        {
            _isImmune = true;
    
            yield return new WaitForSeconds(HitImmuneDuration);
    
            _isImmune = false;
        }

        void Death()
        {
            _playerAnimator.SetTrigger("IsDying");
            _playerAnimator.SetBool("IsDead",true);
            _isDead = true;
            OnPlayerDeath?.Invoke();
            
            //Debug.Log("<color=yellow>Implement death</color>");
        }

        public void ChangeGuns(string weaponName)
        {
            if (weaponName == "Empty")
            {
                foreach (var go in guns)
                {
                   
                    {
                        go.SetActive(false);
                    }
                }
            }
            else
            {
                foreach (var go in guns)
                {
                    if (go.name == weaponName)
                    {
                        go.SetActive(true);
                    }
                    else
                    {
                        go.SetActive(false);
                    }
                }
            }
        }
    }
}
