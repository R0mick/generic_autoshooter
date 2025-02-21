using System;
using System.Collections;
using _Scripts.EnemiesScripts;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace _Scripts
{
    public class IngameUI : MonoBehaviour
    {
   
        [SerializeField] private Image heroHpBar;
        [SerializeField] private RectTransform scoreRectTransform;
        [SerializeField] private TMP_Text scoreText;
        [SerializeField] private RectTransform stopwatchRectTransform;
        [SerializeField] private TMP_Text stopwatchText;
        [SerializeField] private GameObject mainMenuButton;

        [SerializeField]private float heroMaxHp;
        [SerializeField]private float heroCurrentHp;

        

        void Awake()
        {
            PlayerBehaviour.OnMaxHpChanged += SetMaxHp;
            PlayerBehaviour.OnCurrentHpChanged += ChangeHeroCurrentHp;
            AbstractEnemy.OnGiveScore += ChangeScore;
            Stopwatch.OnTimerTextChanged += ChangeTimerText;
            GameManager.OnGameOverWin += GameOverWinEffects;

        }
        
        private void OnDisable()
        {
            PlayerBehaviour.OnMaxHpChanged -= SetMaxHp;
            PlayerBehaviour.OnCurrentHpChanged -= ChangeHeroCurrentHp;
            AbstractEnemy.OnGiveScore -= ChangeScore;
            Stopwatch.OnTimerTextChanged -= ChangeTimerText;
            GameManager.OnGameOverWin -= GameOverWinEffects;
        }

        private void SetMaxHp(float newMaxHp)
        {
            heroMaxHp = newMaxHp;
        }
    
        private void ChangeHeroCurrentHp(float newCurrentHp)
        {
            heroCurrentHp = newCurrentHp;
            heroHpBar.fillAmount = heroCurrentHp/heroMaxHp ;
            
        }

        private void ChangeScore(float newScore)
        {
            float scoreValue = float.Parse(scoreText.text);
            scoreValue+=newScore;
            scoreText.SetText(scoreValue.ToString());
        }
        
        private void ChangeTimerText(string newTimerText)
        {
            stopwatchText.text = newTimerText;
        }

        private void GameOverWinEffects()
        {
            StartCoroutine(StopwatchMovesToCenter());
            StartCoroutine(ScoreMovesToCenter());
        }

        private IEnumerator StopwatchMovesToCenter()
        {
            for (int i = 0; i < 100; i++)
            {
                stopwatchText.fontSize += 0.1f;
                stopwatchRectTransform.anchoredPosition += new Vector2(0, -2);
                yield return new WaitForSeconds(0.01f);
            }
            
        }
        
        private IEnumerator ScoreMovesToCenter()
        {
 
            bool isInCenter = false;
            int targetX = 0;
            int targetY = 72;
            var currentX = 455;
            var currenY = 322;

            while (!isInCenter)
            {
                if (currentX > targetX)
                {
                    currentX -= 3;
                    scoreRectTransform.anchoredPosition += new Vector2(-3, 0);
                    scoreText.fontSize += 0.07f;
                }

                if (currenY > targetY)
                {
                    currenY -=2;
                    scoreRectTransform.anchoredPosition += new Vector2(0, -2);
                }

                if (currentX < targetX  &&  currenY<=targetY)
                {
                    isInCenter = true;
                }
                yield return new WaitForSeconds(0.01f);
            }

            StartCoroutine(EnableMainMenuButton()); //разобраться, че не пашет
        }

        private IEnumerator EnableMainMenuButton()
        {
            yield return new WaitForSeconds(2f);
            mainMenuButton.gameObject.SetActive(true);
        }
        
        public void RestartGame()
        {
            SceneManager.LoadScene(0);
        }
    }
}
