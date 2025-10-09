using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class AssetsRenamingSettings : ScriptableObject {
	public const string PATH = "Assets/Settings/AssetsRenamingSettings.asset";

	private static AssetsRenamingSettings _settings = null;

	[SerializeField]
	private string _root = "Assets";
	[SerializeField]
	private string[] _excludedPaths;
	[SerializeField]
	private bool _backgroundRenaming = false;

	private string[] _absoluteExcludedPaths;

	public static string Root { get => GetOrCreateSettings()._root; }
	public static string[] ExcludedPaths { get => GetOrCreateSettings()._absoluteExcludedPaths; }
	public static bool BackgroundRenaming { get => GetOrCreateSettings()._backgroundRenaming; }

	private void OnValidate() {
		_absoluteExcludedPaths = _excludedPaths.Select((path, index) => Path.Combine(_root, _excludedPaths[index])).ToArray();
	}

	private static AssetsRenamingSettings GetOrCreateSettings() {
		if (_settings == null)
			_settings = AssetDatabase.LoadAssetAtPath<AssetsRenamingSettings>(PATH);
		if (_settings == null) {
			_settings = CreateInstance<AssetsRenamingSettings>();
			AssetDatabase.CreateAsset(_settings, PATH);
			AssetDatabase.SaveAssets();
		}
		return _settings;
	}

	internal static SerializedObject GetSerializedSettings() {
		return new SerializedObject(GetOrCreateSettings());
	}
}

public static class AssetsRenamingSettingsProvider {
	private const string LABEL = "Assets Renaming Settings";

	[SettingsProvider]
	public static SettingsProvider CreateMyCustomSettingsProvider() {
		var provider = new SettingsProvider($"Project/{LABEL}", SettingsScope.Project) {
			label = LABEL,
			guiHandler = (searchContext) => {
				var settings = AssetsRenamingSettings.GetSerializedSettings();
				EditorGUILayout.PropertyField(settings.FindProperty("_root"), new GUIContent("Root"));
				EditorGUILayout.PropertyField(settings.FindProperty("_backgroundRenaming"), new GUIContent("Background Renaming"));
				EditorGUILayout.PropertyField(settings.FindProperty("_excludedPaths"), new GUIContent("Excluded Paths Relative To Root"));
				settings.ApplyModifiedProperties();
			},
		};

		return provider;
	}
}
