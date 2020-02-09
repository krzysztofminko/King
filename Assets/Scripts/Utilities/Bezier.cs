using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Bezier 
{
	public static Vector3 Sample(Vector3 startPoint, Vector3 startModifier, Vector3 endPoint, Vector3 endModifier, float t)
	{
		Vector3 p0 = startPoint;
		Vector3 p1 = p0 + startModifier;
		Vector3 p3 = endPoint;
		Vector3 p2 = p3 - endModifier;

		return Mathf.Pow(1f - t, 3f) * p0 + 3f * Mathf.Pow(1f - t, 2f) * t * p1 + 3f * (1f - t) * Mathf.Pow(t, 2f) * p2 + Mathf.Pow(t, 3f) * p3;
	}
}
