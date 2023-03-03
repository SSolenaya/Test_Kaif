using System;
using UnityEngine;

public class Ball : MonoBehaviour
{
    [SerializeField] private Transform _ballView;
    private float _speed = 180f;
    private Vector3 _startPosition;
    private Vector3 _endPosition;
    private Vector3 _helpingPos = Vector3.zero;
    private readonly float _amplitude = 120f;
    private Action _actOnLanding;


    public void SetDestination(Vector3 startPos, Vector3 endPos, Action actOnLanding)
    {
        _startPosition = startPos;
        _endPosition = endPos;
        _actOnLanding += actOnLanding;
    }

    private void Update()
    {
        Movement();
        CheckForBlast();
    }

    private void Movement()
    {
        transform.position = Vector3.MoveTowards(transform.position, _endPosition, Time.deltaTime * _speed);
        CalculateViewBall(_startPosition, _endPosition, transform.position);
    }

    private void CheckForBlast()
    {
        if (Vector3.Distance(transform.position, _endPosition) <= 0.5f)
        {
            _actOnLanding.Invoke();
            Destroy(gameObject);                //  TODO: pool
        }
    }

    private void CalculateViewBall(Vector3 startPos, Vector3 endPos, Vector3 currentPos)
    {
        float fullDistance = Vector3.Distance(startPos, endPos);
        float currentDistance = Vector3.Distance(currentPos, startPos);
        float factor = currentDistance / fullDistance;
        _helpingPos.y = _amplitude * (factor < 0.5f ? EaseOutQuart(factor) : 1- EaseInQuart(factor));
        _ballView.localPosition = _helpingPos;
    }

    private float EaseOutQuart(float x)
    {
        double d = (double) x;
        return (float) (1 - Math.Pow(1 - d, 4));
    }

    private float EaseInQuart(float x)
    {
        return x * x * x * x;
    }
}