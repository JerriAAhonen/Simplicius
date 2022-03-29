using System.Collections;
using System.Collections.Generic;
using simplicius.Util;
using UnityEngine;

namespace simplicius.Core
{
	public class Actor : InitializedMonoBehaviour
	{
		[SerializeField] private int maxHealth = 100;
		[SerializeField] private int maxArmor = 100;
		[SerializeField] private int startingArmor = 25;

		public int Health;
		public int MaxHealth => maxHealth;
		
		public int Armor;
		public int MaxArmor => maxArmor;

		public bool IsAlive = true;

		public void Init()
		{
			Reset();
			OnInitialized();
		}

		public void AddHealth()
		{
		}

		public bool TakeDamage(int amount)
		{
			Debug.Log($"{transform.name} took {amount} damage");
			Health -= amount;
			Health = Mathf.Clamp(Health, 0, MaxHealth);
			return Health == 0;
		}

		public void Reset()
		{
			Health = maxHealth;
			Armor = startingArmor;
			IsAlive = true;
		}
	}
}