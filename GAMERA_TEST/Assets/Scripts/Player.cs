using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Player : MonoBehaviour {
	public Transform targetTransform;
	//public float targetDistanceTolerance = 3.0f;

	public float movementSpeed;
	public float rotationSpeed;

	public NavMeshAgent nav;
	void Start() {
		nav = GetComponent<NavMeshAgent>();
		nav.updateRotation = false;
	}
	
	private void Update() {
		//Target position
		Vector3 targetPosition = targetTransform.position;

		//Direction to the target
		Vector3 direction = targetPosition - transform.position;

		//Get the rotation to the target and rotate
		Quaternion tarRot = Quaternion.LookRotation(direction);
		transform.rotation = Quaternion.Lerp(transform.rotation, tarRot, rotationSpeed * Time.deltaTime);
		transform.rotation = new Quaternion(0, transform.rotation.y, 0, transform.rotation.w);

		if (Vector3.Distance(targetTransform.position, transform.position) > nav.stoppingDistance) {
			nav.Move(transform.forward * movementSpeed * Time.deltaTime);
		}
	}

	//void Update() {
	//		//Target position
	//		Vector3 targetPosition = targetTransform.position;

	//		//Direction to the target
	//		Vector3 direction = targetPosition - transform.position;

	//		//Get the rotation to the target and rotate
	//		Quaternion tarRot = Quaternion.LookRotation(direction);
	//		transform.rotation = Quaternion.Lerp(transform.rotation, tarRot, rotationSpeed * Time.deltaTime);
	//	if (Vector3.Distance(transform.position, targetTransform.position) > targetDistanceTolerance) {
	//		//Player movement
	//		//transform.Translate(new Vector3(0, 0, movementSpeed * Time.deltaTime));
	//		transform.position = Vector3.MoveTowards(this.transform.position, targetTransform.position, movementSpeed * Time.deltaTime);
	//	}

	//}
}
