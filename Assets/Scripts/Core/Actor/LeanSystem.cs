using UnityEngine;

public class LeanSystem : MonoBehaviour
{
	[SerializeField] private Vector3 leanAmount;
	[SerializeField] private float leanSmoothing;

	private Vector3 targetRotation;
	private Vector3 targetRotationVelocity;
	
	private Vector3 newRotation;
	private Vector3 newRotationVelocity;

	private Vector3 inputRotation;
	
	public void OnLean(bool lean, bool right)
	{
		if (lean)
		{
			inputRotation = right ? leanAmount : -leanAmount;
		}
		else
		{
			inputRotation = Vector3.zero;
		}
	}

	private void Update()
	{
		targetRotation = inputRotation;
		targetRotation = Vector3.SmoothDamp(targetRotation, Vector3.zero, ref targetRotationVelocity, leanSmoothing);
		newRotation = Vector3.SmoothDamp(newRotation, targetRotation, ref newRotationVelocity, leanSmoothing);
		transform.localRotation = Quaternion.Euler(newRotation);
	}
}