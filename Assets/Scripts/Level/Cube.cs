using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Cube : MonoBehaviourSingleton<Cube>
{
	private Dictionary<Vector3Int, Face>	_faces;
	
	protected override void Awake()
	{
		base.Awake();
		Face[] _facesList = GetComponentsInChildren<Face>();

		_faces = _facesList.ToDictionary(face => Vector3Int.RoundToInt(-face.transform.forward));
		foreach (KeyValuePair<Vector3Int, Face> keyValuePair in _faces)
		{
			Debug.Log(keyValuePair.Key + ", " + keyValuePair.Value);
		}
	}
	
	public Face	GetFace(Quaternion localRotation)
	{
		Vector3Int	frontFaceDirection = Vector3Int.RoundToInt(localRotation * Vector3.back);

		if (_faces.TryGetValue(frontFaceDirection, out Face frontFace))
			return (frontFace);
		return null;
	}
}
