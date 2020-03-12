using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class FadeActivator : MonoBehaviour
{
	[SerializeField][ReadOnly]
	private bool _active;
	public bool Active
	{
		get => _active;
		set
		{
			if (_active != value)
			{
				_active = value;
				if (value)
				{
					gameObject.SetActive(true);
					if (fadeIn)
					{
						if (animation.isPlaying)
							animation.CrossFadeQueued(fadeIn.name);
						else
							animation.Play(fadeIn.name);
					}
					if (loop)
						animation.CrossFadeQueued(loop.name);
				}
				else
				{
					StartCoroutine(Deactivate());
				}
			}
		}
	}

	public new Animation animation;

	public AnimationClip fadeIn;
	public AnimationClip loop;
	public AnimationClip fadeOut;


	private void Reset()
	{
		animation = GetComponent<Animation>();
	}

	private void Awake()
	{
		if(!animation)
			animation = GetComponent<Animation>();
	}

	private IEnumerator Deactivate()
	{
		if (fadeOut)
		{
			animation.CrossFade(fadeOut.name);
			yield return new WaitForSeconds(fadeOut.length);
		}
		gameObject.SetActive(Active);
	}
}
