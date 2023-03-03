using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using Assets.Scripts;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

public class BallController : MonoBehaviour
{
    public static BallController Inst;
    [SerializeField] private RaycastController _raycastController;
    [SerializeField] private Ball _ballPrefab;
    [SerializeField] private Transform _spawnPoint;                         //  ball spawning point
    private bool _isBallReady = true;
    private CountdownTimer _coolDownTimer = new CountdownTimer();           // for shoots
    private CountdownTimer _ballCreatingTimer = new CountdownTimer();
    private Action<int> _onBallsAmountChange;
    private int _ballCounter;
    public int BallCounter
    {
        get => _ballCounter;
        set
        {
            _ballCounter = value > 5 ? 5: value;
            if (_ballCounter < 5)
            {
                _ballCreatingTimer.Setup(MainController.Inst.globalParams.coolDownForNewBall, () => BallCounter++);
            }
            _onBallsAmountChange?.Invoke(BallCounter);
        }
    }

    void Awake()
    {
        if (Inst == null)
        {
            Inst = this;
        }
    }


    void Start()
    {
        _raycastController.SubscribeForFindingTarget(ShootBall);
        BallCounter = 1;
        _ballCreatingTimer.SubscribeForTime(UIController.Inst.ChangeTimerToNextBall);
    }

    private void ShootBall(RaycastHit raycastHit)
    {
        if (!_isBallReady || BallCounter == 0)
        {
            return;
        }

        var ball = PoolManager.Inst.GetBallFromPull(_ballPrefab);
        ball.transform.SetParent(_spawnPoint);
        Vector3 endPos = raycastHit.point;
        ball.SetDestination(_spawnPoint.position, endPos,
            () => CubeController.Inst.CheckDestroyingCubes(endPos, MainController.Inst.globalParams.ballRadiusOfBlast));
        BallCounter--;
        _isBallReady = false;
        _coolDownTimer.Setup(MainController.Inst.globalParams.coolDownForShoot, () => _isBallReady = true); 
    }


    void Update()
    {
        _coolDownTimer.Countdown();
        _ballCreatingTimer.Countdown();
    }

    public void SubscribeForBallsAmount(Action<int> act)
    {
        _onBallsAmountChange += act;
    }
}
