using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
	private Bounds bounds;

	private void Start()
	{
		bounds = GetComponent<Collider>().bounds;

		ObstacleMap.instance.Inc(new Rect(new Vector2(bounds.min.x, bounds.min.z), new Vector2(bounds.size.x, bounds.size.z)));
	}

	private void OnDestroy()
	{
		if (ObstacleMap.instance)
			ObstacleMap.instance.Dec(new Rect(new Vector2(bounds.min.x, bounds.min.z), new Vector2(bounds.size.x, bounds.size.z)));
	}
}
