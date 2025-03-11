using UnityEngine;

public class Cube : MonoBehaviour
{
	private const	float	ANGLE_EPSILON = 0.1f;			
	[SerializeField]
	private Face[]			_faces;
	
	public Face			GetFace(Vector3	faceForward)
	{
		foreach (Face face in _faces)
		{
		    if (Vector3.Angle(faceForward, face.transform.forward) < ANGLE_EPSILON)
				return (face);
		}
		return (null);
	}
}
