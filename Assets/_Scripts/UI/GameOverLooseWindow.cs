using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace _Scripts.UI
{
    public class GameOverLooseWindow: MonoBehaviour
    {
        [SerializeField]private GameObject background;
        [SerializeField]private Image leftCorner;
        [SerializeField]private Image rightCorner;
        [SerializeField]private Image topCorner;
        [SerializeField]private Image downCorner;
        
        [SerializeField]private GameObject uiElements;
        [SerializeField]private TMP_Text scoreText;

        private float _score;


        void Start()
        {
            GameManager.OnGameOverLoose += GameOver;
        }
        
        
        
        private void GameOver(float score)
        {
            _score = score;
            
            StartCoroutine(GameOverAnimationCoroutine());
        }

        private void OnDisable()
        {
            GameManager.OnGameOverLoose -= GameOver;
        }


        private IEnumerator GameOverAnimationCoroutine()
        {
            background.SetActive(true);
            for (int i = 0; i < 100; i++)
            {
                leftCorner.fillAmount = i / 100f;
                rightCorner.fillAmount = i / 100f;
                topCorner.fillAmount = i / 100f;
                downCorner.fillAmount = i / 100f;
                yield return new WaitForSeconds(0.01f);
            }
            
            uiElements.SetActive(true);
            scoreText.text = _score.ToString();
        }

        public void RestartGame()
        {
            SceneManager.LoadScene(0);
        }
    }
}
