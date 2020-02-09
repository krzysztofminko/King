using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class King : Character
{
	public static King instance;

	[SerializeField][Required]
	private Resource gold;

	[SerializeField][Required]
	private GameObject goldPrefab;
	[SerializeField][Required]
	private GameObject fistPrefab;
	[SerializeField][ReadOnly]
	private Selectable selected;

	private Transform pointTarget;

	private bool lockActions;	//Lock input triggered actions except movement
	private bool lockRotation;	//Lock movement rotation

	private void Awake()
	{
		instance = this;
	}

	#region Update
	private void Update()
	{
		Movement();

		Selection();

		Actions();

		if (Input.GetKeyDown(KeyCode.Space))
			foreach (var r in Resource.list)
				Debug.Log($"{r.Key}: {r.Value}");
	}

	private void Movement()
	{
		float speed = Input.GetButton("Run") ? runSpeed : walkSpeed;
		Vector3 movement = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")) * speed * Time.deltaTime;
		if (movement.sqrMagnitude > 0)
		{
			if (!lockRotation)
			{
				if (pointTarget)
					transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(pointTarget.position - transform.position), 360 * Time.deltaTime);
				else
					transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(movement), 360 * Time.deltaTime);
			}
		}
		else
		{
			speed = 0;
		}
		transform.position += movement;
		animator.SetFloat("speed", speed);
	}

	private void Selection()
	{
		Selectable newSelected = Physics.OverlapSphere(transform.position + transform.forward, 4).ToList().FindAll(c => c.GetComponent<Selectable>()).OrderBy(c => (c.transform.position - transform.position).sqrMagnitude).FirstOrDefault()?.GetComponent<Selectable>();
		if (selected && selected != newSelected)
			selected.Selected = false;
		selected = newSelected;
		if (selected)
			selected.Selected = true;
	}

	private void Actions()
	{
		if (!lockActions)
		{
			if (Input.GetButtonDown("Activate") && selected?.GetComponent<TaskProvider>())
				StartCoroutine(ToggleTaskActivity(selected.GetComponent<TaskProvider>()));
			if (Input.GetButtonDown("Gold") && selected?.GetComponent<Subject>())
				StartCoroutine(UseGold(selected.GetComponent<Subject>()));
			if (Input.GetButtonDown("Fist") && selected?.GetComponent<Subject>())
				StartCoroutine(UseFist(selected.GetComponent<Subject>()));
		}
	}
	#endregion

	#region Actions

	private IEnumerator ToggleTaskActivity(TaskProvider taskProvider)
	{
		lockActions = true;
		pointTarget = taskProvider.transform;

		yield return StartCoroutine(PlayAnimation("activate"));
		taskProvider.active = !taskProvider.active;

		pointTarget = null;
		lockActions = false;
	}

	private IEnumerator UseGold(Subject subject)
	{
		lockActions = true;
		pointTarget = subject.transform;

		StartCoroutine(PlayAnimation("gold"));
		yield return new WaitForSeconds(0.25f);
		GameObject inHand = Instantiate(goldPrefab, rightHandItemParent);
		yield return new WaitForSeconds(0.25f);
		inHand.transform.parent = null;

		pointTarget = null;
		lockActions = false;

		yield return inHand.GetComponent<Gold>().StartCoroutine(inHand.GetComponent<Gold>().Throw(subject.transform));
		subject.goldSurplus++;
		subject.loyalty++;
	}

	private IEnumerator UseFist(Subject subject)
	{
		lockActions = true;
		pointTarget = subject.transform;

		GameObject inHand = Instantiate(fistPrefab, rightHandItemParent);
		yield return StartCoroutine(PlayAnimation("fist"));
		subject.fistSurplus++;
		subject.loyalty--;
		Destroy(inHand);

		pointTarget = null;
		lockActions = false;
	}

	#endregion

}
