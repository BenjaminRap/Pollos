using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System;
using UnityEditor.Animations;
using UnityEngine.U2D;
using UnityEngine.Video;
using System.IO;
using UnityEngine.VFX;
using System.Linq;

public class AssetNameValidator : AssetPostprocessor {
	private static readonly Dictionary<Type, string> _typePrefixes = new()
	{
		{ typeof(Texture2D), "T" },
		{ typeof(Sprite), "SP" },
		{ typeof(Material), "M" },
		{ typeof(Shader), "SH" },
		{ typeof(Mesh), "SM" },
		{ typeof(SkinnedMeshRenderer), "AM" },
		{ typeof(AnimationClip), "AN" },
		{ typeof(AnimatorController), "AC" },
		{ typeof(ScriptableObject), "SO" },
		{ typeof(AudioClip), "A" },
		{ typeof(Font), "F" },
		{ typeof(SpriteAtlas), "SA" },
		{ typeof(VisualEffectAsset), "VFX" },
		{ typeof(TerrainData), "TD" },
		{ typeof(RenderTexture), "RT" },
		{ typeof(VideoClip), "VC" },
		{ typeof(Avatar), "AV" },
		{ typeof(LightmapParameters), "LP" },
		{ typeof(PhysicsMaterial), "PM" },
		{ typeof(PhysicsMaterial2D), "PM2D" }
	};

	[MenuItem("Tools/EnforceNorm")]
	private static void EnforceNorm() {
		IEnumerable<string> files = Directory.EnumerateFiles(AssetsRenamingSettings.Root, "*.*", SearchOption.AllDirectories);

		EnforceNorm(files);
	}

	private static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths, bool didDomainReload) {
		IEnumerable<string> filesToCheck = importedAssets.Concat(movedAssets);

		EnforceNorm(filesToCheck);
	}

	private static void EnforceNorm(IEnumerable<string> filesPath) {
		List<(string, string)> filesToRename = new();

		foreach (string path in filesPath) {
			if (IsExcluded(path))
				continue;
			string name = Path.GetFileName(path);
			string correctName = GetCorrectName(path, name);

			if (correctName.Equals(name))
				continue;
			filesToRename.Add((path, correctName));
		}
		if (filesToRename.Count == 0)
			return;
		if (AssetsRenamingSettings.BackgroundRenaming)
			RenameAllAssetsNextFrame(filesToRename);
		else
			AssetRenamingWindow.ShowWindow(filesToRename);
	}

	private static void RenameAllAssetsNextFrame(List<(string, string)> filesToRename) {
		EditorApplication.delayCall += () => {
			foreach ((string path, string correctName) in filesToRename)
				AssetDatabase.RenameAsset(path, correctName);
		};
	}

	private static bool IsExcluded(string path) {
		int index = Array.FindIndex(AssetsRenamingSettings.ExcludedPaths, (excludedPath) => path.StartsWith(excludedPath));

		return index != -1;
	}

	private static string GetCorrectName(string path, string name) {
		Type type = GetAssetType(path);

		if (type == null || !_typePrefixes.TryGetValue(type, out string prefix))
			return name;

		if (name.StartsWith(prefix))
			return name;
		string correctName = $"{prefix}_{name}";

		return correctName;
	}

	private static Type GetAssetType(string path) {
		Type type = AssetDatabase.GetMainAssetTypeAtPath(path);

		if (type == typeof(Texture2D)
			|| type == typeof(Mesh)) {
			AssetImporter assetImporter = AssetImporter.GetAtPath(path);

			if (IsAssetImportTypeOf(assetImporter, out TextureImporter textureImporter))
				return (textureImporter.textureType == TextureImporterType.Sprite) ? typeof(Sprite) : type;
			if (IsAssetImportTypeOf(assetImporter, out ModelImporter modelImporter))
				return (modelImporter.animationType != ModelImporterAnimationType.None) ? typeof(SkinnedMeshRenderer) : type;
		}
		return type;
	}

	private static bool IsAssetImportTypeOf<T>(AssetImporter assetImporter, out T type) where T : AssetImporter {
		if (assetImporter.GetType() == typeof(T)) {
			type = assetImporter as T;
			return true;
		}
		type = null;
		return false;
	}
}
