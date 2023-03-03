using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using Assets.Scripts;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

public class BallController : MonoBehaviour
{
    [SerializeField] private RaycastController _raycastController;
    [SerializeField] private Ball _ball;
    [SerializeField] private Transform _spawnPoint;         //  ball spawning point
    [SerializeField] private float _radiusOfBlast = 60f;    //  TODO: move to SO or Config

    void Start()
    {
        _raycastController.SubscribeForFindingTarget(ShootBall);
    }

    private void ShootBall(RaycastHit raycastHit)
    {
        var ball = Instantiate(_ball, _spawnPoint);
        Vector3 endPos = raycastHit.point;
        ball.SetDestination(_spawnPoint.position, endPos, () => CubeController.Inst.CheckDestroyingCubes(endPos, _radiusOfBlast));
    }

    void Update()
    {
        
    }
}
