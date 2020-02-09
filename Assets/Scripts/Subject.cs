using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Subject : Character
{
	public static List<Subject> list = new List<Subject>();
	
	public float loyalty;
	[Min(0)]
	public int goldSurplus;
	[Min(0)]
	public int fistSurplus;
	[ShowInInspector]
	public Resource Resource { get; private set; }
	[ShowInInspector]
	public TaskProvider TaskProvider { get; private set; }

	
	private void Awake()
	{
		list.Add(this);
	}

	private void OnDestroy()
	{
		list.Remove(this);
	}

	
	public bool GoTo(Vector3 position, float minDistance)
	{
		if ((position - transform.position).sqrMagnitude < minDistance * minDistance)
		{
			animator.SetFloat("speed", 0);
			return true;
		}

		float speed = runSpeed; //... ? runSpeed : walkSpeed;
		Vector3 movement = (position - transform.position).normalized * speed * Time.deltaTime;
		if (movement.sqrMagnitude > 0)
			transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(movement, Vector3.up), 360 * Time.deltaTime);
		transform.position += movement;
		animator.SetFloat("speed", speed);
		return false;
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
			if (goldSurplus > 0)
				goldSurplus--;
			else if (fistSurplus > 0)
				fistSurplus--;
		}
		else if (TaskProvider != null)
		{
			TaskProvider.isReserved = false;
			TaskProvider = null;
		}
		return true;
	}
}
