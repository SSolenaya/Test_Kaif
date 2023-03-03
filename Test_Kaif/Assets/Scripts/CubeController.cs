using System.Collections.Generic;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

namespace Assets.Scripts {
    public class CubeController : MonoBehaviour
    {
        public static CubeController Inst;
        [SerializeField] public PlayGroundController playGroundController;
        [SerializeField] private MovingCube _cubeGO;    //  temp
        [SerializeField] private List<MovingCube> _cubesList = new List<MovingCube>();
        [SerializeField] private Transform rootCube;
        private float _minRecreatingCubeTime = 1f;                               //  TODO: move to SO or Config
        private float _maxRecreatingCubeTime = 3f;
        private int _cubeAmount = 3;                                             //  TODO: move to SO or Config
        private List<CountdownTimer> _timersList = new List<CountdownTimer>();

        void Awake()
        {
            if (Inst == null)
            {
                Inst = this;
            }
        }

        public void Start()
        {
            for (int i = 0; i < _cubeAmount; i++)
            {
                CreateCube();
            }
        }

        void Update()
        {
            TimersUpdate();
        }

        private void TimersUpdate()
        {
            if (_timersList.Count == 0)
            {
                return;
            }
            foreach (var timer in _timersList)
            {
                timer.Countdown();
            }
        }

        [ContextMenu("CreateCube")]
        public void CreateCube()
        {
            var newCube = Instantiate(_cubeGO);                //  TODO: pool
            newCube.transform.SetParent(rootCube);
            Vector3 planePos = playGroundController.GetRandomPoint();
            newCube.transform.localPosition = planePos;
            newCube.SetCubeController(this);
            _cubesList.Add(newCube);
        }

        public void RecreateCube(MovingCube oldCube)
        {
            _cubesList.Remove(oldCube);
            float t = Random.Range(_minRecreatingCubeTime, _maxRecreatingCubeTime);
            CountdownTimer timer = new CountdownTimer();
            timer.Setup(t, CreateCube);
            _timersList.Add(timer);
        }

        public void CheckDestroyingCubes(Vector3 center, float rad)
        {
            foreach (var cube in _cubesList)
            {
                if (Vector3.Distance(cube.transform.position, center) < rad)
                {
                    cube.SetDestroyingState();
                }
            }
        }
    }
}
