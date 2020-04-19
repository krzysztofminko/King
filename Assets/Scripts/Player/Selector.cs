using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Player
{
	public class Selector : MonoBehaviour
	{
		[SerializeField][Required]
		private SphereCollider selectorShape;
		[SerializeField][Required]
		private GameObject selectionBox;
		[ReadOnly]
		public Selectable selected;

		private void Update()
		{
			Selectable newSelected = Physics.OverlapSphere(transform.position + transform.rotation * selectorShape.center, selectorShape.radius).ToList().FindAll(c => c.GetComponent<Selectable>()).OrderBy(c => (c.transform.position - transform.position).sqrMagnitude).FirstOrDefault()?.GetComponent<Selectable>();
			if (selected && selected != newSelected)
			{
				selected.Selected = false;
				selectionBox.SetActive(false);
			}
			selected = newSelected;
			if (selected)
			{
				selected.Selected = true;
				selectionBox.SetActive(true);
				selectionBox.transform.position = selected.transform.position;
				selectionBox.transform.rotation = selected.transform.rotation;
			}
		}
	}
}