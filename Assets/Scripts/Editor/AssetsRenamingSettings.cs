using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

class AssetsRenamingSettings : ScriptableObject
{
    public const string path = "Assets/Settings/AssetsRenamingSettings.asset";

	private static AssetsRenamingSettings	_settings = null;

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

	private void	OnValidate()
	{
		_absoluteExcludedPaths = _excludedPaths.Select((path, index) => Path.Combine(_root, _excludedPaths[index])).ToArray();
	}

    private static AssetsRenamingSettings GetOrCreateSettings()
    {
		if (_settings == null)
        	_settings = AssetDatabase.LoadAssetAtPath<AssetsRenamingSettings>(path);
        if (_settings == null)
        {
            _settings = CreateInstance<AssetsRenamingSettings>();
            AssetDatabase.CreateAsset(_settings, path);
            AssetDatabase.SaveAssets();
        }
        return _settings;
    }

    internal static SerializedObject GetSerializedSettings()
    {
        return new SerializedObject(GetOrCreateSettings());
    }
}

static class AssetsRenamingSettingsProvider
{
	private const string _label = "Assets Renaming Settings";

    [SettingsProvider]
    public static SettingsProvider CreateMyCustomSettingsProvider()
    {
        var provider = new SettingsProvider($"Project/{_label}", SettingsScope.Project)
        {
            label =_label,
            guiHandler = (searchContext) =>
            {
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
