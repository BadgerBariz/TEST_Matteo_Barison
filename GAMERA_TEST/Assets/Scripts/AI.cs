using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AI : MonoBehaviour {

	//WANDER
	float targetPositionTolerance;
	[Header("AI Movement Range")]
	public float minX;
	public float maxX;
	public float minZ;
	public float maxZ;


	[Header("Taget transform")]
	public Transform target;

	[Header("Player transform")]
	public Transform playerTransform;
	
	//Semaphore for invoke of new position
	bool canGenerateNewPosition = true;

	//DETECT
	[Header("AI range")]
	public int angleOfView = 45;
	public int viewDistance = 100;
	public float maxFollowDistance = 300;
	
	
	[Header("Debug cubes to see range of view")]
	public GameObject d1, d2, d3;
	
	
	[Header("AI Rot and move speed")]
	public float movementSpeed;
	public float rotationSpeed;


	enum STATE {
		WANDER, FOLLOW
	}
	STATE AI_STATE;
	NavMeshAgent nav;
	
	 void Start() {
		nav = GetComponent<NavMeshAgent>();
		nav.updateRotation = false;
	}

	void Update() {
		Vector3 targetPosition = target.position;
		Vector3 direction = targetPosition - transform.position;
		Quaternion tarRot = Quaternion.LookRotation(direction);
		transform.rotation = Quaternion.Lerp(transform.rotation, tarRot, rotationSpeed * Time.deltaTime);
		transform.rotation = new Quaternion(0, transform.rotation.y, 0, transform.rotation.w);
		if (Vector3.Distance(target.position, transform.position) > nav.stoppingDistance) {
			nav.Move(transform.forward * movementSpeed * Time.deltaTime);
		}

		d1.transform.localEulerAngles = new Vector3(0, angleOfView / 2, 0);
		d2.transform.localEulerAngles = new Vector3(0, -angleOfView / 2, 0);
		d1.transform.localScale = new Vector3(0.1f, 0.1f, viewDistance);
		d2.transform.localScale = new Vector3(0.1f, 0.1f, viewDistance);
		d3.transform.localScale = new Vector3(0.1f, 0.1f, viewDistance);



		if (AI_STATE == STATE.FOLLOW) {
			target.position = playerTransform.position;
			if (Vector3.Distance(target.position, transform.position) > maxFollowDistance) {
				AI_STATE = STATE.WANDER;
			}
		} else if (AI_STATE == STATE.WANDER) {
			if (!Detect()) {
				if (Vector3.Distance(target.position, transform.position) < targetPositionTolerance) {
					if (canGenerateNewPosition) {
						Invoke("GetNextPosition", Random.Range(1, 4));
					}
					canGenerateNewPosition = false;
				} else {
					canGenerateNewPosition = true;
				}
			}
		}
	}

	void GetNextPosition() {
		if (AI_STATE == STATE.WANDER) {
			target.position = new Vector3(Random.Range(minX, maxX), 0.5f, Random.Range(minZ, maxZ));
		}
	}

	bool Detect() {
		Vector3 rayDirection = playerTransform.position - transform.position;
		RaycastHit hit;
		if ((Vector3.Angle(rayDirection, transform.forward) < (angleOfView / 2)) && (Physics.Raycast(transform.position, rayDirection, out hit, viewDistance))) {
			if (hit.transform.CompareTag("Player")) {
				PlayerDetected();
				return true;
			}
		}
		return false;
	}

	void PlayerDetected() {
		target.position = transform.position;
		AI_STATE = STATE.FOLLOW;
	}
	


	/*
	#region Debug
	void OnDrawGizmos() {
		if (playerTransform == null) {
			return;
		}
		Debug.DrawLine(transform.position, playerTransform.position, Color.red);

		//Vector3 frontRayPoint = transform.position + (transform.forward * viewDistance);

		Debug.DrawRay(transform.position, transform.forward * viewDistance, Color.green);
		
	}

	#endregion*/
}