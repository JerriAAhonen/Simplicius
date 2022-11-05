using simplicius.Audio;
using UnityEngine;

namespace simplicius.Core
{
	public enum PlayerControlType { Local, Network }
	
	public class Player : Actor
	{
		[Header("General")] 
		[SerializeField] private PlayerControlType controlType;
		[SerializeField] private Transform local;
		[SerializeField] private Transform network;
		[Header("Audio")]
		[SerializeField] private AudioEvent walkingFootstepSfx;
		[SerializeField] private AudioEvent sprintingFootstepSfx;
		[SerializeField] private AudioEvent crouchingFootstepSfx;
		
		private MouseLook mouseLook;

		public PlayerMovement Movement { get; private set; }
		public PlayerShooting Shooting { get; private set; }
		public PlayerAnimations Anim { get; private set; }
		public FirstPersonCamera FPCamera { get; private set; }
		public Transform CameraRoot { get; private set; }
		
		// Audio
		public AudioEvent WalkingFootstepSfx => walkingFootstepSfx;
		public AudioEvent SprintingFootstepSfx => sprintingFootstepSfx;
		public AudioEvent CrouchingFootstepSfx => crouchingFootstepSfx;

		private void Start()
		{
			FPCamera = GetComponentInChildren<FirstPersonCamera>();
			CameraRoot = FPCamera.transform.parent;

			Movement = GetComponent<PlayerMovement>();
			Shooting = controlType == PlayerControlType.Local ? local.GetComponent<PlayerShooting>() : network.GetComponent<PlayerShooting>();
			Anim = GetComponent<PlayerAnimations>();
			
			Movement.Init(this);
			Shooting.Init(this);
			Anim.Init(Shooting);

			mouseLook = GetComponentInChildren<MouseLook>();
			mouseLook.Init(this, Movement, Shooting);
			
			OnInitialized();
		}
	}
}