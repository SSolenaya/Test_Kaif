using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Assets.Scripts
{
    public class CubeController : MonoBehaviour
    {
        public static CubeController Inst;

        [SerializeField] private MovingCube _cubePrefab;
        [SerializeField] private Transform rootCube;

        private List<MovingCube> _cubesList = new List<MovingCube>();
        private List<CountdownTimer> _timersList = new List<CountdownTimer>();

        private Action<int> _onCubeDestroyCount;
        private int _destroyedCubesCounter;

        public int DestroyedCubesCounter
        {
            get => _destroyedCubesCounter;
            set
            {
                _destroyedCubesCounter = value;
                _onCubeDestroyCount?.Invoke(_destroyedCubesCounter);
            }
        }

        private void Awake()
        {
            if (Inst == null)
            {
                Inst = this;
            }
        }

        private void Start()
        {
            for (int i = 0; i < MainController.Inst.globalParams.cubeAmount; i++)
            {
                CreateCube();
            }
        }

        private void Update()
        {
            TimersUpdate();
        }

        public void CheckDestroyingCubes(Vector3 center, float rad)
        {
            foreach (MovingCube cube in _cubesList)
            {
                if (Vector3.Distance(cube.transform.position, center) < rad)
                {
                    cube.SetDestroyingState();
                }
            }
        }

        public void SubscribeForDestroyedCubesCount(Action<int> act)
        {
            _onCubeDestroyCount += act;
        }

        private void TimersUpdate()
        {
            if (_timersList.Count == 0)
            {
                return;
            }

            foreach (CountdownTimer timer in _timersList)
            {
                timer.Countdown();
            }
        }

        private void CreateCube()
        {
            MovingCube newCube = PoolManager.Inst.GetCubeFromPull(_cubePrefab);
            newCube.transform.SetParent(rootCube);
            Vector3 planePos = PlayGroundController.Inst.GetRandomPoint();
            newCube.transform.position = planePos;
            newCube.gameObject.SetActive(true);
            _cubesList.Add(newCube);
        }

        public void RecreateCube(MovingCube oldCube)
        {
            DestroyedCubesCounter++;
            _cubesList.Remove(oldCube);
            float t = Random.Range(MainController.Inst.globalParams.minRecreatingCubeTime, MainController.Inst.globalParams.maxRecreatingCubeTime);
            CountdownTimer timer = new CountdownTimer();
            timer.Setup(t, CreateCube);
            _timersList.Add(timer);
        }
    }
}