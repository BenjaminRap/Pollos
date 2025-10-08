using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Face : MonoBehaviour
{
	[SerializeField] private GameObject	_upArrow;
	[SerializeField] private GameObject	_downArrow;
	[SerializeField] private GameObject	_rightArrow;
	[SerializeField] private GameObject	_leftArrow;
	
	private Transform[]	_childrens;
	
	private int	_hiddenLayer;

	private void	Awake()
	{
		_childrens = GetComponentsInChildren<Transform>();
		_hiddenLayer = LayerMask.NameToLayer("Hidden");
		SetRendered(transform.rotation == Quaternion.identity);
	}

    private void Start()
    {
		ShowPossibleRotations();
    }

	private void ShowPossibleRotations()
	{
		if (!Cube.TryGetInstance(out Cube cube))
			return ;
		_upArrow.SetActive(IsRotationPossible(Vector3.left, cube));
		_downArrow.SetActive(IsRotationPossible(Vector3.right, cube));
		_rightArrow.SetActive(IsRotationPossible(Vector3.down, cube));
		_leftArrow.SetActive(IsRotationPossible(Vector3.up, cube));
	}

	private bool IsRotationPossible(Vector3 axis, Cube cube)
	{
		Quaternion localRotation = Quaternion.AngleAxis(90f, axis);
		Quaternion globalRotation = transform.rotation * localRotation;

		return (cube.TryGetFace(globalRotation, out Face _));
	}
	
	public void	SetRendered(bool rendered)
	{
		foreach (Transform child in _childrens)
		{
			child.gameObject.layer = rendered ? 0 : _hiddenLayer;
		}
	}
}
