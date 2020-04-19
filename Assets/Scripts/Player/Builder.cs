using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Player
{
	public class Builder : MonoBehaviour
	{
		public List<Structure> structures;

		[ShowInInspector][ReadOnly]
		private bool _structureSelection;
		private bool StructureSelection
		{
			get => _structureSelection;
			set
			{
				if (_structureSelection != value)
				{
					_structureSelection = value;
					if (_structureSelection)
						selectedPreview = Instantiate(structures[SelectedStructureId].preview);
					else
						Destroy(selectedPreview.gameObject);
				}
			}
		}

		[ShowInInspector][ReadOnly]
		private int _selectedStructureId;
		private int SelectedStructureId
		{
			get => _selectedStructureId;
			set
			{
				if (_selectedStructureId != value)
				{
					Destroy(selectedPreview.gameObject);
					_selectedStructureId = value;
					selectedPreview = Instantiate(structures[_selectedStructureId].preview);
				}
			}
		}

		[ShowInInspector][ReadOnly]
		private Transform selectedPreview;

		private void Update()
		{
			if (Input.GetButtonDown("Build"))
				StructureSelection = !StructureSelection;

			if (StructureSelection)
			{
				SelectedStructureId = Mathf.Clamp(SelectedStructureId + (int)(Input.mouseScrollDelta.y * 10), 0, structures.Count - 1);

				selectedPreview.Rotate(Vector3.up, Input.mouseScrollDelta.y * 360 * Time.deltaTime);

				if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit hitInfo, 1000, LayerMask.GetMask("Terrain")))
				{
					selectedPreview.transform.position = hitInfo.point;
					if (Input.GetMouseButtonDown(0))
					{
						Instantiate(structures[SelectedStructureId], selectedPreview.position, selectedPreview.rotation);
						StructureSelection = false;
					}
				}

				if (Input.GetMouseButtonDown(1))
					StructureSelection = false;
			}
		}
	}
}