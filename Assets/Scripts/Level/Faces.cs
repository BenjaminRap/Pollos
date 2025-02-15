using System.Collections.Generic;
using UnityEngine;

public class Faces : MonoBehaviour
{
	[SerializeField]
	private GameObject							_upFace;
	[SerializeField]
	private GameObject							_downFace;
	[SerializeField]
	private GameObject							_rightFace;
	[SerializeField]
	private GameObject							_leftFace;
	[SerializeField]
	private GameObject							_forwardFace;
	[SerializeField]
	private GameObject							_backFace;
	
	private Dictionary<Vector3Int, GameObject>	_faces;
	private Quaternion							_localRotation;
	
	private void Start()
    {
		_localRotation = Quaternion.identity;
		_faces = new()
        {
            { Vector3Int.down, _upFace }, // no idea why I must inverse up and down, it works
            { Vector3Int.up, _downFace },
            { Vector3Int.right, _rightFace },
            { Vector3Int.left, _leftFace },
            { Vector3Int.forward, _forwardFace },
            { Vector3Int.back, _backFace }
        };
    }
	
	public GameObject	RotateCube(Quaternion rotation)
	{
		try
		{
			Quaternion	newLocalRotation = _localRotation * rotation;
			Vector3Int	frontFaceDirection = Vector3Int.RoundToInt(newLocalRotation * Vector3.back);
			GameObject	frontFace = _faces[frontFaceDirection];
			if (frontFace != null)
				_localRotation = newLocalRotation;
			return (frontFace);
		}
		catch (System.Exception)
		{
			return (null);
		}
	}
	
	public void	RotateFace(Quaternion rotation)
	{
		_localRotation *= rotation;
	}
}
