using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
	public class GoldCollector : MonoBehaviour
	{
		[SerializeField]
		[Required]
		private Resource gold;

		private void Update()
		{
			Collider[] colliders = Physics.OverlapSphere(transform.position, 1, LayerMask.GetMask("Gold"));
			for (int i = 0; i < colliders.Length; i++)
			{
				Gold g = colliders[i].GetComponent<Gold>();
				if (g && g.playerOnly)
				{
					Destroy(g.gameObject);
					Resource.count[gold]++;
				}
			}
		}
	}
}