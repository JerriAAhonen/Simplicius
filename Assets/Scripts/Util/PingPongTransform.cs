using UnityEngine;

public class PingPongTransform : MonoBehaviour
{
	[SerializeField] private Vector3 magnitude;
	[SerializeField] private float duration;

	private Vector3 startingPos;
	private float elapsed;
	private bool positive;

	private void Start() => startingPos = transform.position;

	private void Update()
	{
		elapsed += Time.deltaTime;
		float step;
		if (positive)
			step = elapsed / duration;
		else
			step = (duration - elapsed) / duration;

		var x = Mathf.Lerp(0, magnitude.x, step);
		var y = Mathf.Lerp(0, magnitude.y, step);
		var z = Mathf.Lerp(0, magnitude.z, step);
		
		transform.position = startingPos + new Vector3(x, y, z);
		
		if (elapsed >= duration)
		{
			elapsed = 0;
			positive = !positive;
		}
	}
}