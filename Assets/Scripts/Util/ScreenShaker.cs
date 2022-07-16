using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenShaker : MonoBehaviour
{
    [Header("Intensity"), Tooltip("How far the camera moves each interval"), SerializeField]
    float _intensity;
    [Header("Velocity"), Tooltip("How quickly the camera moves towards its shake goal"), SerializeField]
    float _velocity;
    [Header("Velocity Delta"), Tooltip("How quickly the velocity decreases each interval"), SerializeField]
    float _velocityDelta;
    [Header("Interval"), Tooltip("How quickly the camera changes position"), SerializeField]
    float _interval;
    [Header("Length"), Tooltip("How long the camera shakes for"), SerializeField]
    float _length;

    // Start is called before the first frame update
    void Start()
    {
        CameraScreenshake.CameraInstance.ScreenShake(_intensity, _velocity, _velocityDelta, _interval, _length);
    }
}
