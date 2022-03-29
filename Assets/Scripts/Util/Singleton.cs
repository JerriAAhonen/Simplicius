using System;
using UnityEngine;

namespace simplicius.Util
{
	public abstract class Singleton<T> : MonoBehaviour where T : class
	{
		public static T Instance { get; private set; }

		protected virtual void Awake()
		{
			ProcessSingleton();
		}
		
		private void  ProcessSingleton()
		{
			if (!Application.isPlaying) return;

			if (Instance != null)
			{
				Destroy(gameObject); 
				return;
			}

			Instance = this as T;
			if (Instance == null)
				throw new InvalidCastException($"Unable to cast {GetType()} to {typeof(T)}");
		}
	}
}