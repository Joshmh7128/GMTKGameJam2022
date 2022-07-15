using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InverseKinematicAnimationController : MonoBehaviour
{
    // our animator
    [SerializeField] Animator animator; 
    // all our IK targets 
    [SerializeField] Transform rightHandIKTarget, leftHandIKTarget, rightFootIKTarget, leftFootIKTarget, lookIKTarget;

    // run IK targeting
    //a callback for calculating IK
    void OnAnimatorIK()
    {
        if (animator)
        {
            // Set the look target position, if one has been assigned
            if (lookIKTarget != null)
            {
                animator.SetLookAtWeight(1);
                animator.SetLookAtPosition(lookIKTarget.position);
            }

            // Set the right hand target position and rotation, if one has been assigned
            if (rightHandIKTarget != null)
            {
                animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 1);
                animator.SetIKRotationWeight(AvatarIKGoal.RightHand, 1);
                animator.SetIKPosition(AvatarIKGoal.RightHand, rightHandIKTarget.position);
                animator.SetIKRotation(AvatarIKGoal.RightHand, rightHandIKTarget.rotation);
            }

            // Set the left hand target position and rotation, if one has been assigned
            if (leftHandIKTarget != null)
            {
                animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1);
                animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, 1);
                animator.SetIKPosition(AvatarIKGoal.LeftHand, leftHandIKTarget.position);
                animator.SetIKRotation(AvatarIKGoal.LeftHand, leftHandIKTarget.rotation);
            }

            // Set the right foot target position and rotation, if one has been assigned
            if (rightFootIKTarget != null)
            {
                animator.SetIKPositionWeight(AvatarIKGoal.RightFoot, 1);
                animator.SetIKRotationWeight(AvatarIKGoal.RightFoot, 1);
                animator.SetIKPosition(AvatarIKGoal.RightFoot, rightFootIKTarget.position);
                animator.SetIKRotation(AvatarIKGoal.RightFoot, rightFootIKTarget.rotation);
            }
            // Set the left foot target position and rotation, if one has been assigned
            if (leftFootIKTarget != null)
            {
                animator.SetIKPositionWeight(AvatarIKGoal.LeftFoot, 1);
                animator.SetIKRotationWeight(AvatarIKGoal.LeftFoot, 1);
                animator.SetIKPosition(AvatarIKGoal.LeftFoot, leftFootIKTarget.position);
                animator.SetIKRotation(AvatarIKGoal.LeftFoot, leftFootIKTarget.rotation);
            }
            
        }
    }
}
