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
				selectionEffect?.SetActive(value);
				if (value)
					onSelect?.Invoke();
				else
					onUnselect?.Invoke();
			}
		}
	}

	[SerializeField]
	private GameObject selectionEffect;

	public UnityEvent onSelect;
	public UnityEvent onUnselect;


}
