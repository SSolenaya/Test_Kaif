using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts;
using UnityEngine;

public class RaycastController : MonoBehaviour
{
    [SerializeField] private ClickObserver _clickObserver;
    [SerializeField] private GameObject _testCube;
    private Action<RaycastHit> _onFindingTarget;

    void Start()
    {
        _clickObserver.SubscribeForClick(CastRay);
    }

    private void CastRay(Vector2 clickPos)
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(clickPos);

        if (Physics.Raycast(ray, out hit))
        {
            _onFindingTarget(hit);
            Transform objectHit = hit.transform;            //  temp
            var tc = Instantiate(_testCube);                // temp
            tc.transform.SetParent(hit.transform);          // temp  
            tc.transform.position = hit.point;              // temp
        }
    }

    public void SubscribeForFindingTarget(Action<RaycastHit> act)
    {
        _onFindingTarget += act;
    }
}
