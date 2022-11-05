using simplicius.Audio;
using UnityEngine;

public class BulletCasing : MonoBehaviour
{
	[SerializeField] private float force;
	[SerializeField] private AudioEvent collisionSFX;

	private bool hasLanded;
	
	private void Awake()
	{
		GetComponent<Rigidbody>().AddForce(new Vector3(1f, 0f, 1f) * force, ForceMode.Impulse);
	}

	private void OnCollisionEnter(Collision collision)
	{
		if (!hasLanded)
		{
			hasLanded = true;
			AudioManager.Instance.PlayOnce(collisionSFX, transform.position);
		}
	}
}