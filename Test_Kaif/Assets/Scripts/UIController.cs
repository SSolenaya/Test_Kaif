using System.Collections;
using System.Collections.Generic;
using Assets.Scripts;
using TMPro;
using UnityEngine;

public class UIController : MonoBehaviour
{
    [SerializeField] private BallsPanelUI _ballsPanelUI;
    [SerializeField] private TMP_Text _counterTxt;

    void Start()
    {
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
}
