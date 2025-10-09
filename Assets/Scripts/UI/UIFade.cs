using UnityEngine;

public class UIFade : MonoBehaviour {
	[SerializeField] private Animator _animatorFade = null;

	private void Start() {
		_animatorFade.SetBool("RevealWorld", true);
	}

	public void FadeIn() {
		_animatorFade.SetBool("RevealWorld", false);
	}

	public void FadeOut() {
		_animatorFade.SetBool("RevealWorld", true);
	}
}
