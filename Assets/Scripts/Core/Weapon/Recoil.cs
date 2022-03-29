using simplicius.Core;
using UnityEngine;
using Random = UnityEngine.Random;

public class Recoil : MonoBehaviour
{
	private MouseLook mouseLook;
	private PlayerShooting shooting;
	
	private Vector3 targetRot;
	private Vector3 currentRot;
	
	[SerializeField] private float snappiness;
	[SerializeField] private float returnSpeed;

	private void Awake()
	{
		mouseLook = GetComponentInParent<MouseLook>();
		shooting = GetComponentInParent<PlayerShooting>();
	}

	private Vector3 compensatedTargetRot;
	private void Update()
	{
		// TODO: Take into account recoil compensation from the player
		/*if (shooting.IsShooting)
		{
			compensatedTargetRot = new Vector3(-mouseLook.RotWhileShooting.x, -mouseLook.RotWhileShooting.y, 0);
			targetRot = Vector3.Lerp(targetRot, Vector3.zero, returnSpeed * Time.deltaTime);
		}
		else
			targetRot = Vector3.Lerp(targetRot, compensatedTargetRot, returnSpeed * Time.deltaTime);*/
		
		targetRot = Vector3.Lerp(targetRot, Vector3.zero, returnSpeed * Time.deltaTime);
		currentRot = Vector3.Slerp(currentRot, targetRot, snappiness * Time.fixedDeltaTime);
		transform.localRotation = Quaternion.Euler(currentRot);
	}

	public void OnShoot(Vector3 recoil)
	{
		targetRot += new Vector3(recoil.x, Random.Range(-recoil.y, recoil.y), Random.Range(-recoil.z, recoil.z));
	}
}