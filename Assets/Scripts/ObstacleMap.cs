using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ObstacleMap : MonoBehaviour
{
	public static ObstacleMap instance;
	
	public Vector2Int size = Vector2Int.one;
	public Vector2 tileSize = Vector2.one;
	public bool outOfBoundsError;

	private int[,] map;
	[ShowInInspector][ReadOnly]
	[PreviewField(200, ObjectFieldAlignment.Center)]
	private Texture2D mapTexture;

	public List<Vector2Int> positions;

	private void Awake()
	{
		instance = this;

		map = new int[size.x, size.y];

		positions = new List<Vector2Int>();
		for (int x = 0; x < size.x; x++)
			for (int y = 0; y < size.y; y++)
				positions.Add(new Vector2Int(x, y));
		positions = positions.OrderBy(p => (p - new Vector2Int(size.x / 2, size.y / 2)).magnitude).ToList();

		mapTexture = new Texture2D(size.x, size.y);
		mapTexture.filterMode = FilterMode.Point;
		for (int x = 0; x < size.x; x++)
			for (int y = 0; y < size.y; y++)
				mapTexture.SetPixel(x, y, Color.black);
		mapTexture.Apply();
	}


	public void UpdateTexture()
	{
		mapTexture.Apply();
	}


	private void Inc(Vector2Int position)
	{
		if (position.x >= 0 && position.y >= 0 && position.x < size.x && position.y < size.y)
		{
			map[position.x, position.y]++;
			mapTexture.SetPixel(position.x, position.y, map[position.x, position.y] > 1 ? Color.red : Color.yellow);
		}
		else if (outOfBoundsError)
			Debug.LogError($"Out of bounds: {position}", this);
	}

	private void Dec(Vector2Int position)
	{
		if (position.x >= 0 && position.y >= 0 && position.x < size.x && position.y < size.y)
		{
			map[position.x, position.y]--;
			mapTexture.SetPixel(position.x, position.y, map[position.x, position.y] > 1 ? Color.white : Color.gray);
		}
		else if (outOfBoundsError)
			Debug.LogError($"Out of bounds: {position}", this);
	}

	private int Get(Vector2Int mapPosition)
	{
		if (mapPosition.x >= 0 && mapPosition.y >= 0 && mapPosition.x < size.x && mapPosition.y < size.y)
			return map[mapPosition.x, mapPosition.y];
		else if (outOfBoundsError)
			Debug.LogError($"Out of bounds: {mapPosition}", this);
		return 0;
	}


	public void Inc(Vector2 worldPosition)
	{
		Vector2Int mapPosition = WorldToMapPosition(worldPosition);
		Inc(mapPosition);
	}

	public void Dec(Vector2 worldPosition)
	{
		Vector2Int mapPosition = WorldToMapPosition(worldPosition);
		Dec(mapPosition);
	}

	public int Get(Vector2 worldPosition)
	{
		Vector2Int mapPosition = WorldToMapPosition(worldPosition);
		return Get(mapPosition);
	}

	public void Inc(Rect worldRect)
	{
		RectInt mapRect = new RectInt(WorldToMapPosition(worldRect.position), new Vector2Int(Mathf.CeilToInt(worldRect.size.x / tileSize.x), Mathf.CeilToInt(worldRect.size.y / tileSize.y)));
		for (int x = mapRect.xMin; x < mapRect.xMax; x++)
			for (int y = mapRect.yMin; y < mapRect.yMax; y++)
				Inc(new Vector2Int(x, y));
		UpdateTexture();
	}

	public void Dec(Rect worldRect)
	{
		RectInt mapRect = new RectInt(WorldToMapPosition(worldRect.position), new Vector2Int(Mathf.CeilToInt(worldRect.size.x / tileSize.x), Mathf.CeilToInt(worldRect.size.y / tileSize.y)));
		for (int x = mapRect.xMin; x < mapRect.xMax; x++)
			for (int y = mapRect.yMin; y < mapRect.yMax; y++)
				Dec(new Vector2Int(x, y));
		UpdateTexture();
	}

	public Vector2? GetNearestPosition(int value = 0)
	{
		int id = positions.FindIndex(p => map[p.x, p.y] == value);
		if (id > -1)
			return MapToWorldPosition(positions[id]);
		return null;
	}


	private Vector2Int WorldToMapPosition(Vector2 worldPostion)
	{
		worldPostion = new Vector2(worldPostion.x - transform.position.x, worldPostion.y - transform.position.y);
		Vector2Int mapPosition = new Vector2Int(Mathf.FloorToInt(worldPostion.x / tileSize.x), Mathf.FloorToInt(worldPostion.y / tileSize.y));
		mapPosition = new Vector2Int((int)(mapPosition.x + size.x * 0.5f), (int)(mapPosition.y + size.y * 0.5f));

		return mapPosition;
	}

	private Vector2 MapToWorldPosition(Vector2Int mapPosition)
	{
		Vector2 worldPosition = new Vector2((int)(mapPosition.x - size.x * 0.5f), (int)(mapPosition.y - size.y * 0.5f));
		worldPosition = new Vector2(worldPosition.x * tileSize.x, worldPosition.y * tileSize.y);
		worldPosition = new Vector2(worldPosition.x + transform.position.x, worldPosition.y + transform.position.z);
		worldPosition += new Vector2(tileSize.x * 0.5f, tileSize.y * 0.5f);

		return worldPosition;
	}


}
