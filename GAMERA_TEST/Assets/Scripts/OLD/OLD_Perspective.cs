using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[ExecuteInEditMode]
public class Perspective : MonoBehaviour {
	public int fieldOfView = 45;
	public int viewDistance = 100;
	public float detectionRate = 1.0f;

	protected float elapsedTime = 0.0f;

	public Transform playerTransform;
	private Vector3 rayDirection;
	

	void Update() {
		elapsedTime += Time.deltaTime;
		if (elapsedTime >= detectionRate) {
			DetectAspect();
		}
	}


	void DetectAspect() {
		rayDirection = playerTransform.position - transform.position;
		if (Vector3.Angle(rayDirection, transform.forward) < (fieldOfView / 4)) {
			print("\nA: " + Vector3.Angle(rayDirection, transform.forward));
			print("B: " + (fieldOfView / 4));
			print("Enemy Detected");
			Time.timeScale = 0;
		}
	}


	void OnDrawGizmos() {
		if (playerTransform == null) {
			return;
		}
		Debug.DrawLine(transform.position, playerTransform.position, Color.red);

		Vector3 frontRayPoint = transform.position + (transform.forward * viewDistance);
		
		Vector3 leftRayPoint = frontRayPoint;
		leftRayPoint.x += fieldOfView / 2;

		Vector3 rightRayPoint = frontRayPoint;
		rightRayPoint.x -= fieldOfView / 2;

		Debug.DrawRay(transform.position, frontRayPoint, Color.green);
		Debug.DrawRay(transform.position, leftRayPoint, Color.green);
		Debug.DrawRay(transform.position, rightRayPoint, Color.green);
	}
}

