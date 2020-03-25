using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WorldGenerator : MonoBehaviour
{
	[OnValueChanged("Validate")]
	public bool generateInEditMode;
	[OnValueChanged("Validate")]
	public Vector3Int mapSize = new Vector3Int(100, 0, 100);
	[OnValueChanged("Validate", true)][InlineEditor(InlineEditorModes.GUIOnly)]
	public WorldGeneratorSettings settings;

	[PreviewField(200, ObjectFieldAlignment.Center)][ReadOnly]
	public Texture2D mapTexture;
	public Transform treePrefab;

	[System.Serializable]
	public class Region
	{
		public Color color = Color.black;
		public Vector2 position;
		public List<Region> children = new List<Region>();
	}

	[OnValueChanged("Validate")][Min(1)]
	public int regionsMax;
	public List<Region> regions;

	[SerializeField][HideInInspector]
	private bool[,] _map;
	public bool[,] Map { get => _map; private set => _map = value; }
	private BitmaskedTerrain.Terrain terrain;
	
	private void Validate()
	{
		terrain = GetComponent<BitmaskedTerrain.Terrain>();
		mapSize = Vector3Int.Max(Vector3Int.one * 1, mapSize);
		if (gameObject.activeInHierarchy)
		{
			if (settings && generateInEditMode)
			{
				StartCoroutine(GenerateCoroutine());
			}
			else
			{
				terrain.Clear();
				regions.Clear();
			}
		}
	}

	private void Awake()
	{
		terrain = GetComponent<BitmaskedTerrain.Terrain>();
		StartCoroutine(GenerateCoroutine());
	}
	
	public IEnumerator GenerateCoroutine()
	{
		//Generate heightmap
		mapTexture = new Texture2D(mapSize.x, mapSize.z);
		Map = new bool[mapSize.x, mapSize.z];
		
		for (int z = 0; z < mapSize.z; z++)
			for (int x = 0; x < mapSize.x; x++)
			{
				float h = 0;
				float weightSum = Mathf.Max(1.0f, settings.perlinLayers.FindAll(l => l.active).Sum(l => l.weight));
				for (int i = 0; i < settings.perlinLayers.Count; i++)
					if (settings.perlinLayers[i].active)
						h += Mathf.PerlinNoise(x * settings.perlinLayers[i].scale * settings.scale + settings.perlinLayers[i].offset.x, z * settings.perlinLayers[i].scale * settings.scale + settings.perlinLayers[i].offset.y) * (settings.perlinLayers[i].weight / weightSum);

				float horizontal = Mathf.Abs(mapSize.x * 0.5f - x) / (mapSize.x * 0.5f);
				float vertical = Mathf.Abs(mapSize.z * 0.5f - z) / (mapSize.z * 0.5f);
				h *= settings.fallOffCurve.Evaluate(horizontal > vertical ? horizontal : vertical);

				h = h < 0.5f ? 0 : 1;
				Map[x, z] = h > 0;
				mapTexture.SetPixel(x, z, new Color(h, h, h));
			}			
			
			/*
		//Randomize regions
		regions.Clear();
		Region centralRegion = null;
		float smallestDistance = Mathf.Infinity;
		for (int i = 0; i < regionsMax; i++)
		{
			float v = Random.Range(0.0f, 1.0f);
			Region region = new Region { position = new Vector2(Random.Range(0, mapSize.x), Random.Range(0, mapSize.z)) };
			float distance = (region.position - new Vector2(mapSize.x * 0.5f, mapSize.z * 0.5f)).sqrMagnitude;
			if (distance < smallestDistance)
			{
				smallestDistance = distance;
				centralRegion = region;
			}
			regions.Add(region);
		}

		Queue<Region> open = new Queue<Region>();
		open.Enqueue(centralRegion);
		List<Region> closed = new List<Region>();

		int safetyCounter = 40;
		while(open.Count > 0 && safetyCounter > 0)
		{
			safetyCounter--;

			Region region = open.Dequeue();
			float v = 1.0f - 0.5f / safetyCounter;// Random.Range(0.5f, 1.0f);
			region.color = new Color(v, Random.Range(0.5f, 1.0f), Random.Range(0.5f, 1.0f));
			float count = Random.Range(1.0f, 1.0f);// * (1.0f - (region.position - centralRegion.position).magnitude / new Vector2(mapSize.x, mapSize.z).magnitude);
			for (int c = 0; c < count; c++)
			{
				Region child = null;
				smallestDistance = Mathf.Infinity;
				for (int r = 0; r < regions.Count; r++)
				{
					if (regions[r] != region && !closed.Contains(regions[r]) && !open.Contains(regions[r]))
					{
						float distance = (region.position - regions[r].position).sqrMagnitude;
						if (distance < smallestDistance)
						{
							smallestDistance = distance;
							child = regions[r];
						}
					}
				}
				if(child != null)
				{
					region.children.Add(child);
					open.Enqueue(child);
				}				
			}
			closed.Add(region);
		}

		centralRegion.color = Color.red;



		//Draw regions
		for (int x = 0; x < mapSize.x; x++)
			for (int z = 0; z < mapSize.z; z++)
			{
				Region closestRegion = GetClosestRegion(new Vector2(x, z));
				if (closestRegion != null)
				{
					Map[x, z] = closestRegion.color.r > 0;
					mapTexture.SetPixel(x, z, closestRegion.color);
				}
			}*/

		mapTexture.Apply();

		//Generate meshes
		yield return StartCoroutine(GetComponent<BitmaskedTerrain.Terrain>().LoadCoroutine(Map));
	}

	private Region GetClosestRegion(Vector2 position)
	{
		Region closestRegion = null;
		float smallestDistance = Mathf.Infinity;
		for (int i = 0; i < regionsMax; i++)
		{
			float distance = (regions[i].position - position).sqrMagnitude;
			if (distance < smallestDistance)
			{
				smallestDistance = distance;
				closestRegion = regions[i];
			}
		}
		return closestRegion;
	}

}
