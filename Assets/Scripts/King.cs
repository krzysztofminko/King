using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class King : MonoBehaviour
{
	[SerializeField]
	private float walkSpeed = 1;
	[SerializeField]
	private float runSpeed = 2;

	private void Update()
	{
		float speed = Input.GetButton("Run") ? runSpeed : walkSpeed;
		Vector3 movement = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")) * speed * Time.deltaTime;
		if (movement.sqrMagnitude > 0)
			transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(movement, Vector3.up), 360 * Time.deltaTime);
		transform.position += movement;
	}
}
