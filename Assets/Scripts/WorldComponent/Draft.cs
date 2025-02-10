using System.Collections;
using UnityEngine;

/// <summary>This class adds the draft behaviour. It changes the gravity of
/// rigidbodys that enter its collider, to match the _draftDirection</summary>
[RequireComponent(typeof(Collider2D))]
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
	
	/// <summary>If a collider with a rigidbody enters this collider, add this
	/// collider in the draft.</summary>
	private void	OnTriggerEnter2D(Collider2D collider)
	{
		if (!collider.TryGetComponent<Rigidbody2D>(out Rigidbody2D rigidbody))
			return ;
		SetInDraft(rigidbody, true);
		rigidbody.position -= Vector2.up * 0.1f;
		Level.PlaceTransformInGrid(collider.transform);
		StartCoroutine(CheckIfIgidbodyMoves(rigidbody));
	}

	/// <summary>If a collider with a rigidbody exits this collider, remove
	/// this collider from the draft.</summary>
	private void	OnTriggerExit2D(Collider2D collider)
	{
		if (!collider.TryGetComponent<Rigidbody2D>(out Rigidbody2D rigidbody))
			return ;
		if (!IsInDraft(rigidbody)) // his gravioty has already been returned
			return ;
		SetInDraft(rigidbody, false);
		rigidbody.linearVelocity = Vector2.zero;
		Level.PlaceTransformInGrid(collider.transform);
	}

	/// <summary>While a collider with a rigidbody is in the draft, push it to the
	/// _draftDirection.</summary>
	private void	OnTriggerStay2D(Collider2D collider)
	{
		if (!collider.TryGetComponent<Rigidbody2D>(out Rigidbody2D rigidbody))
			return ;
		if (!IsInDraft(rigidbody))
			return ;
		rigidbody.AddForce(transform.TransformVector(_draftForce * Time.deltaTime), ForceMode2D.Force);
	}
	
	/// <summary> After half a second, this coroutine check, each frame, if the gameObject
	/// is moving, if not, remove it from the draft.</summary>
	private IEnumerator	CheckIfIgidbodyMoves(Rigidbody2D rigidbody)
	{
		yield return (new WaitForSeconds(0.5f));
		if (!IsInDraft(rigidbody))
			yield break;
		while (rigidbody.linearVelocity.magnitude > 0.1f)
			yield return (null);
		SetInDraft(rigidbody, false);
	}
	
	/// <summary>Set a rigidbody in the draft or not.</summary>
	/// <param name="inDraft">True if the rigidbody should be added in the draft, 
	/// false if the rigidbody should be removed from the draft</param>
	private void	SetInDraft(Rigidbody2D rigidbody, bool inDraft)
	{
		if (inDraft)
			rigidbody.gravityScale = 0;
		else
			rigidbody.gravityScale = 1;
		rigidbody.linearVelocity = Vector2.zero;
	}
	
	/// <returns>True if the rigidbody is in the draft, false otherwise</returns>
	private bool	IsInDraft(Rigidbody2D rigidbody)
	{
		return (rigidbody.gravityScale == 0);
	}
}
