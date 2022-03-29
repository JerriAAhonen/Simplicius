using System;
using UnityEngine;

namespace simplicius.Core
{
	public class MovementState : MonoBehaviour
	{
		protected Player player;
		protected PlayerMovement movement;
		protected CharacterController cc;

		public bool IsReady { get; private set; }
		public float SpeedModifier { get; protected set; } = 1f;
		public float CrosshairModifier { get; protected set; } = 1f;
		
		public MovementState PreviousState { get; private set; }

		#region MonoBehaviour

		private void Update()
		{
			if (IsReady)
				OnUpdate();
		}

		#endregion

		#region Init

		public virtual void Init(Player player, PlayerMovement movement, CharacterController cc)
		{
			this.player = player;
			this.movement = movement;
			this.cc = cc;
			IsReady = true;
		}

		#endregion

		#region Enter

		public virtual bool CanEnter(MovementState currentState) => false;
		
		public void Enable(MovementState previousState)
		{
			if (!IsReady) return;

			enabled = true;
			OnEnter(previousState);
		}
		
		protected virtual void OnEnter(MovementState previousState)
		{
			PreviousState = previousState;
		}

		#endregion
		
		#region Update
		
		protected virtual void OnUpdate()
		{
			if (movement.IsGrounded)
			{
				movement.VerticalVel.y = -0.1f;
				movement.LastTimeGrounded = Time.time;
			}
		}
		
		#endregion
		
		#region Exit

		private void OnDisable()
		{
			if (IsReady)
				OnExit();
		}

		protected virtual void OnExit() { } 
		
		#endregion

		#region Util

		public bool Is<T>() where T : MovementState
		{
			return this is T;
		}

		public bool PreviousWas<T>() where T : MovementState
		{
			return PreviousState != null && PreviousState.Is<T>();
		}

		#endregion
	}
}