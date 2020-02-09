using NodeCanvas.BehaviourTrees;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TaskProvider : MonoBehaviour
{
	public static List<TaskProvider> list = new List<TaskProvider>();

	public bool active;
	[Tooltip("Only one Subject at a time can use this TaskProvider")]
	public bool oneSubjectOnly = true;
	[ReadOnly]
	public bool isReserved;

	protected virtual void Awake()
	{
		list.Add(this);
	}

	protected virtual void OnDestroy()
	{
		list.Remove(this);
	}
}
