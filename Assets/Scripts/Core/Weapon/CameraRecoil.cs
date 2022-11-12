using simplicius.Core;
using UnityEngine;
using Random = UnityEngine.Random;

public class CameraRecoil : MonoBehaviour
{
	private MouseLook mouseLook;
	private PlayerShooting shooting;
	
	private Vector3 targetRot;
	private Vector3 newRot;
	
	[SerializeField] private float snappiness;
	[SerializeField] private float returnSpeed;

	private void Awake()
	{
		mouseLook = GetComponentInParent<MouseLook>();
		shooting = GetComponentInParent<PlayerShooting>();
	}

	private void Update()
	{
		targetRot = Vector3.Lerp(targetRot, Vector3.zero, returnSpeed * Time.deltaTime);
		newRot = Vector3.Slerp(newRot, targetRot, snappiness * Time.fixedDeltaTime);
		transform.localRotation = Quaternion.Euler(newRot);
	}

	public void OnShoot(Vector3 recoil)
	{
		targetRot += new Vector3(recoil.x, Random.Range(-recoil.y, recoil.y), Random.Range(-recoil.z, recoil.z));
	}
}