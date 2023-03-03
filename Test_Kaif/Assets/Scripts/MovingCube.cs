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
        private CubeController _cubeController;
        private float _currentSpeed;
        private float _currentRotationSpeed;
        private Vector3 _currentDestination;
        private CountdownTimer _dashCountdownTimer = new CountdownTimer();
        

        public void OnEnable()
        {
            SetState(CubeStates.searchingDestination);
        }

        public void Reset()
        {
            _currentState = CubeStates.none;
            _currentSpeed = 0;
            _currentRotationSpeed = 0;
            _currentDestination = Vector3.zero;
            _dashCountdownTimer.Reset();
        }

        public void Setup(CubeController cc)
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
            } while (Vector3.Distance(transform.position, tempVec) <= MainController.Inst.globalParams.cubeMinRadiusForNewPoint);

            if (!flag)
            {
                _currentDestination = tempVec;
                SetState(CubeStates.moving);
            }
        }

        [ContextMenu("TestDash")]
        public void TryDash()
        {
            Vector3 finalDashPos = transform.position + transform.forward.normalized 
                                   * MainController.Inst.globalParams.cubeDashSpeed
                                   * MainController.Inst.globalParams.cubeDashTime;
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
                var currentRotate = Quaternion.Lerp(Quaternion.Euler(transform.eulerAngles), 
                    Quaternion.Euler(needRotate.eulerAngles), _currentRotationSpeed * Time.deltaTime);
                transform.eulerAngles = currentRotate.eulerAngles;
            }
            transform.position += transform.forward.normalized * _currentSpeed * Time.deltaTime;
        }

        private void CalculateRotationSpeedByDistance()
        {
            
            float dis = Vector3.Distance(transform.position, _currentDestination); 
            if (dis > MainController.Inst.globalParams.cubeTheorDisToCalculateRotation)
            {
                _currentRotationSpeed = MainController.Inst.globalParams.cubeNormalRotationSpeed;
            }
            else
            {
                float factor = dis / MainController.Inst.globalParams.cubeTheorDisToCalculateRotation;
                _currentRotationSpeed = Mathf.Lerp(MainController.Inst.globalParams.cubeMaxRotationSpeed, MainController.Inst.globalParams.cubeNormalRotationSpeed, factor * factor);
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
                    _currentSpeed = MainController.Inst.globalParams.cubeNormalSpeed;
                    float randomDashInterval = Random.Range(MainController.Inst.globalParams.cubeDashDeltaMinTime, MainController.Inst.globalParams.cubeDashDeltaMaxTime);
                    _dashCountdownTimer.Setup(randomDashInterval, TryDash);
                    break;
                case CubeStates.dashing:
                    _currentSpeed = MainController.Inst.globalParams.cubeDashSpeed;
                    _dashCountdownTimer.Setup(MainController.Inst.globalParams.cubeDashTime, () => {
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
            if (Vector3.Distance(transform.position, _currentDestination) < MainController.Inst.globalParams.cubeDistance)
            {
                transform.position = _currentDestination;
                SetState(CubeStates.searchingDestination);
            }
        }

        private void Destruction()
        {
            _cubeController.RecreateCube(this);
            Reset();
            PoolManager.Inst.PutCubeToPool(this);
        }
    }
}
