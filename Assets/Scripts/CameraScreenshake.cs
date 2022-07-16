using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScreenshake : MonoBehaviour
{
    [SerializeField] Vector3 home, targetPos; // our home position and movement position

    [SerializeField] float _intensity, _velocity, _velocityDelta, _interval, _length; // all our current variables

    public static CameraScreenshake CameraInstance; // instance for scene reference

    private void Awake()
    {
        CameraInstance = this;
    }

    // Update is called once per frame
    void Update()
    {
        // process the shake
        ProcessShake();

        /// an example screenshare
        // ScreenShake(2, 2, 0.01f, 0.03f, 0.1f);
    }

    // trigger a new screenshake
    public void ScreenShake(float intensity, float velocity, float velocityDelta, float interval, float length)
    {
        Debug.Log("shaking");
        // intensity = how hard we shake
        _intensity = intensity;
        // velocity = how quickly we shake
        _velocity = velocity;
        _velocityDelta = velocityDelta;
        // interval = how quickly we move back and forth
        _interval = interval;
        // length = how long the shake lasts
        _length = length;
        // run the functions
        StartCoroutine(MoveTarget());
    }

    IEnumerator MoveTarget()
    {
        if (_length >= 0)
        {
            float xr = Random.Range(-_intensity, _intensity);
            float yr = Random.Range(-_intensity, _intensity);
            float zr = Random.Range(-_intensity, _intensity);
            targetPos = new Vector3(home.x + xr, home.y + yr, home.y + zr);
            Debug.Log(targetPos);
            // calculate length
            _length -= _interval;
            // loop
            yield return new WaitForSeconds(_interval);
            StartCoroutine(MoveTarget());
        } else
        {
            targetPos = home;
            yield break;
        }
    }

    // move the camera around
    void ProcessShake()
    {
        // lerp at our intensity
        transform.localPosition = Vector3.Lerp(transform.localPosition, transform.localPosition + targetPos, _velocity * Time.deltaTime);
        // lower our velocity by our delta
        if (_velocity > 0)
        {
            _velocity -= _velocityDelta;
        } 
        
        if (_velocity <= 0)
        {
            targetPos = home;
            transform.localPosition = Vector3.Lerp(transform.localPosition, home, 0.5f * Time.deltaTime);
        }
    }

    // gizmos
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(targetPos, 1f);
    }

}
