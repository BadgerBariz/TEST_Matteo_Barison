using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Player : MonoBehaviour {

	[Header("Click delay")]
	public float clickDelay = (float)(0.5);

	[Header("Target to follow")]
	public Transform targetTransform;

	[Header("Rot and move speed")]
	public float movementSpeed;
	public float rotationSpeed;

	[Header("Button to click to move")]
	public int int_buttonNumber = 2;

	[Header("Enemy AI")]
	public Transform enemyTransform;

	[Header("Stop distances")]
	public float stoppingDistanceEnemy = 3;
	public float stoppingDistanceTarget = 0.2f;

	//NavMesh of the player
	NavMeshAgent nav;
	bool followEnemy = false;
	float enlapsedTime = 0;


	void Start() {
		//Get player posizion at start
		targetTransform.position = transform.position;

		nav = transform.GetComponent<NavMeshAgent>();
		stoppingDistanceTarget = nav.stoppingDistance;
		nav.updateRotation = false;
	}


	void Update() {
		if (followEnemy) {
			targetTransform.position = enemyTransform.position;
			if (Vector3.Distance(targetTransform.position, transform.position) < nav.stoppingDistance) {
				//attack
			}
		}

		if ((Time.time - enlapsedTime) > clickDelay) {
			if (Input.GetMouseButton(int_buttonNumber)) {
				enlapsedTime = Time.time;
				//Raycast fom camera
				Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
				RaycastHit hit;
				if (Physics.Raycast(ray.origin, ray.direction, out hit)) {
					//hit tag
					string tag = hit.transform.gameObject.tag;
					switch (tag) {
						//Hit terrain
						case "Terrain":
							followEnemy = false;
							targetTransform.position = hit.point;
							nav.stoppingDistance = (float)(stoppingDistanceTarget);
							break;
						//Hit enemy
						case "Enemy":
							followEnemy = true;
							nav.stoppingDistance = stoppingDistanceEnemy;
							break;
					}
				}
			}
		}


		//Take direction
		Vector3 direction = targetTransform.position - transform.position;
		//Rotate on LookRotation
		transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(direction), rotationSpeed * Time.deltaTime);
		//Set X and Z rotation to zero
		transform.rotation = new Quaternion(0, transform.rotation.y, 0, transform.rotation.w);

		//If not arrived to destination --> move
		if (Vector3.Distance(targetTransform.position, transform.position) > nav.stoppingDistance) {
			nav.Move(transform.forward * movementSpeed * Time.deltaTime);
		}
	}
}
