using UnityEngine;

namespace simplicius.Util
{
	public static class MiscUtil
	{
		public static void ActivateCanvasGroup(CanvasGroup cg, bool active)
		{
			cg.alpha = active ? 1f : 0f;
			cg.interactable = active;
			cg.blocksRaycasts = active;
		}

		public static Vector3 ClampVector3(Vector3 v, float xClamp, float yClamp, float zClamp)
		{
			var x = Mathf.Clamp(v.x, -xClamp, xClamp);
			var y = Mathf.Clamp(v.y, -yClamp, yClamp);
			var z = Mathf.Clamp(v.z, -zClamp, zClamp);
			return new Vector3(x, y, z);
		}
	}
}