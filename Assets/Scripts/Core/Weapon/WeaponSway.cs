using simplicius.Util;
using UnityEngine;

namespace simplicius.Core
{
	public class WeaponSway : MonoBehaviour
	{
		[SerializeField] private float swayIntensity;
		[SerializeField] private float snappiness;
		[SerializeField] private float returnSpeed;
		[SerializeField, Range(0f, 1f)] private float aimingMultiplier;
		[Header("Rotation")]
		[SerializeField] private float jumpIntensityRot;
		[SerializeField] private float landIntensityRot;
		[Header("Position")]
		[SerializeField] private float jumpIntensityPos;
		[SerializeField] private float landIntensityPos;
		[SerializeField] private float movementIntensity;

		private PlayerShooting shooting;
		private PlayerMovement movement;

		// Rotation
		private Vector3 targetRot;
		private Vector3 currentRot;

		// Position
		private Vector3 targetPos;
		private Vector3 currentPos;
	
		private void Awake()
		{
			shooting = GetComponentInParent<PlayerShooting>();
			movement = GetComponentInParent<PlayerMovement>();
			movement.MovementStateChanged += OnMovementStateChanged;
		}
		
		private void Update()
		{
			// Rotation
			var yaw = InputManager.Instance.LookInput.x * swayIntensity;
			var pitch = -InputManager.Instance.LookInput.y * swayIntensity;
			
			targetRot += new Vector3(pitch, yaw, -yaw / 3f);
			targetRot = MiscUtil.ClampVector3(targetRot, 10f, 10f, 10f);

			if (shooting.IsAiming)
				targetRot *= aimingMultiplier;
			
			targetRot = Vector3.Lerp(targetRot, Vector3.zero, returnSpeed * Time.deltaTime);
			currentRot = Vector3.Slerp(currentRot, targetRot, snappiness * Time.fixedDeltaTime);
			transform.localRotation = Quaternion.Euler(currentRot);
			
			// Position
			var movementVel = InputManager.Instance.MovementInput * movementIntensity;

			targetPos += new Vector3(-movementVel.x / 2f, 0f, -movementVel.y);
			targetPos = MiscUtil.ClampVector3(targetPos, 10f, 10f, 10f);

			if (shooting.IsAiming)
				targetPos *= aimingMultiplier;
			
			targetPos = Vector3.Lerp(targetPos, Vector3.zero, returnSpeed * Time.deltaTime);
			currentPos = Vector3.Slerp(currentPos, targetPos, snappiness * Time.fixedDeltaTime);
			transform.localPosition = currentPos;
		}

		private void OnDisable()
		{
			movement.MovementStateChanged -= OnMovementStateChanged;
		}

		private void OnMovementStateChanged(MovementState newState)
		{
			if (newState.Is<MovementState_Jumping>())
			{
				// Jumped
				targetRot += new Vector3(-jumpIntensityRot, 0f, 0f);
				targetPos += new Vector3(0f, jumpIntensityPos, 0f);
			}
			else if (newState.PreviousState != null && newState.PreviousState.Is<MovementState_Jumping>())
			{
				// Landed
				targetRot += new Vector3(landIntensityRot, 0f, 0f);
				targetPos += new Vector3(0f, -landIntensityPos, 0f);
			}
			else if (newState.Is<MovementState_Sprinting>())
			{
				// Started Sprinting
			}
			else if (newState.Is<MovementState_Walking>())
			{
				// Started Walking
			}
			else if (newState.Is<MovementState_Crouching>())
			{
				// Started Crourching
			}
		}
	}
}