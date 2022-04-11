using simplicius.Core;
using simplicius.Util;
using UnityEngine;

public class IngameHUD : Singleton<IngameHUD>
{
	[SerializeField] private Crosshair crosshair;
	[SerializeField] private IngameAmmoDisplay ammoDisplay;
	
	public Crosshair Crosshair => crosshair;
	public IngameAmmoDisplay AmmoDisplay => ammoDisplay;
}