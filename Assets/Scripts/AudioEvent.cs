using System.Collections.Generic;
using simplicius.Util;
using UnityEngine;

namespace simplicius.Audio
{
	[CreateAssetMenu(fileName = "-SFX", menuName = "Audio/AudioEvent")]
	public class AudioEvent : ScriptableObject
	{
		[SerializeField] private List<AudioClip> clips;
		[SerializeField, Range(0f, 1f)] private float volume = 1f;
		[SerializeField] private float volumeVariance;
		[SerializeField, Range(-3f, 3f)] private float pitch = 1f;
		[SerializeField] private float pitchVariance;
		[SerializeField] private float minInterval;
		[SerializeField] private float delay;
	
		public AudioClip Clip => clips.Random();
		public float Volume => Mathf.Clamp(Random.Range(volume - volumeVariance, volume + volumeVariance), 0f, 1f);
		public float Pitch => Mathf.Clamp(Random.Range(pitch - pitchVariance, pitch + pitchVariance), -3f, 3f);

		public float MINInterval => minInterval;
		public float Delay => delay;
	}
}