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

	[SerializeField]
	private SpriteRenderer						_upArrow;
	[SerializeField]
	private SpriteRenderer						_downArraw;
	[SerializeField]
	private SpriteRenderer						_rightArrow;
	[SerializeField]
	private SpriteRenderer						_leftArrow;
	
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
		ShowPossibleRotations(Quaternion.identity);
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
	
	public void	ShowPossibleRotations(Quaternion localRotation)
	{
		// _upArrow.enabled = (GetFace(_localRotation, Quaternion.AngleAxis(-90, Vector3.right)) != null);
		// _downArraw.enabled = (GetFace(_localRotation, Quaternion.AngleAxis(90, Vector3.right)) != null);
		// _rightArrow.enabled = (GetFace(_localRotation, Quaternion.AngleAxis(-90, Vector3.up)) != null);
		// _leftArrow.enabled = (GetFace(_localRotation, Quaternion.AngleAxis(90, Vector3.up)) != null);
	}
}
