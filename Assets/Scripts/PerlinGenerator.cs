using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PerlinGenerator : MonoBehaviour
{
	[System.Serializable]
	public class PerlinLayer
	{
		public bool active;
		[Range(0.00001f, 1.0f)]
		public float scale;
		public Vector2 offset;
		[Range(0.0f, 1.0f)]
		public float weight;
		[Range(-1.0f, 1.0f)]
		public float contrast;
	}

	[OnValueChanged("Generate")]
	public bool generateInEditMode;
	[OnValueChanged("Generate")]
	public Vector3Int mapSize = new Vector3Int(100, 10, 100);
	[OnValueChanged("Generate", true)]
	public List<PerlinLayer> perlinLayers = new List<PerlinLayer>();

	[PreviewField(200, ObjectFieldAlignment.Center)][ReadOnly][ShowInInspector]
	private Texture2D mapTexture;

	private float[,] map;

	private TerracedTerrain.Terrain terrain;

	private void OnValidate()
	{
		mapSize = Vector3Int.Max(Vector3Int.one * 1, mapSize);
		terrain = GetComponent<TerracedTerrain.Terrain>();
	}

	private void Reset()
	{
		terrain = GetComponent<TerracedTerrain.Terrain>();
	}

	private void Start()
	{
		terrain = GetComponent<TerracedTerrain.Terrain>();
		Generate();
	}

	public void Generate()
	{
		if (generateInEditMode)
		{
			StartCoroutine(GenerateCoroutine(mapSize));
		}
	}

	[OnValueChanged("Generate")]
	[Range(-1.0f, 1.0f)]
	public float contrast;

	public IEnumerator GenerateCoroutine(Vector3Int mapSize)
	{
		//Generate heightmap
		mapTexture = new Texture2D(mapSize.x, mapSize.z);
		float [,] map = new float[mapSize.x, mapSize.z];

		for (int z = 0; z < mapSize.z; z++)
			for (int x = 0; x < mapSize.x; x++)
			{
				float h = 0;
				float weightSum = perlinLayers.FindAll(l => l.active).Sum(l => l.weight);
				for (int i = 0; i < perlinLayers.Count; i++)
					if (perlinLayers[i].active)
					{
						float p = Mathf.PerlinNoise(x * perlinLayers[i].scale + perlinLayers[i].offset.x, z * perlinLayers[i].scale + perlinLayers[i].offset.y) * (perlinLayers[i].weight / weightSum);
						float f = (perlinLayers[i].contrast + 1.0f) / (1.0f - perlinLayers[i].contrast);
						p = Mathf.Clamp(f * (p - 0.5f) + 0.5f, 0.0f, 0.99f);
						h += p;
					}
				float factor = (contrast + 1.0f) / (1.0f - contrast);
				h = Mathf.Clamp(factor * (h - 0.5f) + 0.5f, 0.0f, 0.99f);
				map[x, z] = h;
				mapTexture.SetPixel(x, z, new Color(h, h, h));
			}
		mapTexture.Apply();

		this.map = map;
		this.mapSize = mapSize;

		yield return StartCoroutine(terrain.GenerateCoroutine(mapSize.y, map));
	}
}
