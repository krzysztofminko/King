using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BitmaskedTerrain
{
	[CreateAssetMenu(menuName = "Terrain Bitmask", fileName = "TerrainBitmask")]
	public class Bitmask : ScriptableObject
	{
		public float meshSizeX = 1;
		public float meshSizeZ = 1;
		public MeshFilter[] meshes = new MeshFilter[16];
	}
}