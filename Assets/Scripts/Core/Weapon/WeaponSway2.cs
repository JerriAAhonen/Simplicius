using System;
using simplicius.Util;
using UnityEngine;

namespace simplicius.Core
{
	public class WeaponSway2 : MonoBehaviour
	{
		[Header("Look sway")]
		[SerializeField] private float lookSwayAmount = 6f;
		[SerializeField] private float lookSwaySmoothing = 0.1f;
		[SerializeField] private float lookSwayResetSmoothing = 0.05f;
		[SerializeField] private float clampX = 4;
		[SerializeField] private float clampY = 4;

		[Header("Movement sway")] 
		[SerializeField] private Vector2 movementSwayAmount;
		[SerializeField] private float movementSwaySmoothing = 0.1f;
		[SerializeField] private float movementSwayResetSmoothing = 0.05f;

		// Look
		private Vector3 targetLookRotation;
		private Vector3 targetLookRotationVelocity;
		
		private Vector3 newLookRotation;
		private Vector3 newLookRotationVelocity;

		// Movement
		private Vector3 targetMovementRotation;
		private Vector3 targetMovementRotationVelocity;

		private Vector3 newMovementRotation;
		private Vector3 newMovementRotationVelocity;

		#region MonoBehaviour
		
		private void Start()
		{
			newLookRotation = transform.localRotation.eulerAngles;
		}

		private void Update()
		{
			CalculateRotation();
		}
		
		#endregion

		private void CalculateRotation()
		{
			// Look
			targetLookRotation.y += lookSwayAmount * InputManager.Instance.LookInput.x * Time.deltaTime;
			targetLookRotation.x += -lookSwayAmount * InputManager.Instance.LookInput.y * Time.deltaTime;
			targetLookRotation.Clamp(clampX, clampY, 0);
			targetLookRotation.z = -targetLookRotation.y;
			
			targetLookRotation = Vector3.SmoothDamp(targetLookRotation, Vector3.zero, ref targetLookRotationVelocity, movementSwayResetSmoothing);
			newLookRotation = Vector3.SmoothDamp(newLookRotation, targetLookRotation, ref newLookRotationVelocity, lookSwaySmoothing);

			// Movement
			targetMovementRotation.z = -movementSwayAmount.x * InputManager.Instance.MovementInput.x;
			targetMovementRotation.x = movementSwayAmount.y * InputManager.Instance.MovementInput.y;
			
			targetMovementRotation = Vector3.SmoothDamp(targetMovementRotation, Vector3.zero, ref targetMovementRotationVelocity, lookSwayResetSmoothing);
			newMovementRotation = Vector3.SmoothDamp(newMovementRotation, targetMovementRotation, ref newMovementRotationVelocity, movementSwaySmoothing);
			
			// Set
			transform.localRotation = Quaternion.Euler(newLookRotation + newMovementRotation);
		}
	}
}