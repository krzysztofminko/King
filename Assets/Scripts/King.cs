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
	[SerializeField][Required]
	private SphereCollider selectorShape;
	[SerializeField][Required]
	private GameObject selectionBox;
	[SerializeField][ReadOnly]
	private Selectable selected;

	private Transform pointTarget;

	private bool lockActions;	//Lock input triggered actions except movement
	private bool lockRotation;  //Lock movement rotation
	
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

		PickupGold();

		if (Input.GetKeyDown(KeyCode.Space))
			foreach (var r in Resource.count)
				Debug.Log($"{r.Key}: {r.Value}");
	}

	private void Movement()
	{
		if (Input.GetButton("Run"))
		{
			if (runParticle.isStopped)
				runParticle.Play();
		}
		else
		{
			if (runParticle.isPlaying)
				runParticle.Stop();
		}
		float speed = Input.GetButton("Run") ? runSpeed : walkSpeed;
		Vector3 movement = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")) * speed;// * Time.deltaTime;
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
		//transform.position += movement;
		GetComponent<CharacterController>().SimpleMove(movement);
		animator.SetFloat("speed", speed);
	}

	private void Selection()
	{
		Selectable newSelected = Physics.OverlapSphere(transform.position + transform.rotation * selectorShape.center, selectorShape.radius).ToList().FindAll(c => c.GetComponent<Selectable>()).OrderBy(c => (c.transform.position - transform.position).sqrMagnitude).FirstOrDefault()?.GetComponent<Selectable>();
		if (selected && selected != newSelected)
		{
			selected.Selected = false;
			selectionBox.SetActive(false);
		}
		selected = newSelected;
		if (selected)
		{
			selected.Selected = true;
			selectionBox.SetActive(true);
			selectionBox.transform.position = selected.transform.position;
			selectionBox.transform.rotation = selected.transform.rotation;
		}
	}

	private void Actions()
	{
		if (!lockActions)
		{
			if (Input.GetButtonDown("Activate") && selected?.GetComponent<TaskProvider>())
			{
				StartCoroutine(ToggleTaskActivity(selected.GetComponent<TaskProvider>()));
			}
			else
			{
				if (Input.GetButtonDown("Gold"))// && selected?.GetComponent<Subject>())
					StartCoroutine(UseGold(selected?.GetComponent<Subject>()));
				if (Input.GetButtonDown("Fist"))// && selected?.GetComponent<Subject>())
					StartCoroutine(UseFist(selected?.GetComponent<Subject>()));
			}
		}
	}

	private void PickupGold()
	{
		Collider[] colliders = Physics.OverlapSphere(transform.position, 1, LayerMask.GetMask("Gold"));
		for (int i = 0; i < colliders.Length; i++)
		{
			Gold g = colliders[i].GetComponent<Gold>();
			if (g && g.playerOnly)
			{
				Destroy(g.gameObject);
				Resource.count[gold]++;
			}
		}
	}

	#endregion

	#region Actions

	private IEnumerator ToggleTaskActivity(TaskProvider taskProvider)
	{
		lockActions = true;
		//pointTarget = taskProvider.transform;

		StartCoroutine(PlayAnimation("activate"));
		taskProvider.Active = !taskProvider.Active;

		//pointTarget = null;
		lockActions = false;
		yield break;
	}

	private IEnumerator UseGold(Subject subject = null)
	{
		lockActions = true;
		pointTarget = subject?.transform;

		StartCoroutine(PlayAnimation("gold"));
		yield return new WaitForSeconds(0.25f);
		GameObject inHand = Instantiate(goldPrefab, rightHandItemParent);
		yield return new WaitForSeconds(0.25f);
		inHand.transform.parent = null;

		pointTarget = null;
		lockActions = false;

		if (subject)
		{
			yield return inHand.GetComponent<Gold>().StartCoroutine(inHand.GetComponent<Gold>().Throw(subject.transform, 2.5f));
			yield return new WaitForSeconds(0.1f);			
			subject.loyalty++;
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
		pointTarget = subject?.transform;

		GameObject inHand = Instantiate(fistPrefab, rightHandItemParent);
		yield return StartCoroutine(PlayAnimation("fist"));
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

		pointTarget = null;
		lockActions = false;
	}

	#endregion

}
