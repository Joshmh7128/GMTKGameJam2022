using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestMove : MonoBehaviour
{
    public CharacterController controller;
    public Transform cam;

    public float moveSpeed = 6;

    public float turnSmoothTime = 0.1f;
    float turnSmoothVelocity;

    Vector3 velocity;

    public static bool worldForward;

    public int health;

    void Update()
    {

        if (!controller.isGrounded)
        {
            velocity.y = -2f;
            controller.Move(velocity * Time.deltaTime);
        }
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        if (direction.magnitude >= 0.1f)
        {

            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            if (!worldForward) controller.Move(moveDir.normalized * moveSpeed * Time.deltaTime);
            else controller.Move(direction * moveSpeed * Time.deltaTime);


        }

    }


    public void TakeDamage(int damage)
    {
        health -= damage;
    }
}
