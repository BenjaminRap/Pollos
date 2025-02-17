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
	
	private Transform[]		_childrens;
	
	private int					_hiddenLayer;

    private void Start()
    {
		Cube	cube = GetComponentInParent<Cube>();
		if (cube == null)
		{
			Debug.LogError("A gameObject with t face monobehaviour doesn't have a parent with the monobehaviour cube !");
			Destroy(gameObject);
			return ;
		}
		_childrens = GetComponentsInChildren<Transform>();
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
		foreach (Transform child in _childrens)
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
		Debug.Log(direction);
    }
}
