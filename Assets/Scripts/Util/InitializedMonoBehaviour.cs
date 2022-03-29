using System;
using UnityEngine;

namespace simplicius.Util
{
	public class InitializedMonoBehaviour : MonoBehaviour
	{
		private event Action onInitialized;
		
		public bool IsReady { get; private set; }

		protected void OnInitialized()
		{
			IsReady = true;
			onInitialized?.Invoke();
		}

		public void WhenInitialized(Action listener)
		{
			if (IsReady)
				listener?.Invoke();
			else
				onInitialized += listener;
		}
	}
}