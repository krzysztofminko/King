using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
	public bool log;

	[SerializeField][Required]
	protected Transform leftHandItemParent;
	[SerializeField][Required]
	protected Transform rightHandItemParent;
	[SerializeField][Required]
	protected Transform headItemParent;

	protected Animator animator;

	protected virtual void Awake()
	{
		animator = GetComponent<Animator>();
	}
	
}
