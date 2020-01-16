using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPosition : MonoBehaviour
{
	public Transform follow;

	private Vector3? offset;

	private void Update()
	{
		if (offset == null)
			offset = follow ? transform.position - follow.position : Vector3.zero;
		if (follow)
			transform.position = follow.position + offset.Value;
	}
}
