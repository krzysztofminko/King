using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
	public class Movement : MonoBehaviour
	{
		[SerializeField]
		private float walkSpeed = 1;
		[SerializeField]
		private float runSpeed = 2;
		[SerializeField][Required]
		private ParticleSystem runParticle;

		[ReadOnly]
		public Transform pointTarget;
		[ReadOnly]
		public bool lockRotation;  //Lock movement rotation

		private Animator animator;

		private void Awake()
		{
			animator = GetComponent<Animator>();
		}

		private void Update()
		{
			if (Input.GetButton("Run"))
			{
				if (runParticle.isStopped)
					runParticle.Play();
			}
			else
			{
				if (runParticle.isPlaying)
					runParticle.Stop();
			}
			float speed = Input.GetButton("Run") ? runSpeed : walkSpeed;
			Vector3 movement = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")) * speed;// * Time.deltaTime;
			if (movement.sqrMagnitude > 0)
			{
				if (!lockRotation)
				{
					if (pointTarget)
						transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(pointTarget.position - transform.position), 360 * Time.deltaTime);
					else
						transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(movement), 360 * Time.deltaTime);
				}
			}
			else
			{
				speed = 0;
			}
			//transform.position += movement;
			GetComponent<CharacterController>().SimpleMove(movement);
			animator.SetFloat("speed", speed);
		}
	}
}