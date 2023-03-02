using System.Collections.Generic;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

namespace Assets.Scripts {
    public class CubeController : MonoBehaviour
    {
        [SerializeField] public PlayGround playGround;
        [SerializeField] private MovingCube _cubeGO;    //  temp
        [SerializeField] private List<MovingCube> _cubesList = new List<MovingCube>();
        [SerializeField] private Transform rootCube;
        [SerializeField] private int _cubeAmount = 3;                                       //  TODO: move to SO

        public void Start()
        {
            for (int i = 0; i < 3; i++)
            {
                CreateCube();
            }
        }

        [ContextMenu("CreateCube")]
        public void CreateCube()
        {
            var newCube = Instantiate(_cubeGO);
            newCube.transform.SetParent(rootCube);
            Vector3 planePos = playGround.GetRandomPoint();
            newCube.transform.localPosition = planePos;
            newCube.SetCubeController(this);
            _cubesList.Add(newCube);
        }
    }
}
