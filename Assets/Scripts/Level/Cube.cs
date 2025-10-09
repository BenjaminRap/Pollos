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
	}
	
	public bool	TryGetFace(Quaternion rotation, out Face face)
	{
		Vector3Int	frontFaceDirection = Vector3Int.RoundToInt(rotation * Vector3.back);

		if (_faces.TryGetValue(frontFaceDirection, out face))
			return (true);
		return false;
	}
}
