using System.Collections.Generic;
using simplicius.Util;
using UnityEngine;
using UnityEngine.UI;

namespace simplicius.Core
{
	public class Crosshair : Singleton<Crosshair>
	{
		[Header("Crosshair")] 
		[SerializeField] private RectTransform crosshairRt;
		[SerializeField] private float animationDur;
		[Header("Hit Marker")] 
		[SerializeField] private RectTransform hitMarkerRt;
		[SerializeField] private Animation hitMarkerAnimation;
		[SerializeField] private Color defaultHitColor;
		[SerializeField] private Color killHitColor;

		private CanvasGroup crosshairCg;
		private CanvasGroup hitMarkerCg;
		private List<Image> hitMarkers;

		private float defaultWidth;
		private float currentWidth;
		private int? crosshairTweenId;

		protected override void Awake()
		{
			base.Awake();

			defaultWidth = currentWidth = crosshairRt.GetWidth();
			crosshairCg = crosshairRt.GetComponent<CanvasGroup>();
			hitMarkerCg = hitMarkerRt.GetComponent<CanvasGroup>();
			crosshairCg.alpha = 1f;
			hitMarkerCg.alpha = 0f;

			hitMarkers = new List<Image>(hitMarkerRt.GetComponentsInChildren<Image>());
			SetHitMarkerColor(defaultHitColor);

			var player = WorldManager.Instance.GetLocalPlayer();
			player.WhenInitialized(() =>
			{
				player.Shooting.Shot += OnShoot;
				player.Movement.MovementStateChanged += OnMovementStateChanged;
			});
		}

		public void SetCrosshairScale(float mult)
		{
			currentWidth = defaultWidth * mult;
			AnimateCrosshair(currentWidth);
		}

		public void ShowCrosshair(bool show)
		{
			crosshairCg.alpha = show ? 1f : 0f;
		}

		public void ShowHitMarker(bool killed)
		{
			SetHitMarkerColor(killed ? killHitColor : defaultHitColor);
			hitMarkerAnimation.Play(PlayMode.StopAll);
		}

		private void AnimateCrosshair(float to)
		{
			if (crosshairTweenId.HasValue)
				LeanTween.cancel(crosshairTweenId.Value);

			crosshairTweenId = LeanTween.value(gameObject, crosshairRt.GetWidth(), to, animationDur)
				.setOnUpdate(v => crosshairRt.SetSize(new Vector2(v, v)))
				.setOnComplete(() => crosshairTweenId = null).uniqueId;
		}

		private void SetHitMarkerColor(Color color)
		{
			foreach (var hitMarker in hitMarkers)
				hitMarker.color = color;
		}

		private void OnShoot()
		{
			var shotWidth = currentWidth * 3f;
			crosshairRt.SetSize(new Vector2(shotWidth, shotWidth));
			AnimateCrosshair(currentWidth);
		}

		private void OnMovementStateChanged(MovementState newState)
		{
			SetCrosshairScale(newState.CrosshairModifier);
		}
	}
}