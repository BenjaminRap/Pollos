using UnityEngine;

public class Level : MonoBehaviour
{
	private const float		_rotationAngle = 90.0f;
	
	[SerializeField]
	private float			_rotationDuration = 0.2f;
	[SerializeField]
	private GameObject		_rotableChild;

	private static Level	_instance;
	
	private Coroutine		_rotateCoroutine;
	private Quaternion		_rotationGoal;
	private Storm[]			_storms;

	private void		Start()
	{
		if (_instance != null)
		{
			Debug.LogError("Multiples instance of the level class !");
			Destroy(this);
			return ;
		}
		_instance = this;
		_rotationGoal = _rotableChild.transform.rotation;
		_storms = GetComponentsInChildren<Storm>();
	}
	
	public static Level	GetInstance()
	{
		return (_instance);
	}
	
	public void			Rotate(float axisValue)
	{
		CharacterControler	characterControler = CharacterControler.GetInstance();

		if (characterControler == null)
		{
			Debug.LogError("The CharacterControler class has no instance !");
			return ;
		}
		characterControler.RotateCharacter();
		foreach (Storm storm in _storms)
		{
			storm.ComeCloser();
		}
		if (_rotateCoroutine != null)
			StopCoroutine(_rotateCoroutine);
		_rotationGoal *= Quaternion.Euler(-axisValue * _rotationAngle * Vector3.forward);
		_rotateCoroutine = StartCoroutine(TransformUtils.RotateInTime(_rotableChild.transform, _rotationGoal, _rotationDuration));
	}
}
