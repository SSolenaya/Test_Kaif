using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts;
using TMPro;
using UnityEngine;

public class UIController : MonoBehaviour
{
    public static UIController Inst;
    [SerializeField] private BallsPanelUI _ballsPanelUI;
    [SerializeField] private TMP_Text _counterTxt;
    [SerializeField] private TMP_Text _timerTxt;

    void Awake()
    {
        if (Inst == null)
        {
            Inst = this;
        }
    }

    public void Setup()
    {
        _ballsPanelUI.Setup();
        BallController.Inst.SubscribeForBallsAmount(ShowAvailableBalls);
        CubeController.Inst.SubscribeForDestroyedCubesCount(ChangeScoreView);
    }

    private void ShowAvailableBalls(int ballsAmount)
    {
        _ballsPanelUI.ChangeAvailableBallUI(ballsAmount);
    }

    private void ChangeScoreView(int newScore)
    {
        _counterTxt.text = newScore.ToString();
    }

    public void ChangeTimerToNextBall(float timeRemain)
    {
        _timerTxt.text = Math.Abs(timeRemain) < 0.001f? "": timeRemain.ToString("0.0");
    }
}
