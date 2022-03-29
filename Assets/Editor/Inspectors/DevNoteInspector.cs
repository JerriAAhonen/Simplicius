using simplicius.Util;
using UnityEditor;
using UnityEngine;

namespace Editor.Inspectors
{
	[CustomEditor(typeof(DevNote))]
	public class DevNoteInspector : UnityEditor.Editor
	{
		public override void OnInspectorGUI()
		{
			serializedObject.UpdateIfRequiredOrScript();

			var colorProp = serializedObject.FindProperty("m_color");

			Rect rect = GUILayoutUtility.GetRect(1, 1);
			Rect vRect = EditorGUILayout.BeginVertical();

			if (!DevNote.EditingEnabled)
			{
				var r = new Rect(rect.x - 17, rect.y - 4, rect.width + 20, vRect.height + 16);
				EditorGUI.DrawRect(r, colorProp.colorValue);
			}

			var typeProp = serializedObject.FindProperty("m_type");
			MessageType type = (MessageType) typeProp.intValue;

			var textProp = serializedObject.FindProperty("m_text");

			if (DevNote.EditingEnabled)
			{
				EditorGUILayout.PropertyField(colorProp, true);

				type = (MessageType) EditorGUILayout.EnumPopup("Type", type);
				typeProp.intValue = (int) type;

				EditorGUILayout.PropertyField(textProp, true);

				serializedObject.ApplyModifiedProperties();

				if (GUILayout.Button("Finish editing", GUILayout.Width(200)))
				{
					DevNote.EditingEnabled = false;
				}
			}
			else
			{
				EditorGUILayout.HelpBox(textProp.stringValue, type, true);
			}

			EditorGUILayout.EndVertical();
		}
	}
}