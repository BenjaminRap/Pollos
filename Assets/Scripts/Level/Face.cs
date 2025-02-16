using UnityEngine;

public class Face : MonoBehaviour
{
	[SerializeField]
	private SpriteRenderer		_upArrow;
	[SerializeField]
	private SpriteRenderer		_downArraw;
	[SerializeField]
	private SpriteRenderer		_rightArrow;
	[SerializeField]
	private SpriteRenderer		_leftArrow;
	
	private SpriteRenderer[]	_renderers;

    private void Start()
    {
		_renderers = GetComponentsInChildren<SpriteRenderer>();
		SetRendered(transform.rotation == Quaternion.identity);
    }
	
	public void	SetRendered(bool rendered)
	{
		foreach (SpriteRenderer spriteRenderer in _renderers)
		{
			spriteRenderer.enabled = rendered;
		}
	}
}
