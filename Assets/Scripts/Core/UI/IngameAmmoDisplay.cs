using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class IngameAmmoDisplay : MonoBehaviour
{
	[SerializeField] private TextMeshProUGUI currentAmmo;
	[SerializeField] private TextMeshProUGUI maxAmmo;

	public void SetAmmo(int current, int max)
	{
		currentAmmo.text = current.ToString();
		maxAmmo.text = max.ToString();
	}
}