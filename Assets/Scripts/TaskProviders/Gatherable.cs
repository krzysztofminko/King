using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gatherable : TaskProvider
{
	public Resource resource;
	[Min(1)]
	public int count = 1;
	public new string animation;
	public float duration;
}
