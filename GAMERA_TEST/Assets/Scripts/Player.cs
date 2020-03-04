using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class Player : MonoBehaviour {

	public Entity entityStats;

	[Header("Click delay")]
	public float clickDelay = (float)(0.5);

	[Header("Target to follow")]
	public Transform targetTransform;

	//[Header("Rot and move speed")]
	//public float movementSpeed;
	//public float rotationSpeed;

	[Header("Button to click to move")]
	public int int_buttonNumber = 1;

	[Header("Enemy AI")]
	public Transform enemyTransform;

	[Header("Stop distances")]
	//public float stoppingDistanceEnemy = 3;
	public float stoppingDistanceTarget = 0.2f;

	//NavMesh of the player
	NavMeshAgent nav;
	bool followEnemy = false;
	float enlapsedTimeClick = 0;
	float enlapsedTimeAttack = 0;

	//Enemy stats
	Entity enemyStats;

	void Start() {
		nav = transform.GetComponent<NavMeshAgent>();
		enemyStats = enemyTransform.gameObject.GetComponent<Enemy>().entityStats;
	}
	
	void Attack() {
		transform.LookAt(enemyTransform);
		Debug.Log("PLAYER ATTACKS");
		//variable to let attack or not
		enlapsedTimeAttack = Time.time;
		
		//Get my damage
		int damage = entityStats.damages;
		Debug.Log("damage: " + damage);
		
		//If i can do critical hit --> damage * 2
		if (entityStats.criticalHitProbability > 0) {
			if (Random.Range(0, 100) < entityStats.criticalHitProbability) {
				damage *= 2;
			}
		}

		Debug.Log("damage: " + damage);

		//Subtract enemy shield
		damage -= enemyStats.shield;
		Debug.Log("damage: " + damage);

		//Clamp damages from 0 to Enemy life
		damage = Mathf.Clamp(damage, 0, enemyStats.life);
		Debug.Log("damage: " + damage);

		//Hit enemy
		enemyStats.life -= damage;
		
		//Enemy die
		if (enemyStats.life == 0) {
			GameObject.Destroy(enemyTransform.gameObject);
		}
	}
	
	void Update() {
		if (followEnemy) {
			targetTransform.position = enemyTransform.position;
			nav.SetDestination(targetTransform.position);
			if (Vector3.Distance(targetTransform.position, transform.position) < nav.stoppingDistance) {
				if ((Time.time - enlapsedTimeAttack) > (1 / entityStats.attackSpeed)) {
					Attack();
				}
			}
		}

		if ((Time.time - enlapsedTimeClick) > clickDelay) {
			if (Input.GetMouseButton(int_buttonNumber)) {
				enlapsedTimeClick = Time.time;

				//Raycast fom camera
				Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
				RaycastHit hit;
				if (Physics.Raycast(ray.origin, ray.direction, out hit)) {
					string tag = hit.transform.gameObject.tag;
					switch (tag) {
						//Hit terrain
						case "Terrain":
							if (Vector3.Distance(hit.point, transform.position) > 1) {
								followEnemy = false;
								targetTransform.position = hit.point;
								nav.SetDestination(hit.point);
								nav.stoppingDistance = (float)(stoppingDistanceTarget);
							}
							break;

						//Hit enemy
						case "Enemy":
							followEnemy = true;
							nav.stoppingDistance = entityStats.attackDistance;
							break;
					}
				}
			}
		}
	}
}
