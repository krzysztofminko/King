using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class King : Character
{
	[SerializeField]
	private float goldLoyaltyIncrease = 0.5f;
	[SerializeField][Required]
	private GameObject goldPrefab;
	[SerializeField][Required]
	private GameObject fistPrefab;


	private bool lockActions;

	private Player.Movement movement;
	private Player.Selector selector;

	protected override void Awake()
	{
		base.Awake();
		movement = GetComponent<Player.Movement>();
		selector = GetComponent<Player.Selector>();
	}

	private void Update()
	{
		if (!lockActions)
		{
			if (Input.GetButtonDown("Activate") && selector.selected?.GetComponent<TaskProvider>())
			{
				StartCoroutine(ToggleTaskActivity(selector.selected.GetComponent<TaskProvider>()));
			}
			else
			{
				if (Input.GetButtonDown("Gold"))// && selected?.GetComponent<Subject>())
					StartCoroutine(UseGold(selector.selected?.GetComponent<Subject>()));
				if (Input.GetButtonDown("Fist"))// && selected?.GetComponent<Subject>())
					StartCoroutine(UseFist(selector.selected?.GetComponent<Subject>()));
			}
		}
	}

	#region Actions

	private IEnumerator ToggleTaskActivity(TaskProvider taskProvider)
	{
		lockActions = true;
		//pointTarget = taskProvider.transform;

		animator.Play("activate", 0);
		taskProvider.Active = !taskProvider.Active;

		//pointTarget = null;
		lockActions = false;
		yield break;
	}

	private IEnumerator UseGold(Subject subject = null)
	{
		lockActions = true;
		movement.pointTarget = subject?.transform;

		animator.Play("gold", 0);
		yield return new WaitForSeconds(0.25f);
		GameObject inHand = Instantiate(goldPrefab, rightHandItemParent);
		yield return new WaitForSeconds(0.25f);
		inHand.transform.parent = null;

		movement.pointTarget = null;
		lockActions = false;

		if (subject)
		{
			yield return inHand.GetComponent<Gold>().StartCoroutine(inHand.GetComponent<Gold>().Throw(subject.transform, 2.5f));
			yield return new WaitForSeconds(0.1f);
			subject.loyalty += goldLoyaltyIncrease;
			if (subject.loyalty > 0)
				subject.Motivation++;
		}
		else
		{
			yield return inHand.GetComponent<Gold>().StartCoroutine(inHand.GetComponent<Gold>().Throw(transform.position + transform.forward * 8 + Vector3.up * 0.25f, 2.5f));
		}
	}

	private IEnumerator UseFist(Subject subject = null)
	{
		lockActions = true;
		movement.pointTarget = subject?.transform;

		GameObject inHand = Instantiate(fistPrefab, rightHandItemParent);
		animator.Play("fist", 0);
		yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
		if (subject)
		{
			subject.loyalty--;
			if (subject.loyalty > 0)
				subject.Motivation++;
		}
		else
		{
			List<Subject> subjects = Subject.list.FindAll(s => Distance.Manhattan2D(transform.position, s.transform.position) < 20);
			for (int i = 0; i < subjects.Count; i++)
			{
				subjects[i].loyalty--;
				if (subjects[i].loyalty > 0)
					subjects[i].Motivation++;
			}
		}
		Destroy(inHand);

		movement.pointTarget = null;
		lockActions = false;
	}

	#endregion
}