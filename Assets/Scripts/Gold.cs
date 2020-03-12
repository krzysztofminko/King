using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gold : MonoBehaviour
{
	public static List<Gold> listPickable = new List<Gold>();

	private new Animation animation;


	private void Awake()
	{
		animation = GetComponent<Animation>();
	}

	private void OnDestroy()
	{
		listPickable.Remove(this);
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
			Vector3 p = Bezier.Sample(startPosition, Vector3.up * Mathf.Max(2, startDistance * 0.5f), target.position, Vector3.down * Mathf.Max(2, startDistance * 0.5f), progress / duration);
			//Debug.DrawLine(transform.position, p, new Color(progress / duration, progress / duration, progress / duration), 10);
			transform.position = p;
			transform.Rotate(transform.forward, 600 * Time.deltaTime);
			yield return null;
		}
		Destroy(gameObject);
	}

	public IEnumerator Throw(Vector3 targetPosition, float speed = 3)
	{
		animation.Stop();
		Vector3 startPosition = transform.position;
		float startDistance = (targetPosition - startPosition).magnitude;
		float progress = 0;
		float duration = startDistance / speed;
		while (progress < duration)
		{
			progress += speed * Time.deltaTime;
			Vector3 p = Bezier.Sample(startPosition, Vector3.up * Mathf.Max(2, startDistance * 0.5f), targetPosition, Vector3.down * Mathf.Max(2, startDistance * 0.5f), progress / duration);
			//Debug.DrawLine(transform.position, p, new Color(progress / duration, progress / duration, progress / duration), 10);
			transform.position = p;
			transform.Rotate(transform.forward, 360 * speed * Time.deltaTime);
			yield return null;
		}
		listPickable.Add(this);
	}
}
