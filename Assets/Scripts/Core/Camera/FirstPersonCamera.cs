using UnityEngine;

namespace simplicius.Core
{
	[RequireComponent(typeof(Camera))]
	public class FirstPersonCamera : MonoBehaviour
	{
		private const float FovTransitionDur = 0.1f;
		
		private Camera cam;
		private float defaultFov;

		private void Awake()
		{
			cam = GetComponent<Camera>();
			defaultFov = cam.fieldOfView;
		}

		public void SetFov(float fov)
		{
			AnimateFov(fov);
		}

		public void ResetFov()
		{
			AnimateFov(defaultFov);
		}

		private void AnimateFov(float target)
		{
			LeanTween.cancel(gameObject);
			LeanTween.value(gameObject, cam.fieldOfView, target, FovTransitionDur)
				.setOnUpdate(v => cam.fieldOfView = v)
				.setEase(target < cam.fieldOfView ? LeanTweenType.easeOutCubic : LeanTweenType.easeInCubic);
		}
	}
}