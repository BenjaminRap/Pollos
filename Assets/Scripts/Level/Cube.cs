using System.Collections.Generic;
using UnityEngine;

public class Cube : MonoBehaviour
{
	[SerializeField]
	private Face								_upFace;
	[SerializeField]
	private Face								_downFace;
	[SerializeField]
	private Face								_rightFace;
	[SerializeField]
	private Face								_leftFace;
	[SerializeField]
	private Face								_forwardFace;
	[SerializeField]
	private Face								_backFace;
	
	private Dictionary<Vector3Int, Face>		_faces;
	
	private void Start()
	{
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
	
	public Face			GetFace(Quaternion localRotation)
	{
		try
		{
			Vector3Int	frontFaceDirection = Vector3Int.RoundToInt(localRotation * Vector3.back);
			Face		frontFace = _faces[frontFaceDirection];
			return (frontFace);
		}
		catch (System.Exception)
		{
			return (null);
		}
	}
}
