using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
	private Bounds bounds;

	private void Start()
	{
		bounds = GetComponent<Collider>().bounds;

		for (int x = 0; x < Mathf.CeilToInt(bounds.size.x); x++)
			for (int z = 0; z < Mathf.CeilToInt(bounds.size.z); z++)
				ObstacleMap.instance.Inc(new Vector2((transform.position.x + bounds.center.x - bounds.extents.x) + x, (transform.position.z + bounds.center.z - bounds.extents.z) + z));

		ObstacleMap.instance.UpdateTexture();
	}
}
