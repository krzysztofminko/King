using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gold : MonoBehaviour
{
	private new Animation animation;

	private void Awake()
	{
		animation = GetComponent<Animation>();
	}

	public IEnumerator Throw(Transform target, float speed = 3)
	{
		animation.Stop();
		Vector3 startPosition = transform.position;
		float startDistance = (target.position - startPosition).magnitude;
		float progress = 0;
		float duration = startDistance / speed;
		while (progress < duration)
		{
			progress += speed * Time.deltaTime;
			Vector3 p = Bezier.Sample(startPosition, Vector3.up * startDistance * 0.5f, target.position, Vector3.down * startDistance * 0.5f, progress / duration);
			//Debug.DrawLine(transform.position, p, new Color(progress / duration, progress / duration, progress / duration), 10);
			transform.position = p;
			transform.Rotate(transform.forward, -600 * Time.deltaTime);
			yield return null;
		}
		Destroy(gameObject);
	}
}
