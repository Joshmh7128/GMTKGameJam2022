using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace Dice.Player
{
	[RequireComponent(typeof(CharacterController))]
	public class PlayerCharacterController : MonoBehaviour
	{
		#region Variables n' Stuff

		#region References

		[Header("References"), Tooltip("Main camera used by the player."), SerializeField]
		Camera playerCamera;

		// Character controller used to move the player.
		CharacterController controller;

		[SerializeField]
		GameObject pointerThing;

		[SerializeField]
		PlayerInput input;

		[SerializeField]
		WeaponClass currentWeapon; 

		#endregion

		#region General

		/*[Header("General"), Tooltip("Physics layers used to check grounded state."), SerializeField]
		LayerMask groundCheckLayers = -1;

		[Tooltip("Distance used to check grounded state."), SerializeField]
		float groundCheckDistance = 0.05f;

		// Normal vector of the ground the player is standing on.
		Vector3 groundNormal;*/

		#endregion

		#region Movement

		[Header("Movement"), Tooltip("Maximum movement speed on the ground."), SerializeField]
		float maxSpeedOnGround = 10f;

		[Tooltip("Sharpness for grounded movement.\nHigh value increases acceleration speed."), SerializeField]
		float movementSharpnessOnGround = 15;
				
		// Current velocity of the player character.
		Vector3 characterVelocity;
		
		#endregion

		#region Lookin' Around

		// DO STUFF HERE

		#endregion
		
		#endregion

		#region Methods

		#region Input Methods

		Vector2 i_Move;
		Vector2 i_Look;
		bool i_Jump;
		bool i_Sprint;

		public void OnMove(InputAction.CallbackContext context)
		{
			i_Move = context.ReadValue<Vector2>();
		}

		public void OnLook(InputAction.CallbackContext context)
		{
			i_Look = context.ReadValue<Vector2>();
		}

		public void OnAttack(InputAction.CallbackContext context)
		{
			if (context.started)
			{
				Attack();
			}
		}

		#endregion

		#region Initiation Methods

		private void Start()
		{
			// Fetch us some components.
			controller = GetComponent<CharacterController>();

			// Honestly can't remember why this is here.
			controller.enableOverlapRecovery = true;

			// Hide and lock our mouse. Standard stuff.
			//Cursor.visible = false; Cursor.lockState = CursorLockMode.Locked;
		}

		#endregion

		private void Update()
		{
			HandleCharacterMovement();

			if (input.currentControlScheme == "Gamepad" && i_Look.magnitude > 0.25f)
			{
				Vector3 direction = new Vector3(i_Look.x, 0, i_Look.y);
				Quaternion rotation = Quaternion.LookRotation(direction, Vector3.up);
				pointerThing.transform.rotation = rotation;
			}
			else if (input.currentControlScheme == "Keyboard&Mouse")
			{
				Plane plane = new Plane(Vector3.up, transform.position);
				Ray ray = Camera.main.ScreenPointToRay(i_Look);
				float dist;
				if (plane.Raycast(ray, out dist))
				{
					pointerThing.transform.LookAt(ray.GetPoint(dist));
				}
			}
		}

		private void HandleCharacterMovement()
		{
			// Character movement handling.
			{
				bool isSprinting = i_Sprint;

				// Do some shit with this if you put in a sprint speed. |||TODO|||
				float speedModifier = 1f;
				
				// Converts move input to a worldspace vector based on our character's transform orientation.
				Vector3 worldspaceMoveInput = transform.TransformVector(new Vector3(i_Move.x, 0, i_Move.y));

				// Handle grounded movement.
				// Calculate the desired velocity from inputs, max speed, and current slope.
				Vector3 targetVelocity = worldspaceMoveInput * maxSpeedOnGround * speedModifier;

				//targetVelocity = GetDirectionReorientedOnSlope(targetVelocity.normalized, groundNormal) * targetVelocity.magnitude;

				// Smoothly interpolate between our current velocity and the target velocity based on acceleration speed.
				characterVelocity = Vector3.Lerp(characterVelocity, targetVelocity, movementSharpnessOnGround * Time.deltaTime);

				// Play footsteps sound.
				// |||TODO|||
			}

			// Apply the final calculated velocity value as a character movement.
			Vector3 capsuleBottomBeforeMove = GetCapsuleBottomHemisphere();
			Vector3 capsuleTopBeforeMove = GetCapsuleTopHemisphere(controller.height);
			controller.Move(characterVelocity * Time.deltaTime);
			
			if (Physics.CapsuleCast(capsuleBottomBeforeMove, capsuleTopBeforeMove, controller.radius,
				characterVelocity.normalized, out RaycastHit hit, characterVelocity.magnitude * Time.deltaTime, -1,
				QueryTriggerInteraction.Ignore))
			{
				characterVelocity = Vector3.ProjectOnPlane(characterVelocity, hit.normal);
			}
		}

		private void Attack()
		{
			// Do some shit here.
		}

		#region Miscellaneous Methods

		bool IsNormalUnderSlopeLimit(Vector3 normal)
		{
			return Vector3.Angle(transform.up, normal) <= controller.slopeLimit;
		}

		Vector3 GetCapsuleBottomHemisphere()
		{
			return transform.position + (transform.up * controller.radius);
		}

		Vector3 GetCapsuleTopHemisphere(float atHeight)
		{
			return transform.position + (transform.up * (atHeight - controller.radius));
		}

		Vector3 GetDirectionReorientedOnSlope(Vector3 direction, Vector3 slopeNormal)
		{
			Vector3 directionRight = Vector3.Cross(direction, transform.up);
			return Vector3.Cross(slopeNormal, directionRight).normalized;
		}

		#endregion

		#endregion
	}
}