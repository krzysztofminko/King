using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Castle : MonoBehaviour
{
	public static Castle instance;


	private Structure structure;
	
	private void Awake()
	{
		instance = this;
		structure = GetComponent<Structure>();

		StartCoroutine(GrowTown());
	}

	private IEnumerator GrowTown()
	{
		yield return new WaitForSeconds(2);

		for (int radius = 0; radius < Mathf.Max(ObstacleMap.instance.size.x, ObstacleMap.instance.size.y); radius++)
		{

		}
	}

}
