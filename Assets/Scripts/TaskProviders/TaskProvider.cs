using NodeCanvas.BehaviourTrees;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public abstract class TaskProvider : MonoBehaviour
{
	public static List<TaskProvider> list = new List<TaskProvider>();
	public static List<TaskProvider> listActive = new List<TaskProvider>();

	[SerializeField]
	private bool _active;
	public bool Active
	{
		get => _active;
		set
		{
			if(_active != value)
			{
				_active = value;
				if (value)
					OnActivate();
				else
					OnDeactivate();
			}
		}
	}
	[Tooltip("Only one Subject at a time can use this TaskProvider")]
	public bool oneSubjectOnly = true;
	[ReadOnly]
	public bool isReserved;

	[SerializeField][Required]
	public FadeActivator gear;

	protected virtual void Awake()
	{
		list.Add(this);
	}

	protected virtual void OnDestroy()
	{
		list.Remove(this);
		listActive.Remove(this);
	}

	private void OnActivate()
	{
		gear.transform.rotation = Quaternion.identity;
		gear.Active = true;
		listActive.Add(this);
	}

	private void OnDeactivate()
	{
		gear.Active = false;
		listActive.Remove(this);
	}

}
