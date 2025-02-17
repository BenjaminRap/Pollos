using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Face : MonoBehaviour
{
	[SerializeField]
	private GameObject			_upArrow;
	[SerializeField]
	private GameObject			_downArrow;
	[SerializeField]
	private GameObject			_rightArrow;
	[SerializeField]
	private GameObject			_leftArrow;
	
	private RotationManager		_rotationManager;
	private int					_hiddenLayer;

    private void Start()
    {
		Cube	cube = GetComponentInParent<Cube>();
		_rotationManager = GetComponentInParent<RotationManager>();
		if (cube == null || _rotationManager == null)
		{
			Debug.LogError("Missing component in the Face parent : Cube or RotationManager !");
			Destroy(gameObject);
			return ;
		}
		_hiddenLayer = LayerMask.NameToLayer("Hidden");
		SetRendered(transform.rotation == Quaternion.identity);
		ShowPossibleRotations(cube);
    }

	private void	ShowPossibleRotations(Cube cube)
	{
		_upArrow.SetActive(cube.GetFace(transform.rotation * Quaternion.AngleAxis(-90, Vector3.right)) != null);
		_downArrow.SetActive(cube.GetFace(transform.rotation * Quaternion.AngleAxis(90, Vector3.right)) != null);
		_rightArrow.SetActive(cube.GetFace(transform.rotation * Quaternion.AngleAxis(-90, Vector3.up)) != null);
		_leftArrow.SetActive(cube.GetFace(transform.rotation * Quaternion.AngleAxis(90, Vector3.up)) != null);
	}
	
	public void	SetRendered(bool rendered)
	{
		Transform[] childrens = GetComponentsInChildren<Transform>();
		foreach (Transform child in childrens)
		{
			child.gameObject.layer = rendered ? 0 : _hiddenLayer;
		}
	}

    private void OnTriggerExit(Collider other)
    {
		Vector3			velocity = other.attachedRigidbody.linearVelocity.normalized;
		Vector3Int		direction = Vector3Int.RoundToInt(velocity);
		LevelRotation	levelRotation = _rotationManager.CanRotate(direction);
		if (levelRotation == null)
			return ;
		other.transform.SetParent(levelRotation.NewFace.transform);
		other.transform.localPosition = TransformUtils.SetZ(other.transform.localPosition, 0.0f);
		_rotationManager.Rotate(levelRotation);
    }
}
