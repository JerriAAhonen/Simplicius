using System;
using System.Collections;
using System.Collections.Generic;
using simplicius.Core;
using simplicius.Util;
using UnityEngine;
using UnityEngine.Pool;

namespace simplicius.Audio
{
	public class AudioManager : Singleton<AudioManager>
	{
		[SerializeField] private Transform sourceContainer;
		
		private Transform mainCamTm;
		private IObjectPool<AudioSource> pool;
		private List<AudioSource> activeSources;
		private Dictionary<AudioEvent, float> cooldowns;
		
		protected override void Awake()
		{
			base.Awake();
			
			PoolUtil.Get(ref activeSources);
			PoolUtil.Get(ref cooldowns);
			
			pool = new ObjectPool<AudioSource>(
				CreateAudioSource,
				s =>
				{
					s.gameObject.SetActive(true);
					activeSources.Add(s);
				},
				s =>
				{
					s.Stop();
					s.clip = null;
					s.gameObject.SetActive(false);
					s.transform.SetParent(sourceContainer);
					activeSources.Remove(s);
				},
				Destroy);
		}

		private void Start()
		{
			var player = WorldManager.Instance.GetLocalPlayer();
			player.WhenInitialized(() =>
			{
				mainCamTm = player.FPCamera.transform;
			});
		}

		private void LateUpdate()
		{
			foreach (var s in activeSources)
			{
				if (!s.isPlaying)
				{
					pool.Release(s);
					return;
				}
			}
		}

		private void OnDisable()
		{
			PoolUtil.Release(ref activeSources);
			PoolUtil.Release(ref cooldowns);
		}

		/// <summary>
		/// Plays given Audio event once
		/// </summary>
		/// <param name="ae">Audio Event</param>
		/// <param name="pos">Position in world space where to play the audio event. Leave null to play at Camera</param>
		/// <returns>Duration of the audio clip</returns>
		public float PlayOnce(AudioEvent ae, Vector3? pos = null)
		{
			if (!CanBePlayed(ae))
				return 0f;
			
			var s = pool.Get();
			s.clip = ae.Clip;
			s.volume = ae.Volume;
			s.pitch = ae.Pitch;
			s.loop = false;

			if (pos.HasValue)
				s.transform.position = pos.Value;
			else
			{
				s.transform.SetParent(mainCamTm);
				s.transform.localPosition = Vector3.zero;
			}
			
			if (ae.Delay <= 0f)
				s.Play();
			else
				s.PlayDelayed(ae.Delay);
			
			return s.clip.length / Mathf.Abs(s.pitch);
		}

		private AudioSource CreateAudioSource()
		{
			var go = new GameObject("AudioSource");
			go.transform.SetParent(sourceContainer);

			var source = go.AddComponent<AudioSource>();
			source.playOnAwake = false;
			source.spatialBlend = 1f;
			source.minDistance = 10f;
			source.maxDistance = 30f;
			return source;
		}

		private bool CanBePlayed(AudioEvent ae)
		{
			if (ae.MINInterval <= 0f)
				return true;

			if (cooldowns.TryGetValue(ae, out float endsAt) && Time.realtimeSinceStartup < endsAt)
				return false;

			cooldowns[ae] = Time.realtimeSinceStartup + ae.MINInterval;
			return true;
		}
	}
}