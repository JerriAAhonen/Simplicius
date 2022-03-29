using JetBrains.Annotations;
using UnityEngine;

namespace simplicius.Util
{
	public sealed class DevNote : MonoBehaviour
	{
		[SerializeField, UsedImplicitly] private Color m_color = Color.clear;
		[SerializeField, UsedImplicitly] private int m_type;
		[TextArea(5, 5)]
		[SerializeField, UsedImplicitly] private string m_text;

#if UNITY_EDITOR
		public static bool EditingEnabled;
	
		[ContextMenu("Enable Editing")]
		public void EnableEditing()
		{
			EditingEnabled = true;
		}
#endif
	}
}