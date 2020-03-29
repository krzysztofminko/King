using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleMap : MonoBehaviour
{
	public static ObstacleMap instance;
	
	public Vector2Int size = Vector2Int.one;
	public Vector2 tileSize = Vector2.one;
	public bool outOfBoundsError;

	private int[,] map;
	[ShowInInspector][ReadOnly][PreviewField(200, ObjectFieldAlignment.Center)]
	private Texture2D mapTexture;

	private void Awake()
	{
		instance = this;

		map = new int[size.x, size.y];

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
			mapTexture.SetPixel(position.x, position.y, map[position.x, position.y] > 1 ? Color.white : Color.gray);
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


	private Vector2Int WorldToMapPosition(Vector2 worldPostion)
	{
		Vector2Int mapPosition = new Vector2Int((int)worldPostion.x, (int)worldPostion.y) - new Vector2Int((int)transform.position.x, (int)transform.position.z);
		mapPosition = new Vector2Int((int)(mapPosition.x / tileSize.x), (int)(mapPosition.y / tileSize.y));
		mapPosition = new Vector2Int((int)(mapPosition.x + size.x * 0.5f), (int)(mapPosition.y + size.y * 0.5f));
		return mapPosition;
	}


}
