using System;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UIElements;

public class StartMenu : MonoBehaviour
{

    public static Action<string> OnStartGame;
    
    [SerializeField] private TMP_Text label;
    private bool _isFadingIn = true;
    private float _currentAlpha;
    
    private const float FadeDuration = 1.0f;
    private const float MinAlpha = 0.0f;
    private const float MaxAlpha = 1.0f;


    public void StartArena()
    {
        OnStartGame?.Invoke("Arena");
    }

    public void CloseStartMenu()
    {
        gameObject.SetActive(false);
    }

    private void Update()
    {
        GameNameLabelAnimation();
    }

    private void GameNameLabelAnimation()
    {
        if (_isFadingIn)
        {
            _currentAlpha += Time.deltaTime / FadeDuration;
            if (_currentAlpha >= MaxAlpha)
            {
                _currentAlpha = MaxAlpha;
                _isFadingIn = false;
            }
        }
        else
        {
            _currentAlpha -= Time.deltaTime / FadeDuration;
            if (_currentAlpha <= MinAlpha)
            {
                _currentAlpha = MinAlpha;
                _isFadingIn = true;
            }
        }
        
        label.alpha = _currentAlpha;
    }
    
    
}
