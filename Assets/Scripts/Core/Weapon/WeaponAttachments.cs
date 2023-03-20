using System;
using System.Collections.Generic;
using UnityEngine;

namespace simplicius.Core
{
	public enum WeaponAttachment_Sights
	{
		RedDot, scope_x4,
	}

	public enum WeaponAttachment_Muzzles
	{
		Compensator, Suppressor
	}
	
	[Serializable]
	public class WeaponAttachments
	{
		[Serializable]
		public class WeaponAttachment_SightsMap
		{
			public WeaponAttachment_Sights type;
			public GameObject sight;
			public Transform adsRefPos;
			public float adsFOV;
		}
		
		[Serializable]
		public class WeaponAttachment_MuzzlesMap
		{
			public WeaponAttachment_Muzzles type;
			public GameObject muzzle;
			public Transform shootPoint;
			public float recoilMod;
		}

		[SerializeField] private List<WeaponAttachment_SightsMap> sights;
		[SerializeField] private List<WeaponAttachment_MuzzlesMap> muzzles;

		private WeaponAttachment_Sights currentSight;
		private WeaponAttachment_Muzzles currentMuzzle;

		public float RecoilModifier { get; private set; } = 1f;
		public float ADSFov { get; private set; } = 1f;

		public void Init()
		{
			foreach (var item in sights)
			{
				item.sight.SetActive(false);
			}

			ChangeSight(WeaponAttachment_Sights.RedDot);

			foreach (var item in muzzles)
			{
				item.muzzle.SetActive(false);
			}

			ChangeMuzzle(WeaponAttachment_Muzzles.Compensator);
		}

		public void ChangeSight(WeaponAttachment_Sights type)
		{
			var currentMap = sights.Find(x => x.type == currentSight);
			var newMap = sights.Find(x => x.type == type);

			currentMap.sight.SetActive(false);
			newMap.sight.SetActive(true);

			currentSight = type;
			ADSFov = newMap.adsFOV;
		}
		
		public void ChangeMuzzle(WeaponAttachment_Muzzles type)
		{
			var currentMap = muzzles.Find(x => x.type == currentMuzzle);
			var newMap = muzzles.Find(x => x.type == type);
			
			currentMap.muzzle.SetActive(false);
			newMap.muzzle.SetActive(true);

			currentMuzzle = type;
			RecoilModifier = newMap.recoilMod;
		}

		public WeaponAttachment_SightsMap GetCurrentSight() => sights.Find(x => x.type == currentSight);
		public WeaponAttachment_MuzzlesMap GetCurrentMuzzle() => muzzles.Find(x => x.type == currentMuzzle);

		public WeaponAttachment_Sights GetNextSight()
		{
			return currentSight == WeaponAttachment_Sights.RedDot
				? WeaponAttachment_Sights.scope_x4
				: WeaponAttachment_Sights.RedDot;
		}
		
		public WeaponAttachment_Muzzles GetNextMuzzle()
		{
			return currentMuzzle == WeaponAttachment_Muzzles.Compensator
				? WeaponAttachment_Muzzles.Suppressor
				: WeaponAttachment_Muzzles.Compensator;
		}
	}
}