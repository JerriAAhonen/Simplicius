using System;
using System.Collections.Generic;
using UnityEngine;

namespace simplicius.Util
{
	public static class PhysicsUtil
	{
		private static readonly Dictionary<string, int> _layers = new Dictionary<string, int>(8);

		public static readonly Collider[] Colliders = new Collider[64];
		public static readonly RaycastHit[] Hits = new RaycastHit[64];

		public static bool IsLayerInMask(LayerMask mask, int layer)
		{
			return mask == (mask | (1 << layer));
		}

		public static int GetClosestHit(int count, Predicate<RaycastHit> ignore = null)
		{
			int closest = -1;
			float distance = 0f;

			for (int i = 0; i < count; i++)
			{
				if (ignore != null && ignore(Hits[i]))
					continue;

				float d = Hits[i].distance;
				if (closest < 0 || d < distance)
				{
					closest = i;
					distance = d;
				}
			}

			return closest;
		}

		public static int GetClosestHit(Vector3 position, int count, Predicate<RaycastHit> ignore = null)
		{
			int closest = -1;
			float distance = 0f;

			for (int i = 0; i < count; i++)
			{
				if (ignore != null && ignore(Hits[i]))
					continue;

				float d = (position - Hits[i].point).sqrMagnitude;
				if (closest < 0 || d < distance)
				{
					closest = i;
					distance = d;
				}
			}

			return closest;
		}

		public static int GetClosestCollider(Vector3 position, int count, Predicate<Collider> ignore = null)
		{
			int closest = -1;
			float distance = 0f;

			for (int i = 0; i < count; i++)
			{
				if (ignore != null && ignore(Colliders[i]))
					continue;

				float d = (position - Colliders[i].transform.position).sqrMagnitude;
				if (closest < 0 || d < distance)
				{
					closest = i;
					distance = d;
				}
			}

			return closest;
		}

		public static bool GetHit(Collider collider, int hitCount, out RaycastHit hit)
		{
			for (int i = 0; i < hitCount; i++)
			{
				hit = Hits[i];
				if (hit.collider == collider)
					return true;
			}

			hit = default;
			return false;
		}

		public static bool FindHit(int hitCount, out RaycastHit hit, Predicate<RaycastHit> predicate)
		{
			for (int i = 0; i < hitCount; i++)
			{
				hit = Hits[i];
				if (predicate(hit))
					return true;
			}

			hit = default;
			return false;
		}

		public static int GetLayer(string layer)
		{
			if (!_layers.TryGetValue(layer, out int value))
			{
				value = LayerMask.NameToLayer(layer);
				_layers.Add(layer, value);
			}
			return value;
		}

		public static int GetMask(string layer)
		{
			return AddLayerToMask(GetLayer(layer), 0);
		}

		public static int GetMask(string layer0, string layer1)
		{
			return GetMask(GetLayer(layer0), GetLayer(layer1));
		}

		public static int GetMask(string layer0, string layer1, string layer2)
		{
			return GetMask(GetLayer(layer0), GetLayer(layer1), GetLayer(layer2));
		}

		public static int GetMask(string layer0, string layer1, string layer2, string layer3)
		{
			return GetMask(GetLayer(layer0), GetLayer(layer1), GetLayer(layer2), GetLayer(layer3));
		}

		public static int GetMask(int layer)
		{
			return AddLayerToMask(layer, 0);
		}

		public static int GetMask(int layer0, int layer1)
		{
			int mask = AddLayerToMask(layer0, 0);
			mask = AddLayerToMask(layer1, mask);
			return mask;
		}

		public static int GetMask(int layer0, int layer1, int layer2)
		{
			int mask = AddLayerToMask(layer0, 0);
			mask = AddLayerToMask(layer1, mask);
			mask = AddLayerToMask(layer2, mask);
			return mask;
		}

		public static int GetMask(int layer0, int layer1, int layer2, int layer3)
		{
			int mask = AddLayerToMask(layer0, 0);
			mask = AddLayerToMask(layer1, mask);
			mask = AddLayerToMask(layer2, mask);
			mask = AddLayerToMask(layer3, mask);
			return mask;
		}

		private static int AddLayerToMask(int layer, int mask)
		{
			if (layer != -1)
				mask |= 1 << layer;
			return mask;
		}
	}
}