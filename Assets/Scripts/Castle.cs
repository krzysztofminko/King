using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Castle : MonoBehaviour
{
	public static Castle instance;

	[Required]
	public Transform housePrefab;

	private Structure structure;
	
	private void Awake()
	{
		instance = this;
		structure = GetComponent<Structure>();

		StartCoroutine(GrowTown());
	}

	private IEnumerator GrowTown()
	{
		while (true)
		{
			yield return new WaitForSeconds(1);

			Vector2? position = ObstacleMap.instance.GetNearestPosition(0);
			if (position != null)
				Instantiate(housePrefab, new Vector3(position.Value.x + Random.Range(-ObstacleMap.instance.tileSize.x * 0.125f, ObstacleMap.instance.tileSize.x * 0.125f), 0, position.Value.y + Random.Range(-ObstacleMap.instance.tileSize.y * 0.125f, ObstacleMap.instance.tileSize.y * 0.125f)), Quaternion.Euler(0,Random.Range(-45,45),0));
		}
	}

}
