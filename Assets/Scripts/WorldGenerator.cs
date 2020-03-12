using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WorldGenerator : MonoBehaviour
{
	[System.Serializable]
	public struct Layer
	{
		public bool active;
		[Range(0.00001f, 1.0f)]
		public float scale;
		public Vector2 offset;
		[Range(0.0f, 1.0f)]
		public float weight;
	}

	[OnValueChanged("Generate")]
	public bool generateInEditMode;
	[OnValueChanged("Generate")]
	public Vector3Int mapSize = new Vector3Int(100, 10, 100);
	[OnValueChanged("Generate", true)]
	public List<Layer> layers = new List<Layer>();

	[PreviewField(200, ObjectFieldAlignment.Center)][ReadOnly][ShowInInspector]
	private Texture2D mapTexture;

	private float[,] map;

	private TerracedTerrain.Terrain terrain;

	private void OnValidate()
	{
		mapSize = Vector3Int.Max(Vector3Int.one * 2, mapSize);
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


	public IEnumerator GenerateCoroutine(Vector3Int mapSize)
	{
		//Generate heightmap
		mapTexture = new Texture2D(mapSize.x, mapSize.z);
		float [,] map = new float[mapSize.x, mapSize.z];

		for (int z = 0; z < mapSize.z; z++)
			for (int x = 0; x < mapSize.x; x++)
			{
				float h = 0;
				float weightSum = layers.FindAll(l => l.active).Sum(l => l.weight);
				for (int i = 0; i < layers.Count; i++)
					if (layers[i].active)
						h += Mathf.PerlinNoise(x * layers[i].scale + layers[i].offset.x, z * layers[i].scale + layers[i].offset.y) * (layers[i].weight/weightSum);
				//h += Mathf.PerlinNoise(x * 0.4f, z * 0.4f) * 0.1f;
				//h += Mathf.PerlinNoise(x * 0.2f, z * 0.2f) * 0.3f;
				//h += Mathf.PerlinNoise(x * 0.1f, z * 0.1f) * 0.6f;
				map[x, z] = h;
				mapTexture.SetPixel(x, z, new Color(h, h, h));
			}
		mapTexture.Apply();

		this.map = map;
		this.mapSize = mapSize;

		yield return StartCoroutine(terrain.GenerateCoroutine(mapSize.y, map));
	}
}
