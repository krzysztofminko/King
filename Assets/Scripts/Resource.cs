using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Resource : ScriptableObject
{
	public static Dictionary<Resource, int> list = new Dictionary<Resource, int>();

	public int startCount;
	public GameObject model;

	private void OnEnable()
	{
		list.Add(this, startCount);
	}

	private void OnDisable()
	{
		list.Remove(this);
	}
}
