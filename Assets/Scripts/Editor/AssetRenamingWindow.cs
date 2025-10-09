using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.IO;
using System.Linq;

[SerializeField]
public class	FileToRename
{
	public string correctName;
	public string title;
	public bool shouldRename = true;
	public bool unfolded = false;

	public FileToRename(string path, string correctName)
	{
		title = $"{Path.GetFileName(path)} => {correctName}";
		this.correctName = correctName;
	}
}

public class	AssetRenamingWindow : EditorWindow
{
	private static readonly Dictionary<string, FileToRename>	_filesToRename = new();

	private bool _unfolded = true;
	private bool _closeOnRename = true;
	private Vector2 _scrollPos;

	[MenuItem("Tools/AssetRenaming")]
	public static void ShowWindow()
	{
		AssetRenamingWindow	popup = (AssetRenamingWindow)GetWindow(typeof(AssetRenamingWindow));

		popup.minSize = new Vector2(300, 500);
		popup.maxSize = new Vector2(600, 800);
	}

	public static void	ShowWindow(List<(string, string)> additionalFilesToRename)
	{
		foreach ((string path, string correctName) in additionalFilesToRename)
		{
		    _filesToRename[path] = new (path, correctName);
		}
		ShowWindow();
	}

	private void	OnGUI()
	{
		_scrollPos = EditorGUILayout.BeginScrollView(_scrollPos);
		DrawAllFilesToRename();
		DrawTogglingButtons();
		DrawClearButton();
		DrawRenameButton();
		_closeOnRename = EditorGUILayout.Toggle("Close On Rename", _closeOnRename);
		EditorGUILayout.EndScrollView();
	}

	private void	DrawClearButton()
	{
		Color defaultColor = GUI.color;

		GUI.color = Color.red;
        if (GUILayout.Button("Clear"))
			_filesToRename.Clear();
		GUI.color = defaultColor;
	}

	private void	DrawRenameButton()
	{
		Color defaultColor = GUI.color;

		GUI.color = Color.green;
        if (GUILayout.Button("Rename Selected Files."))
        {
			RenameSelectedAssets();
			if (_closeOnRename)
				Close();
        }
		GUI.color = defaultColor;
	}

	private void	DrawTogglingButtons()
	{
		EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Toggle Everything"))
			foreach (FileToRename fileToRename in _filesToRename.Values)
			    fileToRename.shouldRename = true;
        if (GUILayout.Button("Untoggle Everything"))
			foreach (FileToRename fileToRename in _filesToRename.Values)
			    fileToRename.shouldRename = false;
		EditorGUILayout.EndHorizontal();

		EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Fold Everything"))
			foreach (FileToRename fileToRename in _filesToRename.Values)
			    fileToRename.unfolded = false;
        if (GUILayout.Button("Unfold Everything"))
			foreach (FileToRename fileToRename in _filesToRename.Values)
			    fileToRename.unfolded = true;
		EditorGUILayout.EndHorizontal();
	}

	private void	DrawAllFilesToRename()
	{
		_unfolded = EditorGUILayout.Foldout(_unfolded, "Files To Rename");
		if (_unfolded) 
		{
		   EditorGUI.indentLevel++;
		   foreach (string path in _filesToRename.Keys)
		   {
				DrawFileToRename(path, _filesToRename[path]);
		   }
		   EditorGUI.indentLevel--;
		}
	}

	private void	DrawFileToRename(string path, FileToRename fileToRename)
	{
		EditorGUILayout.BeginHorizontal();
		fileToRename.unfolded = EditorGUILayout.Foldout(fileToRename.unfolded, "");
		EditorGUILayout.LabelField(fileToRename.title);
		fileToRename.shouldRename = EditorGUILayout.Toggle(fileToRename.shouldRename);
		EditorGUILayout.EndHorizontal();
		if (fileToRename.unfolded) 
		{
			EditorGUI.indentLevel++;
			EditorGUILayout.LabelField($"path : {path}");
			EditorGUI.indentLevel--;
		}
	}

	private void	RenameSelectedAssets()
	{
	
		foreach (var kvp in _filesToRename.Where(kvp => kvp.Value.shouldRename).ToList())
		{
			AssetDatabase.RenameAsset(kvp.Key, kvp.Value.correctName);
			_filesToRename.Remove(kvp.Key);
		}
	}
}
