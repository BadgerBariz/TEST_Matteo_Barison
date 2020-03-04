using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Entity", menuName = "Entity")]
public class Entity : ScriptableObject {
	public int life;

	public int mana;
	public int shield;
	public int damages;

	//How many attacks for each second
	public float attackSpeed;

	[Range(0, 100)]
	public int criticalHitProbability;

	[Range(3, 10)]
	public int attackDistance;
}
