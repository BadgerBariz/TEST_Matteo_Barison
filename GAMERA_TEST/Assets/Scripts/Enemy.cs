using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour {

	public Entity entityStats;

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

	public float stoppingDistance = 0.2f;

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
	
	float enlapsedTimeAttack = 0;

	//Player stats
	Entity playerStats;

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
		playerStats = playerTransform.gameObject.GetComponent<Player>().entityStats;
	}

	void Attack() {
		transform.LookAt(playerTransform);
		//Debug.Log("ENEMY ATTACKS");
		//variable to let attack or not
		enlapsedTimeAttack = Time.time;

		//Get my damage
		int damage = entityStats.damages;

		//If i can do critical hit --> damage * 2
		if (entityStats.criticalHitProbability > 0) {
			if (Random.Range(0, 100) < entityStats.criticalHitProbability) {
				damage *= 2;
			}
		}

		//Subtract enemy shield
		damage -= playerStats.shield;

		//Clamp damages from 0 to Enemy life
		damage = Mathf.Clamp(damage, 0, playerStats.life);

		//Hit enemy
		playerStats.life -= damage;

		//Enemy die
		if (playerStats.life == 0) {
			//Player dead
			Debug.Log("Player dead");
		}
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

			if (Vector3.Distance(playerTransform.position, transform.position) < nav.stoppingDistance) {
				if ((Time.time - enlapsedTimeAttack) > (1 / entityStats.attackSpeed)) {
					Attack();
				}
			}

			//IF I GET DISTANT FROM PLAYER...
			if ((Vector3.Distance(target.position, transform.position) > maxFollowDistance) ||
				!new Rect(minX, minZ, Mathf.Abs((maxX - minX)), Mathf.Abs((maxZ - minX))).Contains(new Vector2(playerTransform.position.x, playerTransform.position.z))) {
				//I RESTART WANDER
				AI_STATE = STATE.WANDER;
				nav.stoppingDistance = stoppingDistance;
				GetNextPosition();
			}
		}
	}

	//GENERATE NEW POSITION
	void GetNextPosition() { 
		//CONTROL OF THE STATE BECAUSE THIS FUNCTION CAN BE INVOKED AFTER PLAYER DETECT
		if (AI_STATE == STATE.WANDER) { 
			target.position = new Vector3(Random.Range(minX, maxX), 0.5f, Random.Range(minZ, maxZ));
			nav.SetDestination(target.position);
		}
	}

	bool Detect() {
		//RAYCAST TO THE PLAYER
		Vector3 rayDirection = playerTransform.position - transform.position;

		//CHECK THE ANGLE OF VIEW
		if (Vector3.Angle(rayDirection, transform.forward) < (angleOfView / 2)) { 
			RaycastHit hit;

			//CHECK RAYCAST
			if (Physics.Raycast(transform.position + Vector3.up, rayDirection, out hit, viewDistance)) { 
				if (hit.transform.CompareTag("Player")) {
					target.position = transform.position;
					AI_STATE = STATE.FOLLOW;
					nav.stoppingDistance = entityStats.attackDistance;
					return true;
				}
			}
		}
		return false;
	}
	
	//DEBUG VIEW RANGE
	private void OnDrawGizmos() {
		d1.transform.localEulerAngles = new Vector3(0, angleOfView / 2, 0);
		d2.transform.localEulerAngles = new Vector3(0, -angleOfView / 2, 0);
		d1.transform.localScale = new Vector3(0.1f, 0.1f, viewDistance);
		d2.transform.localScale = new Vector3(0.1f, 0.1f, viewDistance);
		d3.transform.localScale = new Vector3(0.1f, 0.1f, viewDistance);


		Debug.DrawRay(transform.position, playerTransform.position - transform.position, Color.red);

	}
}
