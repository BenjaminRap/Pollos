using System.Collections.Generic;
using UnityEngine;

public class Cube : MonoBehaviourSingleton<Cube>
{
	[SerializeField] private Face	_upFace;
	[SerializeField] private Face	_downFace;
	[SerializeField] private Face	_rightFace;
	[SerializeField] private Face	_leftFace;
	[SerializeField] private Face	_forwardFace;
	[SerializeField] private Face	_backFace;
	
	private Dictionary<Vector3Int, Face>		_faces;
	
	protected override void Awake()
	{
		base.Awake();
		_faces = new()
		{
			{ Vector3Int.up, _upFace },
			{ Vector3Int.down, _downFace },
			{ Vector3Int.right, _rightFace },
			{ Vector3Int.left, _leftFace },
			{ Vector3Int.forward, _forwardFace },
			{ Vector3Int.back, _backFace }
		};
	}
	
	public Face	GetFace(Quaternion localRotation)
	{
		Vector3Int	frontFaceDirection = Vector3Int.RoundToInt(localRotation * Vector3.back);

		if (_faces.TryGetValue(frontFaceDirection, out Face frontFace))
			return (frontFace);
		return null;
	}
}
