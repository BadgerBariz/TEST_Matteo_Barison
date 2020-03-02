using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class PlayerTarget : MonoBehaviour {
	[Header("Button to click to move")]
	public int int_buttonNumber = 2;
	public Transform playerTransform;
	public Transform enemyTransform;

	public float stoppingDistanceFromEnemy = 3;
	public float stoppingDistanceFromTarget = 0.2f;


	bool followEnemy = false;
	NavMeshAgent playerNavMeshAgent;

	private void Start() {
		//Get player posizion at start
		transform.position = playerTransform.position;
		playerNavMeshAgent = playerTransform.GetComponent<NavMeshAgent>();
		stoppingDistanceFromTarget = playerNavMeshAgent.stoppingDistance;
		//playerNavMeshAgent.stoppingDistance = stoppingDistanceFromTarget;
	}

	void Update() {
		if (followEnemy) {
			transform.position = enemyTransform.position;
		}
		if (Input.GetMouseButton(int_buttonNumber)) {
			//Raycast fom camera
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;

			//If I hit something
			if (Physics.Raycast(ray.origin, ray.direction, out hit)) {
				//If I hit terrain with RayCast...
				string tag = hit.transform.gameObject.tag;
				switch (tag) {
					case "Terrain":
						followEnemy = false;
						transform.position = hit.point;
						playerNavMeshAgent.stoppingDistance = (float)(stoppingDistanceFromTarget);
						break;
					case "Enemy":
						followEnemy = true;
						playerNavMeshAgent.stoppingDistance = stoppingDistanceFromEnemy;
						break;
				}
			}
		}
	}
}