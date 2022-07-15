using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCameraController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform playerTransform;
    [Header("Tuning")]
    [SerializeField] private Vector3 offset;
    [SerializeField, Range(0f, 1f)] private float mouseWeight = 0.5f;
    [SerializeField, Range(0f, 0.5f)] private float deadZoneLeft, deadZoneRight, deadZoneTop, deadZoneBottom = 0.15f; 

    void Update()
    {
        Vector3 targetPosition = Vector3.Lerp(playerTransform.position, GetMousePosition(), mouseWeight);
        transform.position = targetPosition + offset;
    }

    private Vector3 GetMousePosition()
    {
        Plane plane = new Plane(Vector3.up, playerTransform.position);
        Vector3 clampedMousePos = new Vector3(
            Mathf.Clamp(Input.mousePosition.x, Screen.width * deadZoneLeft, Screen.width * (1 - deadZoneRight)),
            Mathf.Clamp(Input.mousePosition.y, Screen.height * deadZoneBottom, Screen.height * (1 - deadZoneTop))
            );
        Ray ray = Camera.main.ScreenPointToRay(clampedMousePos);
        float dist;

        if (plane.Raycast(ray, out dist))
            return ray.GetPoint(dist);
        else
            return Vector3.zero;
    }
}
