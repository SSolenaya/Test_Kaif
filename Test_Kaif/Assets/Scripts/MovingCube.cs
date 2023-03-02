using UnityEngine;

namespace Assets.Scripts {
    public class MovingCube : MonoBehaviour
    {
        [SerializeField] private float _speed = 1f;
        [SerializeField] private Vector3 _currentDestination;
        private float _distance = 0.5f;
        private CubeController _cubeController;
        private bool _canMove = false;
        

        void Start()
        {
            SetDestination();
        }

        public void SetCubeController(CubeController cc)
        {
            _cubeController = cc;
        }

        public void SetDestination()
        {
            _currentDestination = _cubeController.playGround.GetRandomPoint();
            transform.LookAt(_currentDestination);                                          // temp
            _canMove = true;
        }

        void Update()
        {
            if (_canMove)
            {
                Movement();
                if (Vector3.Distance(transform.position, _currentDestination) < _distance)
                {
                    _canMove = false;
                    transform.position = _currentDestination;
                    SetDestination();
                }
            }
        }

        public void Movement()
        {
            transform.position += transform.forward.normalized * Time.deltaTime * _speed;
            transform.LookAt(_currentDestination);
            //Vector3 targetDirection = _currentDestination - transform.position;
            //float singleStep = _speed * Time.deltaTime;
            //Vector3 newDirection = Vector3.RotateTowards(transform.forward, targetDirection, singleStep, 0.0f);
            //Debug.DrawRay(transform.position, newDirection, Color.red);
            //transform.rotation = Quaternion.LookRotation(newDirection);
        }
    }
}
