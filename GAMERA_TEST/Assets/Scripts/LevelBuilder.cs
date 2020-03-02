using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class LevelBuilder : MonoBehaviour {

	public NavMeshSurface navMeshSurface;

	void Start() {
		navMeshSurface.BuildNavMesh();
	}


	void Update() {

	}
}
