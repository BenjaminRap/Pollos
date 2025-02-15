using System.Collections.Generic;
using UnityEngine;

public class Faces : MonoBehaviour
{
	[SerializeField]
	private GameObject	_upFace;
	[SerializeField]
	private GameObject	_downFace;
	[SerializeField]
	private GameObject	_rightFace;
	[SerializeField]
	private GameObject	_leftFace;
	[SerializeField]
	private GameObject	_forwardFace;
	[SerializeField]
	private GameObject	_backFace;
	
	private Dictionary<Vector3Int, GameObject>	_faces;
	
	private void Start()
    {
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
	
	public bool	CanRotate(Quaternion localRotation, Quaternion newRotation)
	{
		Vector3Int	frontFaceDirection = Vector3Int.RoundToInt(localRotation * newRotation * Vector3.back);
		
		try
		{
			Debug.Log(_faces[frontFaceDirection]);
			return (_faces[frontFaceDirection] != null);
		}
		catch (System.Exception)
		{
			return (false);
		}
	}
}
