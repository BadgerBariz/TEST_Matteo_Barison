using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AI : MonoBehaviour {

	//WANDER
	//public float AIMovementSpeed = 10.0f;
	//public float AIRotationSpeed = 8.0f;
	float targetPositionTolerance;
	[Header("AI Movement Range")]
	public float minX;
	public float maxX;
	public float minZ;
	public float maxZ;
	public Transform target;
	bool canGenerateNewPosition = true;


	//DETECT
	public int angleOfView = 45;
	public int viewDistance = 100;
	public float maxFollowDistance = 300;



	//public float detectionRate = 1.0f;
	//protected float elapsedTime = 0.0f;
	public Transform playerTransform;
	//public float timeBeforeStartFollowing;
	Vector3 rayDirection;





	public GameObject d, dd, ddd;




	enum STATE {
		WANDER, FOLLOW
	}
	STATE AI_STATE;

	private void Start() {
		targetPositionTolerance = GetComponent<NavMeshAgent>().stoppingDistance;
	}

	void Update() {
		d.transform.localEulerAngles = new Vector3(0, angleOfView / 2, 0);
		dd.transform.localEulerAngles = new Vector3(0, -angleOfView / 2, 0);
		
		d.transform.localScale = new Vector3(0.1f, 0.1f, viewDistance);
		dd.transform.localScale = new Vector3(0.1f, 0.1f, viewDistance);
		ddd.transform.localScale = new Vector3(0.1f, 0.1f, viewDistance);



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
		rayDirection = playerTransform.position - transform.position;
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
		//Invoke("FollowPlayer", timeBeforeStartFollowing);
	}

	//void FollowPlayer() {
		//AI_STATE = STATE.FOLLOW;
	//}



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