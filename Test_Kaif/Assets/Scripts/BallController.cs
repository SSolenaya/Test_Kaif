using System.Collections;
using System.Collections.Generic;
using Assets.Scripts;
using UnityEngine;

public class BallController : MonoBehaviour
{
    [SerializeField] private RaycastController _raycastController;
    [SerializeField] private Ball _ball;
    [SerializeField] private Transform _spawnPoint;         //  ball spawning point

    void Start()
    {
        _raycastController.SubscribeForFindingTarget(ShootBall);
    }

    private void ShootBall(RaycastHit rh)
    {
        var b = Instantiate(_ball, _spawnPoint);

    }

    void Update()
    {
        
    }
}
