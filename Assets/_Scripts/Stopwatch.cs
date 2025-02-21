using System;
using UnityEngine;

public class Stopwatch : MonoBehaviour
{
    private string _timerText; // Текстовый объект для отображения времени
    private float _time; // Переменная для хранения времени
    private int _lastSecond;
    public bool _isTimerRunning = false;
    
    public static Action<int> OnTimerChanged;
    public static Action<string> OnTimerTextChanged;

    
    void Update()
    {
        Timer();
    }

    public void StartTimer()
    {
        _isTimerRunning = true;
    }
    public void StopTimer()
    {
        _isTimerRunning = false;
    }

    private void Timer()
    {
        if (_isTimerRunning)
        {
            // Добавляем время, прошедшее с последнего кадра
            _time += Time.deltaTime; //Time.realtimeSinceStartup

            // Проверяем, прошла ли целая секунда
            if ((int)_time > _lastSecond)
            {
                // Вызываем метод, когда прошла целая секунда
                _lastSecond = (int)_time;
                OnTimerChanged?.Invoke(_lastSecond+1);
            }


            // Форматируем время в виде минут:секунд:сотые
            string minutes = ((int)_time / 60).ToString("00");
            string seconds = (_time % 60).ToString("00");
            string fraction = ((int)(_time * 100) % 100).ToString("00");

            // Обновляем текстовый объект
            //_timerText = minutes + ":" + seconds + ":" + fraction;
            _timerText = minutes + ":" + seconds;
            
            OnTimerTextChanged?.Invoke(_timerText);
        }
    }
}
