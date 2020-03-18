using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace BitmaskedTerrain
{
	public class Terrain : MonoBehaviour
	{
		public bool generateInEditMode;
		public Bitmask bitmask;
		public bool combine;
		
		public int SizeX { get; private set; }
		public int SizeZ { get; private set; }
		public bool[,] Map { get; private set; }

		[ShowInInspector]
		public bool IsLoading { get; private set; }
		
		private void Awake()
		{
			bool[,] map = new bool[100, 100];
			for (int x = 0; x < 100; x++)
				for (int z = 0; z < 100; z++)
					map[x,z] = Mathf.PerlinNoise(x * 0.05f, z * 0.05f) > 0.5f;
			Load(map);
		}

		private void OnValidate()
		{
			if (gameObject.activeInHierarchy)
			{
				if (generateInEditMode)
				{
					bool[,] map = new bool[100, 100];
					for (int x = 0; x < 100; x++)
						for (int z = 0; z < 100; z++)
							map[x, z] = Mathf.PerlinNoise(x * 0.05f, z * 0.05f) > 0.5f;
					Load(map);
				}
				else
				{
					DestroyChildren();
				}
			}
		}

		private void DestroyChildren()
		{
			for (int i = transform.childCount - 1; i >= 0; i--)
				DestroyImmediate(transform.GetChild(i).gameObject);
		}

		public void Load(bool[,] map, int loadPerFrame = 0)
		{
			StartCoroutine(LoadCoroutine(map, loadPerFrame));
		}

		private IEnumerator LoadCoroutine(bool[,] map, int loadPerFrame = 0)
		{
			if (!bitmask)
			{
				Debug.LogError("Bitmask not selected in Terrain Component.", this);
				yield break;
			}
			if (bitmask.meshes.Length != 16)
			{
				Debug.LogError($"Wrong size of bitmask '{bitmask.name}' meshes array. Must be 16.", this);
				yield break;
			}
			if (IsLoading)
			{
				Debug.LogError("Terrain is currently loading map. Wait until it's finished.", this);
				yield break;
			}

			IsLoading = true;

			Map = map;
			SizeX = map.GetLength(0);
			SizeZ = map.GetLength(1);

			if (loadPerFrame <= 0)
				loadPerFrame = SizeX * SizeZ;
			int counter = 0;


			for (int x = 0; x < SizeX; x++)
				for (int z = 0; z < SizeZ; z++)
					if(map[x,z])
					{
						byte id = GetBitmaskId(x, z);
						if (bitmask.meshes[id])
						{
							MeshFilter mesh = Instantiate(bitmask.meshes[id], transform);
							mesh.transform.localPosition = new Vector3(x, 0, z);
						}
						counter++;
						if(counter >= loadPerFrame)
						{
							counter = 0;
							yield return null;
						}
					}

			if (combine)
				Combine();

			IsLoading = false;
		}

		private byte GetBitmaskId(int x, int z)
		{
			string bits = "";
			bits += GetMapTile(x + 0, z + 1) == true ? 1 : 0;
			bits += GetMapTile(x - 1, z + 0) == true ? 1 : 0;
			bits += GetMapTile(x + 1, z + 0) == true ? 1 : 0;
			bits += GetMapTile(x + 0, z - 1) == true ? 1 : 0;
			return System.Convert.ToByte(bits, 2); //.ToInt32(bits, 2);
		}

		private bool GetMapTile(int x, int z)
		{
			return Map[Mathf.Clamp(x, 0, SizeX - 1), Mathf.Clamp(z, 0, SizeZ - 1)];
		}

		private void Combine()
		{
			MeshFilter[] meshFilters = GetComponentsInChildren<MeshFilter>();
			CombineInstance[] combine = new CombineInstance[meshFilters.Length];

			for (int i = 0; i < meshFilters.Length; i++)
			{
				combine[i].mesh = meshFilters[i].sharedMesh;
				combine[i].transform = meshFilters[i].transform.localToWorldMatrix;
				meshFilters[i].gameObject.SetActive(false);
			}
			GetComponent<MeshFilter>().sharedMesh = new Mesh();
			GetComponent<MeshFilter>().sharedMesh.CombineMeshes(combine);
			GetComponent<MeshCollider>().sharedMesh = GetComponent<MeshFilter>().sharedMesh;
			gameObject.SetActive(true);
		}
	}
}