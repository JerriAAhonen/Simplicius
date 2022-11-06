using System.Collections.Generic;
using UnityEngine;

namespace simplicius.Util
{
	public static class Extensions
	{
		public static T GetOrAddComponent<T>(this GameObject self)
			where T : Component
		{
			T component = self.GetComponent<T>();
			if (component != null)
				return component;
			return self.AddComponent<T>();
		}
	}
	
	public static class Vector2Extensions
	{
		public static bool ApproximatelyEquals(this Vector2 self, Vector2 other)
		{
			return Mathf.Approximately(self.x, other.x)
			       && Mathf.Approximately(self.y, other.y);
		}
	}

	public static class Vector3Extensions
	{
		public static bool ApproximatelyEquals(this Vector3 self, Vector3 other)
		{
			return Mathf.Approximately(self.x, other.x)
			       && Mathf.Approximately(self.y, other.y)
			       && Mathf.Approximately(self.z, other.z);
		}

		public static void Clamp(this Vector3 self, float xClamp, float yClamp, float zClamp)
		{
			self.x = Mathf.Clamp(self.x, -xClamp, xClamp);
			self.y = Mathf.Clamp(self.x, -yClamp, yClamp);
			self.z = Mathf.Clamp(self.x, -zClamp, zClamp);
		}
	}

	public static class ListExtensions
	{
		public static T Random<T>(this List<T> self)
		{
			if (self.Count == 0)
				return default;
			return self[UnityEngine.Random.Range(0, self.Count - 1)];
		}
	}
	
	public static class RectTransformExtensions
	{
		public static Vector2 GetSize(this RectTransform self, Space space = Space.Self)
		{
			if (space == Space.Self)
				return self.rect.size;
			return Vector2.Scale(self.rect.size, self.lossyScale);
		}

		public static float GetWidth(this RectTransform self, Space space = Space.Self)
		{
			if (space == Space.Self)
				return self.rect.width;
			return Vector2.Scale(self.rect.size, self.lossyScale).x;
		}
	
		public static float GetHeight(this RectTransform self, Space space = Space.Self)
		{
			if (space == Space.Self)
				return self.rect.height;
			return Vector2.Scale(self.rect.size, self.lossyScale).y;
		}
	
		public static void SetSize(this RectTransform self, Vector2 size)
		{
			Vector2 oldSize = self.rect.size;
			Vector2 deltaSize = size - oldSize;

			self.offsetMin = self.offsetMin - new Vector2(
				deltaSize.x * self.pivot.x,
				deltaSize.y * self.pivot.y);
			self.offsetMax = self.offsetMax + new Vector2(
				deltaSize.x * (1f - self.pivot.x),
				deltaSize.y * (1f - self.pivot.y));
		}
	
		public static void SetWidth(this RectTransform self, float size)
		{
			self.SetSize(new Vector2(size, self.rect.size.y));
		}

		public static void SetHeight(this RectTransform self, float size)
		{
			self.SetSize(new Vector2(self.rect.size.x, size));
		}

		public static Canvas GetRootCanvas(this RectTransform self)
		{
			return self.root.GetComponentInChildren<Canvas>();
		}
	
		public static Rect GetScreenSpaceRect(this RectTransform self, Camera camera)
		{
			return self.GetScreenSpaceRect(camera, Vector2.one);
		}
	
		public static Rect GetScreenSpaceRect(this RectTransform self, Camera camera, Vector2 scale)
		{
			Vector2 size = Vector2.Scale(self.GetSize(Space.World), scale);
			Rect rect = new Rect((Vector2) self.position - size * 0.5f, size);

			Vector2 min = RectTransformUtility.WorldToScreenPoint(camera, rect.min);
			Vector2 max = RectTransformUtility.WorldToScreenPoint(camera, rect.max);

			return new Rect(min, max - min);
		}
	}
}