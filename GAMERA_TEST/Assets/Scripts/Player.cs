using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Player : MonoBehaviour {
	public Transform targetTransform;
	public float movementSpeed;
	public float rotationSpeed;
	public NavMeshAgent nav;

	[Header("Button to click to move")]
	public int int_buttonNumber = 2;
	public Transform enemyTransform;

	public float stoppingDistanceFromEnemy = 3;
	public float stoppingDistanceFromTarget = 0.2f;


	bool followEnemy = false;
	NavMeshAgent playerNavMeshAgent;
	
	void Start() {
		//Get player posizion at start
		targetTransform.position = transform.position;
		playerNavMeshAgent = transform.GetComponent<NavMeshAgent>();
		stoppingDistanceFromTarget = playerNavMeshAgent.stoppingDistance;
		
		nav = GetComponent<NavMeshAgent>();
		nav.updateRotation = false;
	}
	
	private void Update() {

		if (followEnemy) {
			targetTransform.position = enemyTransform.position;
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
						targetTransform.position = hit.point;
						playerNavMeshAgent.stoppingDistance = (float)(stoppingDistanceFromTarget);
						break;
					case "Enemy":
						followEnemy = true;
						playerNavMeshAgent.stoppingDistance = stoppingDistanceFromEnemy;
						break;
				}
			}
		}
		
		Vector3 targetPosition = targetTransform.position;
		Vector3 direction = targetPosition - transform.position;
		Quaternion tarRot = Quaternion.LookRotation(direction);
		transform.rotation = Quaternion.Lerp(transform.rotation, tarRot, rotationSpeed * Time.deltaTime);
		transform.rotation = new Quaternion(0, transform.rotation.y, 0, transform.rotation.w);
		if (Vector3.Distance(targetTransform.position, transform.position) > nav.stoppingDistance) {
			nav.Move(transform.forward * movementSpeed * Time.deltaTime);
		}
	}
}
