using System;
using UnityEngine;
using UnityEngine.Serialization;

public class Stopwatch : MonoBehaviour
{
    private string _timerText;
    private float _time; 
    private int _lastSecond;
    public bool isTimerRunning = false;
    
    public static Action<int> OnTimerChanged;
    public static Action<string> OnTimerTextChanged;

    
    void Update()
    {
        Timer();
    }

    public void StartTimer()
    {
        isTimerRunning = true;
    }
    public void StopTimer()
    {
        isTimerRunning = false;
    }

    private void Timer()
    {
        if (isTimerRunning)
        {
            // time from last frame
            _time += Time.deltaTime; //Time.realtimeSinceStartup

            // check for second
            if ((int)_time > _lastSecond)
            {
                _lastSecond = (int)_time;
                OnTimerChanged?.Invoke(_lastSecond+1);
            }


            // time format
            string minutes = ((int)_time / 60).ToString("00");
            string seconds = (_time % 60).ToString("00");
            string fraction = ((int)(_time * 100) % 100).ToString("00");
            
            _timerText = minutes + ":" + seconds;
            
            OnTimerTextChanged?.Invoke(_timerText);
        }
    }
}
