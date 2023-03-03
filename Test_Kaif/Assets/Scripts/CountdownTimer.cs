using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CountdownTimer
{
    private bool _isCountingAllowed = false;
    private float _countdownTime;
    private Action _act;

   public void Setup(float time, Action closureAction)
    {
        _countdownTime = time;
        _act = closureAction;
        _isCountingAllowed = true;
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
                _act?.Invoke();
            }
        }
    }


}
