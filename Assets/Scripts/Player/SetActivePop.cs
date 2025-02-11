using UnityEngine;

public class SetActivePop : MonoBehaviour
{
	[SerializeField] private GameObject _fade = null;

	public void PlayGame()
	{
		_fade.SetActive(true);
	}
}
