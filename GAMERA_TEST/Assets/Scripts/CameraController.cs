using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

	Vector3 offset;
	public Transform playerTransform;

	void Start() {
		offset = playerTransform.position - transform.position;
	}

	void Update() {
		Camera.main.transform.position = playerTransform.position - offset;
	}
}
