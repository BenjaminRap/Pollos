using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Draft : MonoBehaviour
{
	[SerializeField] private Vector3	_draftDirection;
	[SerializeField] private float	_draftLength;
	
	private Vector3	_draftForce;

	private void	Awake()
	{
		_draftForce = _draftDirection.normalized * _draftLength;
	}
	
	private void	OnTriggerEnter(Collider collider)
	{
		if (!collider.TryGetComponent(out Rigidbody rigidbody)
			|| !collider.TryGetComponent(out Rotatable rotatable))
			return ;
		SetInDraft(rigidbody, true);
		rigidbody.position -= Vector3.up * 0.1f;
		collider.transform.localPosition = rotatable.GetNearestGridCell();
		StartCoroutine(CheckIfIgidbodyMoves(rigidbody));
	}

	private void	OnTriggerExit(Collider collider)
	{
		if (!collider.TryGetComponent(out Rigidbody rigidbody)
			|| !collider.TryGetComponent(out Rotatable rotatable))
			return ;
		if (!IsInDraft(rigidbody))
			return ;
		SetInDraft(rigidbody, false);
		rigidbody.linearVelocity = Vector2.zero;
		collider.transform.localPosition = rotatable.GetNearestGridCell();
	}

	private void	OnTriggerStay(Collider collider)
	{
		if (!collider.TryGetComponent(out Rigidbody rigidbody))
			return ;
		if (!IsInDraft(rigidbody))
			return ;
		rigidbody.AddForce(transform.TransformVector(_draftForce * Time.deltaTime), ForceMode.Force);
	}
	
	private IEnumerator	CheckIfIgidbodyMoves(Rigidbody rigidbody)
	{
		yield return (new WaitForSeconds(0.5f));
		if (!IsInDraft(rigidbody))
			yield break;
		while (rigidbody.linearVelocity.magnitude > 0.1f)
			yield return (null);
		SetInDraft(rigidbody, false);
	}
	
	private void	SetInDraft(Rigidbody rigidbody, bool inDraft)
	{
		rigidbody.useGravity = !inDraft;
		rigidbody.linearVelocity = Vector3.zero;
	}
	
	private bool	IsInDraft(Rigidbody rigidbody)
	{
		return (rigidbody.useGravity == false);
	}
}
