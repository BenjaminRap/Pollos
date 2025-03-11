using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Face : MonoBehaviour
{
	private RotationManager		_rotationManager;
	private Collider			_collider;

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
		_collider = GetComponent<Collider>();
		SetRendered(transform.rotation == Quaternion.identity);
	}

	private void OnTriggerExit(Collider other)
	{
		if (!other.TryGetComponent(out Rotatable rotatable))
			return ;
		Rigidbody		rigidbody = other.attachedRigidbody;
		Vector3			normalizedVelocity = rotatable.IsFroze ? rotatable.VelocityAtFreeze.normalized : rigidbody.linearVelocity.normalized ;
		Vector3Int		direction = Vector3Int.RoundToInt(normalizedVelocity);
		if (other.TryGetComponent(out PollosController _)
			&& Vector3Int.RoundToInt(other.transform.forward) == Vector3Int.forward)
		{
			Face newFace = _rotationManager.Rotate(direction);
			if (newFace != null)
				newFace.SetParentToRigidbody(rigidbody);
		}
		else
		{
			Vector3Int	rotationAxis = _rotationManager.GetRotationAxisFromInput(direction);
			if (rotationAxis == Vector3Int.zero)
				return ;
			Quaternion	relativeRotation = Quaternion.AngleAxis(90, rotationAxis);
			Quaternion	globalRotation = other.transform.parent.rotation * relativeRotation;
			Face		newFace = _rotationManager.Cube.GetFace(Quaternion.Inverse(globalRotation) * other.transform.parent.forward);
			if (newFace == null)
				return ;
			newFace.SetParentToRigidbody(rigidbody);
			other.transform.localRotation = Quaternion.identity;
			Vector3	newPosition = rotatable.GetNearestGridCell(newFace.GetRelativeDirectionToClosestPointOnBounds(other.transform));
			if (!RigidbodyUtils.CanRigidbodyMoveTo(rigidbody, newPosition))
				Debug.Log("It has been teleported but has collided !");
			rotatable.UpdateGravityUse();
			rigidbody.transform.position = newPosition;
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

	public Vector3	GetRelativeDirectionToClosestPointOnBounds(Transform obj)
	{
		Vector3	closestPointOnBounds = _collider.ClosestPointOnBounds(obj.position);
		Vector3	direction = closestPointOnBounds - obj.position;
		Vector3	relativeDirection = obj.InverseTransformDirection(direction);
		return (relativeDirection);
	}
}
