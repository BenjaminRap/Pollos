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
		if (!other.TryGetComponent<Rigidbody>(out Rigidbody rigidbody))
			return ;
		Vector3		velocity = rigidbody.linearVelocity.normalized;
		Vector2Int	direction = (Vector2Int)Vector3Int.RoundToInt(velocity);
		Face 		newFace =_rotationManager.RotateCube(direction);
		if (newFace == null)
			return ;
		if (!PollosController.TryGetInstance(out PollosController characterControler))
			return ;
		characterControler.RotateCharacter();
		other.transform.SetParent(newFace.transform);
		Vector3	newLocalPosition = other.transform.localPosition;
		newLocalPosition.z = 0;
		other.transform.localPosition = newLocalPosition;
    }
}
