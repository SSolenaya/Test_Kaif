using System;
using Assets.Scripts;
using UnityEngine;

public class BallController : MonoBehaviour
{
    public static BallController Inst;

    [SerializeField] private RaycastController _raycastController;
    [SerializeField] private Ball _ballPrefab;
    [SerializeField] private Transform _spawnPoint;

    private bool _isBallReady = true;
    private CountdownTimer _shootTimer = new CountdownTimer();
    private CountdownTimer _ballCreatingTimer = new CountdownTimer();

    private Action<int> _onBallsAmountChange;
    private int _ballCounter;

    public int BallCounter
    {
        get => _ballCounter;
        set
        {
            _ballCounter = value > 5 ? 5 : value;
            if (_ballCounter < 5)
            {
                _ballCreatingTimer.Setup(MainController.Inst.globalParams.coolDownForNewBall, () => BallCounter++);
            }

            _onBallsAmountChange?.Invoke(BallCounter);
        }
    }

    private void Awake()
    {
        if (Inst == null)
        {
            Inst = this;
        }
    }

    public void Setup()
    {
        _raycastController.SubscribeForFindingTarget(ShootBall);
        BallCounter = MainController.Inst.globalParams.startBallsAmount;
        _ballCreatingTimer.SubscribeForTime(UIController.Inst.ChangeTimerToNextBall);
    }

    private void ShootBall(RaycastHit raycastHit)
    {
        if (!_isBallReady || BallCounter == 0)
        {
            return;
        }

        Ball ball = PoolManager.Inst.GetBallFromPull(_ballPrefab);
        ball.transform.SetParent(_spawnPoint);
        Vector3 endPos = raycastHit.point;
        ball.SetDestination(_spawnPoint.position, endPos,
            () => CubeController.Inst.CheckDestroyingCubes(endPos, MainController.Inst.globalParams.ballRadiusOfBlast));
        BallCounter--;
        _isBallReady = false;
        _shootTimer.Setup(MainController.Inst.globalParams.coolDownForShoot, () => _isBallReady = true);
    }

    private void Update()
    {
        _shootTimer.Countdown();
        _ballCreatingTimer.Countdown();
    }

    public void SubscribeForBallsAmount(Action<int> act)
    {
        _onBallsAmountChange += act;
    }
}