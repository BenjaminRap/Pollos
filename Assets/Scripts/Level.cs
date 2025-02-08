using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{
    private const float rotationDuration = 0.2f;
    private const float rotationAngle = 90.0f;

    private static Level    _instance;
    
    private Coroutine       _rotateCoroutine;
    private Quaternion      _rotationGoal;
    private Storm[]         _storms;

    private void    Start()
    {
        if (_instance != null)
        {
            Debug.LogError("Multiples instance of the level class !");
            Destroy(this);
            return ;
        }
        _instance = this;
        _rotationGoal = transform.rotation;
        _storms = GetComponentsInChildren<Storm>();
    }
    
    public static Level    GetInstance()
    {
        return (_instance);
    }
    
    public void     Rotate(float axisValue)
    {
        foreach (Storm storm in _storms)
        {
            storm.ComeCloser();
        }
        if (_rotateCoroutine != null)
            StopCoroutine(_rotateCoroutine);
        _rotationGoal *= Quaternion.Euler(-axisValue * rotationAngle * Vector3.forward);
        _rotateCoroutine = StartCoroutine(TransformUtils.RotateInTime(transform, _rotationGoal, rotationDuration));
    }
}
