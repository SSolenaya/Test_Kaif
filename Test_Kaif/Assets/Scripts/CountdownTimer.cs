using System;
using UnityEngine;

public class CountdownTimer
{
    private bool _isCountingAllowed;
    private float _countdownTime;
    private Action _finalAct; 
    private Action<float> _timerAct; 

    public void Setup(float time, Action closureAction)
    {
        _countdownTime = time;
        _finalAct = closureAction;
        _isCountingAllowed = true;
    }

    public void Reset()
    {
        _finalAct = null;
        _timerAct = null;
        _countdownTime = 0;
        _isCountingAllowed = false;
    }

    public void SubscribeForTime(Action<float> act)
    {
        _timerAct += act;
    }

    public void Countdown()
    {
        if (_isCountingAllowed)
        {
            _countdownTime -= Time.deltaTime;
            if (_countdownTime <= 0)
            {
                _countdownTime = 0;
                _isCountingAllowed = false;
                _finalAct?.Invoke();
            }

            _timerAct?.Invoke(_countdownTime);
        }
    }
}