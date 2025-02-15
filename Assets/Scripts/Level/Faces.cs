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
	
	[SerializeField]
	private SpriteRenderer						_upArrow;
	[SerializeField]
	private SpriteRenderer						_downArraw;
	[SerializeField]
	private SpriteRenderer						_rightArrow;
	[SerializeField]
	private SpriteRenderer						_leftArrow;
	
	private Dictionary<Vector3Int, GameObject>	_faces;
	private Quaternion							_localRotation;
	
	private void Start()
    {
		_localRotation = Quaternion.identity;
		// When getting the current visible face, we must return the face opposite
		// to the frontFaceDirection vector. That is why we inverse the up and down vector.
		// As we are applying the rotation on the vector.back, the rotation on the y axis
		// are already inversed. That's just a guess though.
		_faces = new()
        {
            { Vector3Int.down, _upFace },
            { Vector3Int.up, _downFace },
            { Vector3Int.right, _rightFace },
            { Vector3Int.left, _leftFace },
            { Vector3Int.forward, _forwardFace },
            { Vector3Int.back, _backFace }
        };
		ShowPossibleRotations();
    }
	
	public bool			CanRotate(Quaternion rotation)
	{
		try
		{
			Vector3Int	frontFaceDirection = Vector3Int.RoundToInt(_localRotation * rotation * Vector3.back);
			GameObject	frontFace = _faces[frontFaceDirection];
			return (frontFace != null);
		}
		catch (System.Exception)
		{
			return (false);
		}
	}

	public void	RotateFace(Quaternion rotation)
	{
		_localRotation *= rotation;
	}
	
	public void	ShowPossibleRotations()
	{
		_upArrow.enabled = (CanRotate(Quaternion.AngleAxis(-90, Vector3.right)));
		_downArraw.enabled = (CanRotate(Quaternion.AngleAxis(90, Vector3.right)));
		_rightArrow.enabled = (CanRotate(Quaternion.AngleAxis(-90, Vector3.up)));
		_leftArrow.enabled = (CanRotate(Quaternion.AngleAxis(90, Vector3.up)));
		Debug.Log("up : " + CanRotate(Quaternion.AngleAxis(-90, Vector3.right))
		+ ", down : " + CanRotate(Quaternion.AngleAxis(90, Vector3.right))
		+ ", right : " + CanRotate(Quaternion.AngleAxis(-90, Vector3.up))
		+ ", left : " + CanRotate(Quaternion.AngleAxis(90, Vector3.up)));
	}
}
