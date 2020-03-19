using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class WorldGeneratorSettings : ScriptableObject
{
	[System.Serializable]
	public class PerlinLayer
	{
		public bool active = true;
		[Range(0.00001f, 1.0f)]
		public float scale = 0.04f;
		public Vector2 offset;
		[Range(0.0f, 1.0f)]
		public float weight = 1;
	}

	public float scale = 1;
	public AnimationCurve fallOffCurve = AnimationCurve.Constant(0, 1, 1);
	public List<PerlinLayer> perlinLayers = new List<PerlinLayer>();




}
