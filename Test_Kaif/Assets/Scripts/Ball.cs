using System;
using UnityEngine;

public class Ball : MonoBehaviour
{
    [SerializeField] private Transform _ballView;
    private Vector3 _startPosition;
    private Vector3 _endPosition;
    private Vector3 _helpingPos = Vector3.zero;
    private Action _actOnLanding;
    
    public void SetDestination(Vector3 startPos, Vector3 endPos, Action actOnLanding)
    {
        transform.position = startPos;
        _startPosition = startPos;
        _endPosition = endPos;
        _actOnLanding += actOnLanding;
        gameObject.SetActive(true);
    }

    public void Reset()
    {
        _helpingPos = Vector3.zero;
        _ballView.localPosition = _helpingPos;
        _startPosition = Vector3.zero;
        _endPosition = Vector3.zero;
        _actOnLanding = null;
    }

    private void Update()
    {
        Movement();
        CheckForBlast();
    }

    private void Movement()
    {
        transform.position = Vector3.MoveTowards(transform.position, _endPosition, Time.deltaTime * MainController.Inst.globalParams.ballSpeed);
        CalculateViewBall(_startPosition, _endPosition, transform.position);
    }

    private void CheckForBlast()
    {
        if (Vector3.Distance(transform.position, _endPosition) <= 0.5f)
        {
            _actOnLanding.Invoke();
            Reset();
            PoolManager.Inst.PutBallToPool(this);
        }
    }

    private void CalculateViewBall(Vector3 startPos, Vector3 endPos, Vector3 currentPos)
    {
        float fullDistance = Vector3.Distance(startPos, endPos);
        float currentDistance = Vector3.Distance(currentPos, startPos);
        float factor = currentDistance / fullDistance;
        _helpingPos.y = MainController.Inst.globalParams.ballAmplitude * (factor < 0.5f ? EaseOutQuart(factor) : 1 - EaseInQuart(factor));
        _ballView.localPosition = _helpingPos;
    }

    private float EaseOutQuart(float x)
    {
        double d = x;
        return (float) (1 - Math.Pow(1 - d, 4));
    }

    private float EaseInQuart(float x)
    {
        return x * x * x * x;
    }
}