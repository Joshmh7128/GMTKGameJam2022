using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.UI;

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
		[SerializeField] private Text ammoCounter;

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
		float playerJumpVelocity, verticalVelocity, gravity; // our current jump momentum
		[SerializeField] float jumpVelocity, gravityValue; // the force applied if/when we jump, and the gravity
		bool landed; // have we landed?


		#endregion

		#region Lookin' Around

		// DO STUFF HERE

		#endregion

		#region Camera
		[Header("Camera Tuning")]
		[SerializeField] private Vector3 cameraOffset = new Vector3(0, 12, -8.25f);
		[SerializeField, Range(0f, 1f)] private float cameraInputWeight = 0.5f;
		[SerializeField, Range(5, 15)] private float cameraControllerWeightMultiplier = 10f;
		[SerializeField, Range(1, 2)] private float cameraControllerWidthMultiplier = 1.5f;
		[SerializeField, Range(0f, 0.5f)] private float cameraDeadZoneLeft = 0.15f, cameraDeadZoneRight = 0.15f, cameraDeadZoneTop = 0.15f, cameraDeadZoneBottom = 0.15f; 
		#endregion
		
		#endregion

		public static PlayerCharacterController instance;

		#region Methods

		#region Input Methods

		Vector2 i_Move;
		Vector2 i_Look;
		bool i_Jump;
		bool i_Sprint;
		bool i_Attack;

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
			if (context.started && false)
			{
				Attack();
			}
			i_Attack = context.ReadValue<float>() > 0;
		}

		#endregion

		#region Initiation Methods

		private void Awake()
		{
			instance = this;
		}

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

			HandleCameraMovement();

			if (true && i_Attack) {
				Attack();
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
				
				// check to see if we are on the ground
				RaycastHit groundedHit;
				Physics.Raycast(transform.position, Vector3.down, out groundedHit, 1.5f, Physics.AllLayers, QueryTriggerInteraction.Ignore);
				
				// if we are in the air, apply gravity
				if (groundedHit.transform == null)
				{
					playerJumpVelocity += gravityValue * Time.deltaTime;
					landed = false;
				}
				else if (groundedHit.transform != null)
				{
					// jumping
					if (Input.GetKeyDown(KeyCode.Space))
					{
						playerJumpVelocity = Mathf.Sqrt(jumpVelocity * -3.0f * gravity);
					}
					else if (!landed)
					{
						playerJumpVelocity = 0f;
						landed = true;
					}
				}

				targetVelocity.y = playerJumpVelocity;

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

		private void HandleCameraMovement()
		{
			Vector3 targetPosition = transform.position;
			if (input.currentControlScheme == "Gamepad")
			{
				targetPosition = transform.position + new Vector3(i_Look.x * cameraControllerWeightMultiplier * cameraControllerWidthMultiplier, 0, i_Look.y * cameraControllerWeightMultiplier);
			}
			else if (input.currentControlScheme == "Keyboard&Mouse")
			{
				Plane plane = new Plane(Vector3.up, transform.position);
				Vector3 clampedMousePos = new Vector3(
            		Mathf.Clamp(i_Look.x, Screen.width * cameraDeadZoneLeft, Screen.width * (1 - cameraDeadZoneRight)),
            		Mathf.Clamp(i_Look.y, Screen.height * cameraDeadZoneBottom, Screen.height * (1 - cameraDeadZoneTop))
            );
				Ray ray = Camera.main.ScreenPointToRay(clampedMousePos);
				float dist;
				if (plane.Raycast(ray, out dist))
				{
					targetPosition = ray.GetPoint(dist);
				}
			}
			targetPosition = Vector3.Lerp(transform.position, targetPosition, cameraInputWeight);
			playerCamera.transform.position = Vector3.Slerp(playerCamera.transform.position, targetPosition + cameraOffset, 0.05f);
		}

		#region Weapon Methods

		GameObject activeWeapon;
		WeaponClass _activeWeapon;

		public void SwitchWeapon(GameObject weapon)
		{
			if (activeWeapon) {
				Destroy(activeWeapon);
			}

			activeWeapon = Instantiate(weapon, pointerThing.transform);
			_activeWeapon = activeWeapon.GetComponent<WeaponClass>();
			ammoCounter.text = $"{_activeWeapon.Uses}";
		}

		private void Attack()
		{
			if (_activeWeapon) {
				_activeWeapon.Attack();
				ammoCounter.text = $"{_activeWeapon.Uses}";
			}
		}

		#endregion

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