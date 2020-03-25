using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Resource : ScriptableObject
{
	public static Dictionary<Resource, int> count = new Dictionary<Resource, int>();

	public int startCount;
	public GameObject model;

	private void OnEnable()
	{
		count.Add(this, startCount);
	}

	private void OnDisable()
	{
		count.Remove(this);
	}
}
