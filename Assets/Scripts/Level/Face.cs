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

	private void OnTriggerExit(Collider other)
	{
		if (!other.TryGetComponent<Rotatable>(out Rotatable rotatable))
			return ;
		Rigidbody		rigidbody = other.attachedRigidbody;
		Vector3			velocity = rigidbody.linearVelocity.normalized;
		Vector3Int		direction = Vector3Int.RoundToInt(velocity);
		LevelRotation	levelRotation = _rotationManager.CanRotate(direction);
		if (levelRotation == null)
			return ;
		levelRotation.NewFace.SetParentToRigidbody(rigidbody);
		if (other.TryGetComponent<PollosController>(out PollosController pollosController)
			&& other.transform.forward == Vector3.back)
			_rotationManager.Rotate(levelRotation);
		else
		{
			other.transform.rotation = levelRotation.NewFace.transform.rotation;
			rigidbody.MovePosition(rotatable.GetNearestGridCell(rigidbody.linearVelocity));
			rotatable.UpdateGravityUse();
		}
	}
	
	public void	SetRendered(bool rendered)
	{
		if (rendered)
			Camera.main.cullingMask |= 1 << gameObject.layer;
		else
			Camera.main.cullingMask &= ~(1 << gameObject.layer);
	}
	
	public void	SetParentToRigidbody(Rigidbody rigidbody)
	{
		rigidbody.excludeLayers = ~(1 << gameObject.layer);
		rigidbody.transform.SetParent(transform);
		rigidbody.transform.localPosition = TransformUtils.SetZ(rigidbody.transform.localPosition, 0.0f);
		rigidbody.transform.gameObject.layer = gameObject.layer;
	}
}
