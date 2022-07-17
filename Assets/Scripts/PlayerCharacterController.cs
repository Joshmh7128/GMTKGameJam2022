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

		[Header("References"), Tooltip("Main camera transform used by the player."), SerializeField]
		Transform playerCamera; // hey rob I had to change this cus I want screenshare

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

		[Header("General"), Tooltip("Physics layers used to check grounded state."), SerializeField]
		LayerMask groundCheckLayers = -1;

		[Tooltip("Distance used to check grounded state."), SerializeField]
		float groundCheckDistance = 0.05f;

		// Normal vector of the ground the player is standing on.
		Vector3 groundNormal;

		#endregion

		#region Movement

		[Header("Movement"), Tooltip("Maximum movement speed on the ground."), SerializeField]
		float maxSpeedOnGround = 10f;

		[Tooltip("Sharpness for grounded movement.\nHigh value increases acceleration speed."), SerializeField]
		float movementSharpnessOnGround = 15;

		[Tooltip("Maximum movement speedwhen airborne."), SerializeField]
		float maxSpeedInAir = 10f;

		[Tooltip("Acceleration speed when in the air."), SerializeField]
		float accelerationSpeedInAir = 25f;

		// Current velocity of the player character.
		Vector3 characterVelocity;

		// Speed of the character the last time they touched the ground.
		Vector3 latestImpactSpeed;

		#endregion

		#region Jump

		[Header("Jump"), Tooltip("Maximum height of the player's jump."), SerializeField]
		float maxJumpHeight = 4;

		[Tooltip("Minimum height of the player's jump."), SerializeField]
		float minJumpHeight = 1;

		[Tooltip("Time it takes to reach the height of the player's jump."), SerializeField]
		float timeToJumpApex = 1f;

		// Acceleration of the player downwards.
		private float gravity;
		// Initial velocity needed for the character to reach the maximum jump height.
		private float maxJumpVelocity;
		// Initial velocity needed for the character to reach the minimum jump height.
		private float minJumpVelocity;

		// Is the player currently grounded?
		bool isGrounded;
		// Has the player jumped this frame?
		bool hasJumpedThisFrame;

		// Time in which the player last jumped.
		float lastTimeJumped = 0f;

		// Time spent not checking for ground, so as to not snap the player back.
		const float jumpGroundingPreventionTime = 0.2f;
		// How far should the raycast travel to find ground in the air?
		const float groundCheckDistanceInAir = 0.07f;

		#endregion

		#region Camera
		[Header("Camera Tuning")]
		[SerializeField] private Vector3 cameraOffset = new Vector3(0, 12, -8.25f);
		[SerializeField, Range(0f, 1f)] private float cameraInputWeight = 0.5f;
		[SerializeField, Range(5, 15)] private float cameraControllerWeightMultiplier = 10f;
		[SerializeField, Range(1, 2)] private float cameraControllerWidthMultiplier = 1.5f;
		[SerializeField, Range(0f, 0.5f)] private float cameraDeadZoneLeft = 0.15f, cameraDeadZoneRight = 0.15f, cameraDeadZoneTop = 0.15f, cameraDeadZoneBottom = 0.15f;
		#endregion

		#region Health
		[Header("Health"), Tooltip("How much HP the player has and currently has"), SerializeField]
		float currentHealth = 5f;
		[SerializeField]
		float maxHealth = 5f;
		[Header("Hurt Cooldown"), Tooltip("How long between you can get hurt"), SerializeField]
		float damageCooldownMax, damageCooldownCurrent;
		[SerializeField] CanvasGroup hurtCanvas, deadCanvas; // the canvas that makes the screen go red
		[SerializeField] float hurtAlphaReductionRate; // how fast the screen stops being red after being hurt
		[SerializeField] Text healthText; // the display for our health
		[SerializeField] Slider healthBar; // our healthbar
		#endregion

		#region Money
		public int currencyAmount; // how much currency the player has
		[SerializeField] Text currencyDisplayText;
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

		public void OnJump(InputAction.CallbackContext context)
		{
			i_Jump = context.ReadValue<float>() == 1;
			if (context.started)
			{
				Jump();
			}
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

			// Do some math (PHYSICS YEAAH) to get us some necessary variables.
			gravity = -(2 * maxJumpHeight) / Mathf.Pow(timeToJumpApex, 2);
			maxJumpVelocity = Mathf.Abs(gravity) * timeToJumpApex;
			minJumpVelocity = Mathf.Sqrt(2 * Mathf.Abs(gravity) * minJumpHeight);

			// Make sure ground movement ain't slower. Unless you're into that.
			maxSpeedInAir = Mathf.Min(maxSpeedInAir, maxSpeedOnGround);

			// Hide and lock our mouse. Standard stuff.
			//Cursor.visible = false; Cursor.lockState = CursorLockMode.Locked;
		}

		#endregion

		private void Update()
		{
			// Frame's just started, of course you haven't jumped yet.
			hasJumpedThisFrame = false;

			// Yesterday's "is" is today's "was".
			bool wasGrounded = isGrounded;
			// Check to see if we're solidly planted on the earth.
			GroundCheck();
			if (isGrounded && !wasGrounded) { }
			// Stuff that happens upon landing.
			if (isGrounded && !wasGrounded)
			{
				// Land SFX
			}

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
					// josh wrote this rotation code so that the pointerThing doesnt look at the ground
					Vector3 point = ray.GetPoint(dist);
					pointerThing.transform.LookAt(new Vector3(point.x, transform.position.y+1f, point.z));
				}
			}

			if (true && i_Attack) {
				Attack();
			}
		}

		private void LateUpdate()
		{
			HandleCameraMovement();
		}

		private void GroundCheck()
		{
			// Make sure ground check distance in air is small, to prevent sudden snapping to ground.
			float chosenGroundCheckDistance =
				isGrounded ? (controller.skinWidth + groundCheckDistance) : groundCheckDistanceInAir;

			// Reset values before the ground check.
			isGrounded = false;
			controller.stepOffset = 0;
			groundNormal = Vector3.up;

			// Don't detect ground right after jumping.
			if (Time.time >= lastTimeJumped + jumpGroundingPreventionTime)
			{
				// If we're grounded, collect about the ground normal with a downward capsule cast representing our character capsule.
				if (Physics.CapsuleCast(GetCapsuleBottomHemisphere(), GetCapsuleTopHemisphere(controller.height),
					controller.radius, Vector3.down, out RaycastHit hit, chosenGroundCheckDistance, groundCheckLayers,
					QueryTriggerInteraction.Ignore))
				{
					// Store the normal for the surface found.
					groundNormal = hit.normal;

					// Only consider this a valid ground hit if the ground normal goes in the same
					// direction as the character up and if the slope angle is lower than the
					// character controller's limit.
					if (Vector3.Dot(hit.normal, transform.up) > 0f &&
						IsNormalUnderSlopeLimit(groundNormal))
					{
						isGrounded = true;
						controller.stepOffset = 0.3f;

						// Handle snapping to the ground.
						if (hit.distance > controller.skinWidth)
							controller.Move(Vector3.down * hit.distance);
					}
				}
			}
		}

		private void Jump()
		{
			if (isGrounded)
			{
				// Start by cancelling out the vertical component of our velocity.
				characterVelocity = new Vector3(characterVelocity.x, 0f, characterVelocity.z);

				// Then, add the jumpSpeed value upwards.
				characterVelocity += Vector3.up * maxJumpVelocity;

				// Play sound.
				// |||TODO|||

				// Remember the last time we jumped because we need to prevent snapping to ground for a short time.
				lastTimeJumped = Time.time;
				hasJumpedThisFrame = true;

				// Force grounding to false.
				isGrounded = false;
				groundNormal = Vector3.up;
			}
		}
		
        private void FixedUpdate()
        {
			ProcessUIElements();
			// process our damge cooldown
			ProcessDamageCooldown();
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
				if (isGrounded)
				{
					// Calculate the desired velocity from inputs, max speed, and current slope.
					Vector3 targetVelocity = worldspaceMoveInput * maxSpeedOnGround * speedModifier;

					//targetVelocity = GetDirectionReorientedOnSlope(targetVelocity.normalized, groundNormal) * targetVelocity.magnitude;

					// Smoothly interpolate between our current velocity and the target velocity based on acceleration speed.
					characterVelocity = Vector3.Lerp(characterVelocity, targetVelocity, movementSharpnessOnGround * Time.deltaTime);

					// Play footsteps sound.
					// |||TODO|||
				}
				// Handle air movement.
				else
				{
					// Add air acceleration.
					characterVelocity += worldspaceMoveInput * accelerationSpeedInAir * Time.deltaTime;

					// Limit horizontal air speed to a maximum.
					float verticalVelocity = characterVelocity.y;
					Vector3 horizontalVelocity = Vector3.ProjectOnPlane(characterVelocity, Vector3.up);
					horizontalVelocity = Vector3.ClampMagnitude(horizontalVelocity, maxSpeedInAir * speedModifier);

					// Limit vertical air speed to a minimum. Limit to max if the player lets go of jump.
					verticalVelocity = Mathf.Clamp(verticalVelocity, -3 * maxJumpVelocity, !i_Jump ? minJumpVelocity : maxJumpVelocity);

					// Combine horizontal and vertical velocity.
					characterVelocity = horizontalVelocity + (Vector3.up * verticalVelocity);

					// Apply the gravity to the velocity.
					characterVelocity -= Vector3.down * gravity * Time.deltaTime;
				}
			}

			// Apply the final calculated velocity value as a character movement.
			Vector3 capsuleBottomBeforeMove = GetCapsuleBottomHemisphere();
			Vector3 capsuleTopBeforeMove = GetCapsuleTopHemisphere(controller.height);
			controller.Move(characterVelocity * Time.deltaTime);

			// Detect obstructions to adjust velocity accordingly.
			latestImpactSpeed = Vector3.zero;
			if (Physics.CapsuleCast(capsuleBottomBeforeMove, capsuleTopBeforeMove, controller.radius,
				characterVelocity.normalized, out RaycastHit hit, characterVelocity.magnitude * Time.deltaTime, -1,
				QueryTriggerInteraction.Ignore))
			{
				// We remember the last impact speed because the fall damage logic might need it.
				latestImpactSpeed = characterVelocity;

				characterVelocity = Vector3.ProjectOnPlane(characterVelocity, hit.normal);
			}
		}

		private void HandleCameraMovement()
		{
			Vector3 targetPosition = transform.position;
			//playerCamera.position = targetPosition + cameraOffset; return;
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
			playerCamera.position = Vector3.Slerp(playerCamera.position, targetPosition + cameraOffset, 0.05f);
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

		void ProcessUIElements()
        {
			// make sure our screne stops being red after we get hurt
			if (hurtCanvas.alpha >= 0)
			{ hurtCanvas.alpha -= hurtAlphaReductionRate;}

			// for our HP
			healthText.text = "HP: " + currentHealth;
			healthBar.value = currentHealth;

			// money
			currencyDisplayText.text = "Money: " + currencyAmount;
		}

		void ProcessDamageCooldown()
        {
			if (damageCooldownCurrent > -1)
            {
				damageCooldownCurrent--;
            }
        }

		#endregion

		#region Modifying Health
		public void TakeDamage(int damage)
        {
			if (damageCooldownCurrent <= 0)
			{
				currentHealth -= damage;
				hurtCanvas.alpha += 0.6f;
				CameraScreenshake.CameraInstance.ScreenShake(3, 3, 0.2f, 0.1f, 2);

				if (currentHealth <= 0)
                {
					deadCanvas.alpha = 1;
                }

				damageCooldownCurrent = damageCooldownMax;
			}
		}

		public void AddHealth(int amount)
        {
			currentHealth += amount;
        }

		#endregion

		#endregion
	}
}