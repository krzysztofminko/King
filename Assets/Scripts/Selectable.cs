using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class Selectable : MonoBehaviour
{
	[SerializeField][ReadOnly]
	private bool _selected;
    public bool Selected
	{
		get => _selected;
		set
		{
			if(_selected != value)
			{
				_selected = value;
				if (selectionEffect)
					selectionEffect.SetActive(value);
				if (value)
					onSelect?.Invoke();
				else
					onUnselect?.Invoke();
			}
		}
	}

	[SerializeField]
	private GameObject selectionEffect;
	[SerializeField][Range(-1, 1)][Tooltip("Adds value to color on selection.")]
	private float colorValueChange;

	private UnityEvent onSelect;
	private UnityEvent onUnselect;

	private Dictionary<Renderer, List<Material>> sharedMaterials = new Dictionary<Renderer, List<Material>>();

	private void Awake()
	{
		Renderer[] renderers = GetComponentsInChildren<Renderer>();
		for (int r = 0; r < renderers.Length; r++)
			sharedMaterials.Add(renderers[r], renderers[r].sharedMaterials.ToList());

		if (colorValueChange != 0)
		{
			onSelect.AddListener(
				delegate
				{
					StopAllCoroutines();
					foreach (var r in sharedMaterials)
						for (int m = 0; m < r.Value.Count; m++)
							StartCoroutine(ChangeColorValue(r.Key.materials[m], colorValueChange, colorValueChange * 4));
				});
			onUnselect.AddListener(
				delegate
				{
					StopAllCoroutines();
					foreach (var r in sharedMaterials)
						for (int m = 0; m < r.Value.Count; m++)
							StartCoroutine(ChangeColorValue(r.Key.materials[m], -colorValueChange, colorValueChange * 4, r.Key));
				});
		}
	}

	private IEnumerator ChangeColorValue(Material material, float changeValue, float speed, Renderer renderer = null)
	{
		Color.RGBToHSV(material.color, out float h, out float s, out float v);
		float targetV = v + changeValue;
		while (v != targetV)
		{
			v = v < targetV ? Mathf.Min(targetV, v + speed * Time.deltaTime) : Mathf.Max(targetV, v - speed * Time.deltaTime);
			material.color = Color.HSVToRGB(h, s, v);
			yield return null;
		}
		if (renderer)
			renderer.materials = sharedMaterials[renderer].ToArray();
	}
}
