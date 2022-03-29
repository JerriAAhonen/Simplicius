using simplicius.Core;
using UnityEngine;

public class MouseLook : MonoBehaviour
{
	[SerializeField] private float sensitivity;
	
	private InputManager inputManager;
	private Player player;
	private PlayerMovement movement;
	private PlayerShooting shooting;
	private Transform tm;

	private float xRot;
	private float yRot;
	
	public Vector2 RotWhileShooting { get; private set; }
	public bool IsReady { get; private set; }
	
	public void Init(Player player, PlayerMovement movement, PlayerShooting shooting)
	{
		inputManager = InputManager.Instance;
		tm = transform;

		this.player = player;
		this.movement = movement;
		this.shooting = shooting;

		shooting.StartedShooting += ResetShootingRot;
		
		IsReady = true;
	}

	private void Update()
	{
		if (!IsReady) return;
		if (!player.IsAlive) return;

		var delta = inputManager.LookInput;
		delta *= sensitivity;
		var yaw = delta.x;
		var pitch = -delta.y;

		if (shooting.IsShooting)
			RotWhileShooting += new Vector2(pitch, yaw);

		xRot += pitch;
		xRot = Mathf.Clamp(xRot, -89, 89);
		
		var targetRot = tm.eulerAngles;
		targetRot.x = xRot;
		tm.eulerAngles = targetRot;
		
		movement.Rotate(yaw);
	}

	private void ResetShootingRot(bool started)
	{
		if (!started)
			RotWhileShooting = Vector2.zero;
	}

	private void OnDisable()
	{
		shooting.StartedShooting -= ResetShootingRot;
	}
}