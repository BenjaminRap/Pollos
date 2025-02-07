using Unity.VisualScripting;
using UnityEngine;

public class Level : MonoBehaviour
{
    private const float rotationDuration = 0.5f;
    private const float rotationAngle = 90.0f;

    private static Level    _instance;

    private void    Start()
    {
        if (_instance != null)
        {
            Debug.LogError("Multiples instance of the level class !");
            Destroy(this);
            return ;
        }
        _instance = this;
    }
    
    private void    OnDestroy()
    {
        _instance = null;
    }
    
    public static Level    GetInstance()
    {
        return (_instance);
    }
    
    public void     Rotate(float axisValue)
    {
        StartCoroutine(TransformUtils.RotateAroundInTime(transform, Vector3.forward, axisValue * rotationAngle, rotationDuration));
    }
}
