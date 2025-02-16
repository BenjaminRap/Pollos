using UnityEngine;

public class Face : MonoBehaviour
{
	[SerializeField]
	private SpriteRenderer						_upArrow;
	[SerializeField]
	private SpriteRenderer						_downArraw;
	[SerializeField]
	private SpriteRenderer						_rightArrow;
	[SerializeField]
	private SpriteRenderer						_leftArrow;

    private void Start()
    {
        Debug.Log(transform.rotation.eulerAngles);
    }
	
}
