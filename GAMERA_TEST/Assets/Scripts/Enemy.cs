using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour {


	//WANDER
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
	public GameObject d1;
	public GameObject d2;
	public GameObject d3;


	//NAVMESHAGENT
	NavMeshAgent nav;

	//STATE OF THE ENTITY
	enum STATE {
		WANDER, FOLLOW
	}
	STATE AI_STATE;

	void Start() {
		nav = GetComponent<NavMeshAgent>();
		nav.SetDestination(target.position);
	}

	void Update() {
		//NORMAL STATE
		if (AI_STATE == STATE.WANDER) {
			if (!Detect()) { //PLAYER NOT FOUND
				if (Vector3.Distance(target.position, transform.position) < nav.stoppingDistance) {
					//I WANDER UNTIL I FIND THE PLAYER
					if (canGenerateNewPosition) {
						Invoke("GetNextPosition", Random.Range(1, 3));
					}
					canGenerateNewPosition = false;
				} else {
					canGenerateNewPosition = true;
				}
			}
		} else if (AI_STATE == STATE.FOLLOW) { //IF I AM FOLLOWING THE EPLAYER
			target.position = playerTransform.position;
			nav.SetDestination(target.position);
			if (Vector3.Distance(target.position, transform.position) > maxFollowDistance) { //IF I GET DISTANT FROM PLAYER...
				AI_STATE = STATE.WANDER; //I RESTART WANDER
			}
		}
	}

	void GetNextPosition() { //GENERATE NEW POSITION
		if (AI_STATE == STATE.WANDER) { //CONTROL OF THE STATE BECAUSE THIS FUNCTION CAN BE INVOKED AFTER PLAYER DETECT
			target.position = new Vector3(Random.Range(minX, maxX), 0.5f, Random.Range(minZ, maxZ));
			nav.SetDestination(target.position);
		}
	}

	bool Detect() {
		//RAYCAST TO THE PLAYER
		Vector3 rayDirection = playerTransform.position - transform.position;
		if (Vector3.Angle(rayDirection, transform.forward) < (angleOfView / 2)) { //CHECK THE ANGLE OF VIEW
			RaycastHit hit;
			if (Physics.Raycast(transform.position + Vector3.up, rayDirection, out hit, viewDistance)) { //CHECK RAYCAST
				if (hit.transform.CompareTag("Player")) {
					target.position = transform.position;
					AI_STATE = STATE.FOLLOW;
					return true;
				}
			}
		}
		return false;
	}




	private void OnDrawGizmos() {
		d1.transform.localEulerAngles = new Vector3(0, angleOfView / 2, 0);
		d2.transform.localEulerAngles = new Vector3(0, -angleOfView / 2, 0);
		d1.transform.localScale = new Vector3(0.1f, 0.1f, viewDistance);
		d2.transform.localScale = new Vector3(0.1f, 0.1f, viewDistance);
		d3.transform.localScale = new Vector3(0.1f, 0.1f, viewDistance);


		Debug.DrawRay(transform.position, playerTransform.position - transform.position, Color.red);

	}
}
