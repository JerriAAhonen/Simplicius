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
			public GameObject go;
		}
		
		[Serializable]
		public class WeaponAttachment_MuzzlesMap
		{
			public WeaponAttachment_Muzzles type;
			public GameObject go;
		}

		[SerializeField] private List<WeaponAttachment_SightsMap> sights;
		[SerializeField] private List<WeaponAttachment_MuzzlesMap> muzzles;

		private WeaponAttachment_Sights currentSight;
		private WeaponAttachment_Muzzles currentMuzzle;
		
		public float RecoilModifier { get; private set; }
		public float ADSZoomModifier { get; private set; }

		public void ChangeSight(WeaponAttachment_Sights type)
		{
			
		}
		
		public void ChangeMuzzle(WeaponAttachment_Muzzles type)
		{
			
		}
	}
}