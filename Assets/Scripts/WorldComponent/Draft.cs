using System.Collections;
using UnityEngine;

public class Draft : MonoBehaviour
{
	[SerializeField]
	private Vector3	_draftDirection;
	[SerializeField]
	private float	_draftLength;
	
	private Vector3	_draftForce;

	private void	Start()
	{
		_draftForce = _draftDirection.normalized * _draftLength;
	}
	
	private void	OnTriggerEnter2D(Collider2D collider)
	{
		if (!collider.TryGetComponent<Rigidbody2D>(out Rigidbody2D rigidbody))
			return ;
		SetInDraft(rigidbody, true);
		rigidbody.position -= Vector2.up * 0.3f;
		Level.placeTransformInGrid(collider.transform);
		StartCoroutine(CheckIfIgidbodyMoves(rigidbody));
	}

	private void	OnTriggerExit2D(Collider2D collider)
	{
		if (!collider.TryGetComponent<Rigidbody2D>(out Rigidbody2D rigidbody))
			return ;
		if (!IsInDraft(rigidbody)) // his gravioty has already been returned
			return ;
		SetInDraft(rigidbody, false);
		rigidbody.linearVelocity = Vector2.zero;
		Level.placeTransformInGrid(collider.transform);
	}

	private void	OnTriggerStay2D(Collider2D collider)
	{
		if (!collider.TryGetComponent<Rigidbody2D>(out Rigidbody2D rigidbody))
			return ;
		if (!IsInDraft(rigidbody))
			return ;
		rigidbody.AddForce(transform.TransformVector(_draftForce * Time.deltaTime), ForceMode2D.Impulse);
	}
	
	private IEnumerator	CheckIfIgidbodyMoves(Rigidbody2D rigidbody)
	{
		yield return (new WaitForSeconds(1.0f));
		if (!IsInDraft(rigidbody))
			yield break;
		if (rigidbody.linearVelocity.magnitude < 0.1f)
			SetInDraft(rigidbody, false);
	}
	
	private void	SetInDraft(Rigidbody2D rigidbody, bool inDraft)
	{
		if (inDraft)
			rigidbody.gravityScale = 0;
		else
			rigidbody.gravityScale = 1;
		rigidbody.linearVelocity = Vector2.zero;
	}
	
	private bool	IsInDraft(Rigidbody2D rigidbody)
	{
		return (rigidbody.gravityScale == 0);
	}
}
