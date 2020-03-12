using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
	public bool log;
	[SerializeField]
	protected float walkSpeed = 1;
	[SerializeField]
	protected float runSpeed = 2;

	[SerializeField][Required]
	protected Transform leftHandItemParent;
	[SerializeField][Required]
	protected Transform rightHandItemParent;
	[SerializeField][Required]
	protected Transform headItemParent;
	[SerializeField][Required]
	protected Animator animator;
	[SerializeField][Required]
	protected ParticleSystem runParticle;

	public IEnumerator PlayAnimation(string animation, float normalizedEnd = 0.9f)
	{
		animator.Play(animation, 0);
		yield return null;
		while (animator.GetCurrentAnimatorStateInfo(0).normalizedTime < normalizedEnd)
			yield return null;
		animator.SetTrigger("stop");
	}
}
