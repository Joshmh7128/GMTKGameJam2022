using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScreenshake : MonoBehaviour
{
    Vector3 home, targetPos; // our home position and movement position

    float _intensity, _velocity, _velocityDelta, _interval, _length; // all our current variables

    public static CameraScreenshake CameraInstance; // instance for scene reference

    private void Awake()
    {
        CameraInstance = this;
    }

    // Update is called once per frame
    void Update()
    {
        ProcessShake();
    }

    // trigger a new screenshake
    public void ScreenShake(float intensity, float velocity, float velocityDelta, float interval, float length)
    {
        // intensity = how hard we shake
        _intensity = intensity;
        // velocity = how quickly we shake
        _velocity = velocity;
        _velocityDelta = velocityDelta;
        // interval = how quickly we move back and forth
        _interval = interval;
        // length = how long the shake lasts
        _length = length;

        // run the function

    }

    // move the camera around
    void ProcessShake()
    {
        // lerp at our intensity
        Vector3.Lerp(transform.position, targetPos, currentVelocity * Time.deltaTime);
        // lower our velocity by our delta
    }
}
