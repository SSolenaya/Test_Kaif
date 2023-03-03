using System.Collections;
using System.Collections.Generic;
using Assets.Scripts;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    public static PoolManager Inst;

    private Stack<Ball> _ballStack;
    private Stack<MovingCube> _cubeStack;
    private Transform _parentForDeactivatedGO;

    public Transform parentGO;

    private void Awake()
    {
        if (Inst == null)
        {
            Inst = this;
        }
    }

    private void Start()
    {
        _parentForDeactivatedGO = parentGO;
        _ballStack = new Stack<Ball>();
        _cubeStack = new Stack<MovingCube>();
    }

    public Ball GetBallFromPull(Ball ballPrefab)
    {
        Ball result;
        if (_ballStack.Count > 0)
        {
            result = _ballStack.Pop();
            return result;
        }
        result = Instantiate(ballPrefab, _parentForDeactivatedGO);
        result.name = ballPrefab.name;
        return result;
    }

    public MovingCube GetCubeFromPull(MovingCube cubePrefab)
    {
        MovingCube result;
        if (_cubeStack.Count > 0)
        {
            result = _cubeStack.Pop();
            return result;
        }
        result = Instantiate(cubePrefab, _parentForDeactivatedGO);
        result.name = cubePrefab.name;
        return result;
    }

    public void PutBallToPool(Ball target)
    {
        target.transform.SetParent(_parentForDeactivatedGO);
        target.gameObject.SetActive(false);
        _ballStack.Push(target);
    }

    public void PutCubeToPool(MovingCube target)
    {
        target.transform.SetParent(_parentForDeactivatedGO);
        target.gameObject.SetActive(false);
        _cubeStack.Push(target);
    }
}

