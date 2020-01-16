using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Task : MonoBehaviour
{
	//public List<Subject> subjects;	<-- uncomment when Subject implemented
	public int value;

	public abstract bool Execute();
}
