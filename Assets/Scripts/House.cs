using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class House : MonoBehaviour, IDamageable
{
	[Min(1)]
	public int subjectCapacityIncrease = 1;
	[Required]
	public Resource subjectCapacity;
	[Required]
	public Gold goldPrefab;
	[Required]
	public Transform goldThrowStart;
	[Required]
	public Transform goldThrowEnd;
	[Min(0)]
	public float spawnFrequency = 60;
	public List<Subject> subjects = new List<Subject>();

	[SerializeField][Min(0)]
	private float _hp = 100;
	public float Hp { get => _hp; set => _hp = value; }
	[SerializeField][Min(0)]
	private float _hpMax = 100;
	public float HpMax { get => _hpMax; set => _hpMax = value; }

	private void OnEnable()
	{
		Resource.count[subjectCapacity] += subjectCapacityIncrease;
		StartCoroutine(SpawnGold());
	}

	private void OnDisable()
	{
		Resource.count[subjectCapacity] -= subjectCapacityIncrease;
	}

	private IEnumerator SpawnGold()
	{
		while (true)
		{
			yield return new WaitForSeconds(spawnFrequency);
			Gold gold = Instantiate(goldPrefab, goldThrowStart.position, goldThrowStart.rotation);
			gold.StartCoroutine(gold.Throw(goldThrowEnd.position));
		}
	}

	public bool Damage(float value)
	{
		if (Hp <= 0)
		{
			Destroy(gameObject);
			return true;
		}
		return false;
	}
}
