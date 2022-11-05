using simplicius.Audio;
using UnityEngine;

namespace simplicius.Core
{
	public class PracticeTarget : Actor
	{
		[SerializeField] private AudioEvent hitSFX;
		
		public override bool TakeDamage(int amount)
		{
			AudioManager.Instance.PlayOnce(hitSFX);
			return false;
		}
	}
}