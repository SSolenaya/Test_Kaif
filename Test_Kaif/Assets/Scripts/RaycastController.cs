using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts;
using UnityEngine;

public class RaycastController : MonoBehaviour
{
    [SerializeField] private ClickObserver _clickObserver;
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
        }
    }

    public void SubscribeForFindingTarget(Action<RaycastHit> act)
    {
        _onFindingTarget += act;
    }
}
