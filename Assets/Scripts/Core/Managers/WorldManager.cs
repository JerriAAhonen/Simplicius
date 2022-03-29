using System;
using simplicius.Util;
using UnityEngine;

namespace simplicius.Core
{
	public class WorldManager : Singleton<WorldManager>
	{
		[SerializeField] private Player player;

		public Player GetLocalPlayer() => player;

		private void Start()
		{
			Application.targetFrameRate = 60;
		}

		private void Update()
		{
			//if (Input.GetKeyDown(KeyCode.Space))
			//	Time.timeScale = 0.5f;
		}
	}
}