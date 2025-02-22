using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace _Scripts
{
    public class CameraBehaviour : MonoBehaviour
    {
        public static CameraBehaviour Instance;

       [SerializeField] private float cameraShakeDurationOnPlayerGetDamage = 0.05f;
       [SerializeField]private float cameraShakeMagnitudeOnPlayerGetDamage = 0.03f;
        
       [SerializeField]private float cameraShakeDurationOnPlayerDeath = 1f;
       [SerializeField]private float cameraShakeMagnitudeOnPlayerDeath = 1f;
        
        
        
        private PlayerBehaviour _player;
        public Transform targetPlayer;
        private bool _isShaking = false;

        void LateUpdate()
        {
            
            if (!_isShaking)
            {
                transform.position =
                    new Vector3(targetPlayer.position.x, targetPlayer.position.y,
                        gameObject.transform.position.z); // Перемещаем камеру сразу за персонажем
            }
        }

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
        }


        private void Start()
        {
            PlayerBehaviour.OnPlayerTakeDamage += CameraShakeOnPlayerDamaged;
            PlayerBehaviour.OnPlayerDeath += CameraShakeOnPlayerDeath; 
        }


        private void OnDisable()
        {
            PlayerBehaviour.OnPlayerTakeDamage -= CameraShakeOnPlayerDamaged;
            PlayerBehaviour.OnPlayerDeath -= CameraShakeOnPlayerDeath;
        }

        private void CameraShakeOnPlayerDeath()
        {
            _isShaking = true;
            StartCoroutine(Shake(cameraShakeDurationOnPlayerDeath, cameraShakeMagnitudeOnPlayerDeath));
            Debug.Log("death shaking");
        }
        
        private void CameraShakeOnPlayerDamaged()
        {
            _isShaking = true;
            StartCoroutine(Shake(cameraShakeDurationOnPlayerGetDamage, cameraShakeMagnitudeOnPlayerGetDamage));
        }

        public void cameraShakeOnShoot(float duration, float magnitude)
        {
            _isShaking = true;
            StartCoroutine(Shake(duration, magnitude));
        }
        
        //use for shake effects
        public IEnumerator Shake(float duration, float magnitude)
        {
            Vector3 originalPos = transform.localPosition;

            float elapsedTime = 0f;

            while (elapsedTime < duration)
            {
                float x = Random.Range(-1f, 1f) * magnitude;
                float y = Random.Range(-1f, 1f) * magnitude;

                transform.localPosition = new Vector3(originalPos.x + x, originalPos.y + y, originalPos.z);

                elapsedTime += Time.deltaTime;

                yield return null;
            }

            transform.localPosition = originalPos;
            _isShaking = false;
        }
    }
}

