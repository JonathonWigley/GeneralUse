using UnityEditor;
using UnityEngine;

[CustomEditor (typeof (DicePrefabs))]
public class DicePrefabsEditor : Editor
{
	SerializedProperty d4_3d;
	SerializedProperty d6_3d;
	SerializedProperty d8_3d;
	SerializedProperty d10_3d;
	SerializedProperty d12_3d;
	SerializedProperty d20_3d;

	SerializedProperty d4_2d;
	SerializedProperty d6_2d;
	SerializedProperty d8_2d;
	SerializedProperty d10_2d;
	SerializedProperty d12_2d;
	SerializedProperty d20_2d;

	SerializedProperty dieDragger;
	SerializedProperty lockUI;

	private bool unfolded3D = true;
	private bool unfolded2D = true;

	private void OnEnable()
	{
		d4_3d = serializedObject.FindProperty("D4_3D");
		d6_3d = serializedObject.FindProperty("D6_3D");
		d8_3d = serializedObject.FindProperty("D8_3D");
		d10_3d = serializedObject.FindProperty("D10_3D");
		d12_3d = serializedObject.FindProperty("D12_3D");
		d20_3d = serializedObject.FindProperty("D20_3D");

		d4_2d = serializedObject.FindProperty("D4_2D");
		d6_2d = serializedObject.FindProperty("D6_2D");
		d8_2d = serializedObject.FindProperty("D8_2D");
		d10_2d = serializedObject.FindProperty("D10_2D");
		d12_2d = serializedObject.FindProperty("D12_2D");
		d20_2d = serializedObject.FindProperty("D20_2D");

		dieDragger = serializedObject.FindProperty("dragger");
		lockUI = serializedObject.FindProperty("lockUI");
	}

	public override void OnInspectorGUI ()
	{
		serializedObject.Update();

		EditorGUI.BeginChangeCheck();

		unfolded3D = EditorGUILayout.Foldout(unfolded3D, new GUIContent("3D Dice"));
		if(unfolded3D)
		{
			EditorGUILayout.PropertyField(d4_3d);
			EditorGUILayout.PropertyField(d6_3d);
			EditorGUILayout.PropertyField(d8_3d);
			EditorGUILayout.PropertyField(d10_3d);
			EditorGUILayout.PropertyField(d12_3d);
			EditorGUILayout.PropertyField(d20_3d);
		}


		unfolded2D = EditorGUILayout.Foldout(unfolded2D, new GUIContent("2D Dice"));
		if(unfolded2D)
		{
			EditorGUILayout.PropertyField(d4_2d);
			EditorGUILayout.PropertyField(d6_2d);
			EditorGUILayout.PropertyField(d8_2d);
			EditorGUILayout.PropertyField(d10_2d);
			EditorGUILayout.PropertyField(d12_2d);
			EditorGUILayout.PropertyField(d20_2d);
		}

		EditorGUILayout.Space();
		EditorGUILayout.Space();

		EditorGUILayout.PropertyField(dieDragger);
		EditorGUILayout.PropertyField(lockUI);

		if(EditorGUI.EndChangeCheck())
			serializedObject.ApplyModifiedProperties();
	}
}