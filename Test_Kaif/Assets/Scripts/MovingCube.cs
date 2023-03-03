using System;
using System.Numerics;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Random = UnityEngine.Random;
using Vector3 = UnityEngine.Vector3;

public enum CubeStates
{
    moving,
    dashing,
    destroying,
    searchingDestination,
    none
}

namespace Assets.Scripts {
    public class MovingCube : MonoBehaviour
    {
        [SerializeField] private CubeStates _currentState;

        private float _currentSpeed;
        private float _dashSpeed = 100f;    //  TODO: move to SO or Config
        private float _normalSpeed = 50f;

        private float _currentRotationSpeed;
        private float _normalRotationSpeed = 5f;    //  TODO: move to SO or Config
        private float _maxRotationSpeed = 50f;

        private float _dashTime = 0.5f;
        private float _dashDeltaMinTime = 3f;    //  TODO: move to SO or Config
        private float _dashDeltaMaxTime = 6f;
        private Vector3 _currentDestination;
        
        private float _distance = 0.5f;
        private CubeController _cubeController;
        private CountdownTimer _dashCountdownTimer = new CountdownTimer();
        private float _minRadiusForNewPoint = 80f;

        void Start()
        {
            SetState(CubeStates.searchingDestination);
        }

        public void Reset()
        {

        }

        public void SetCubeController(CubeController cc)
        {
            _cubeController = cc;
        }

        private void SetDestination()
        {
            Vector3 tempVec;
            bool flag = false;
            int count = 0;
            do
            {
                tempVec = _cubeController.playGroundController.GetRandomPoint();
                count++;
                if (count > 10)
                {
                    flag = true;
                    break;
                }
            } while (Vector3.Distance(transform.position, tempVec) <= _minRadiusForNewPoint);

            if (!flag)
            {
                _currentDestination = tempVec;
                SetState(CubeStates.moving);
            }
        }

        [ContextMenu("TestDash")]
        public void TryDash()
        {
            Vector3 finalDashPos = transform.position + transform.forward.normalized * _dashSpeed * _dashTime;
            if (PlayGroundController.Inst.IsThisPointOnPlayground(finalDashPos))
            {
                SetState(CubeStates.dashing);
                //PlayGroundController.Inst.TestPoint(finalDashPos);    // temp
            }
        }

        void Update()
        {
            UpdateState();
            
        }

        private void MoveAndRotate(bool isRotateNeeded = true)
        {
            if (isRotateNeeded)
            {
                var direction = _currentDestination - transform.position;
                var needRotate = Quaternion.LookRotation(direction);
                var currentRotate = Quaternion.Lerp(Quaternion.Euler(transform.eulerAngles), Quaternion.Euler(needRotate.eulerAngles), _currentRotationSpeed * Time.deltaTime);
                transform.eulerAngles = currentRotate.eulerAngles;
            }
            transform.position += transform.forward.normalized * _currentSpeed * Time.deltaTime;
        }

        private void CalculateRotationSpeedByDistance()
        {
            float theorDis = 15;
            float dis = Vector3.Distance(transform.position, _currentDestination); 
            if (dis > theorDis)
            {
                _currentRotationSpeed = _normalRotationSpeed;
            }
            else
            {
                float factor = dis / theorDis;
                _currentRotationSpeed = Mathf.Lerp(_maxRotationSpeed, _normalRotationSpeed, factor * factor);
            }
        }

        public void SetDestroyingState()
        {
            SetState(CubeStates.destroying);
        }

        private void SetState(CubeStates newState)
        {
            if (_currentState == newState)
            {
                return;
            }

            switch (newState)
            {
                case CubeStates.moving:
                    _currentSpeed = _normalSpeed;
                    //PlayGroundController.Inst.TestPoint(_currentDestination, 20);               //  temp
                    float randomDashInterval = Random.Range(_dashDeltaMinTime, _dashDeltaMaxTime);
                    _dashCountdownTimer.Setup(randomDashInterval, TryDash);
                    break;
                case CubeStates.dashing:
                    _currentSpeed = _dashSpeed;
                    _dashCountdownTimer.Setup(_dashTime, () => {
                        SetState(CubeStates.moving);
                    });
                    break;
                case CubeStates.destroying:
                    break;
                case CubeStates.searchingDestination:
                    break;
                case CubeStates.none:
                default:
                    break;
            }
            _currentState = newState;
        }

        public CubeStates GetCubeState()
        {
            return _currentState;
        }

        private void UpdateState()
        {
            switch (_currentState)
            {
                case CubeStates.moving:
                    CalculateRotationSpeedByDistance();
                    MoveAndRotate();
                    CheckDistanceToTarget();
                    _dashCountdownTimer.Countdown();
                    break;
                case CubeStates.dashing:
                    MoveAndRotate(false);
                    _dashCountdownTimer.Countdown();
                    break;
                case CubeStates.destroying:
                    Destruction();
                    break;
                case CubeStates.searchingDestination:
                    SetDestination();
                    break;
                case CubeStates.none:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void CheckDistanceToTarget()
        {
            if (Vector3.Distance(transform.position, _currentDestination) < _distance)
            {
                transform.position = _currentDestination;
                SetState(CubeStates.searchingDestination);
            }
        }

        private void Destruction()
        {
            _cubeController.RecreateCube(this);
            Destroy(gameObject);                //  bear
        }
    }
}
