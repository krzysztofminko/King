using NodeCanvas.BehaviourTrees;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Subject : Character
{
	public static List<Subject> list = new List<Subject>();

	public float loyalty;
	[SerializeField][Min(0)]
	private int _motivation;
	public int Motivation
	{
		get => _motivation;
		set
		{
			if(_motivation != value)
			{
				if (value > _motivation && loyalty > 0)
					StartCoroutine(AddSpeedBoost(5));
				_motivation = value;
			}
		}
	}
	[SerializeField]
	private int _speedBoost;
	public int SpeedBoost
	{
		get => _speedBoost;
		set
		{
			if(_speedBoost != value)
			{
				_speedBoost = value;
				if (_speedBoost < 0)
					_speedBoost = 0;
			}
		}
	}
	[SerializeField]
	private float speedPushForce = 1;

	[SerializeField][Required]
	private FadeActivator infoCloud;

	[ShowInInspector][ReadOnly]
	public Resource Resource { get; private set; }
	[ShowInInspector][ReadOnly]
	public TaskProvider TaskProvider { get; private set; }

	public BehaviourTreeOwner BTOwner { get; private set; }

	
	private void Awake()
	{
		list.Add(this);
		BTOwner = GetComponent<BehaviourTreeOwner>();
	}

	private void OnDestroy()
	{
		list.Remove(this);
	}
	
	private IEnumerator AddSpeedBoost(float duration)
	{
		SpeedBoost++;
		animator.SetFloat("actionSpeed", 2);
		yield return new WaitForSeconds(duration);
		animator.SetFloat("actionSpeed", 1);
		SpeedBoost--;
	}


	public bool GoTo(Vector3 position, float minDistance)
	{
		position = new Vector3(position.x, transform.position.y, position.z);
		if ((position - transform.position).sqrMagnitude < minDistance * minDistance)
		{
			StopMovement();
			return true;
		}

		if(SpeedBoost > 0)
		{
			if (runParticle.isStopped)
				runParticle.Play();
			Collider[] colliders = Physics.OverlapBox(transform.position + transform.forward * 0.5f, new Vector3(1, 0.5f, 1), transform.rotation);
			for (int i = 0; i < colliders.Length; i++)
			{
				if(colliders[i].gameObject != gameObject)
					colliders[i].GetComponent<Subject>()?.GetComponent<Rigidbody>().AddForce((colliders[i].transform.position - transform.position).normalized * speedPushForce, ForceMode.Impulse);
			}
		}
		else
		{
			if (runParticle.isPlaying)
				runParticle.Stop();
		}

		float speed = SpeedBoost > 0 ? runSpeed : walkSpeed;
		Vector3 movement = (position - transform.position).normalized * speed * Time.deltaTime;
		if (movement.sqrMagnitude > 0)
			transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(movement, Vector3.up), 360 * Time.deltaTime);
		transform.position += movement;
		animator.SetFloat("speed", speed);
		return false;
	}

	public void StopMovement()
	{
		animator.SetFloat("speed", 0);
		if (runParticle.isPlaying)
			runParticle.Stop();
	}

	public bool AssignTaskProvider(TaskProvider provider)
	{
		if (provider)
		{
			if (provider.isReserved)
				return false;
			if (TaskProvider != null)
				AssignTaskProvider(null);
			if (provider.oneSubjectOnly)
				provider.isReserved = true;
			TaskProvider = provider;
			if (Motivation > 0)
				Motivation--;
		}
		else if (TaskProvider != null)
		{
			TaskProvider.isReserved = false;
			TaskProvider = null;
		}
		return true;
	}

	public void ShowInfo()
	{
		infoCloud.Active = true;
	}

	public void HideInfo()
	{
		infoCloud.Active = false;
	}
}
