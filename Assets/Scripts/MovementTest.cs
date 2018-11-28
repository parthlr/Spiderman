using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Obi;

public class MovementTest : MonoBehaviour {

	Rigidbody chest;

	public List<GameObject> bodyParts;
	List<Vector3> bodyPartPositions;
	List<Quaternion> bodyPartRotations;

	public Camera cam;
	public float upForce = 10f;
	public float sideForce = 20f;

	List<LineRenderer> lineRenderers = new List<LineRenderer> ();

	// Use this for initialization
	void Start () {
		Cursor.lockState = CursorLockMode.Locked;

		chest = bodyParts [0].GetComponent<Rigidbody>();
		bodyPartPositions = new List<Vector3> ();
		bodyPartRotations = new List<Quaternion> ();
		foreach (GameObject bodyPart in bodyParts) {
			bodyPartPositions.Add(bodyPart.GetComponent<Rigidbody> ().transform.position);
			bodyPartRotations.Add (bodyPart.GetComponent<Rigidbody> ().transform.rotation);

			bodyPart.GetComponent<Rigidbody> ().maxAngularVelocity = 1f;
		}
	}
	
	// Update is called once per frame
	void Update () {
		for (int i = 0; i < bodyParts.Count; i++) {
			bodyPartPositions [i] = bodyParts [i].GetComponent<Rigidbody> ().transform.position;
			bodyPartRotations [i] = bodyParts [i].GetComponent<Rigidbody> ().transform.rotation;
		}
		chest.GetComponent<Rigidbody> ().AddForce (cam.transform.right * Input.GetAxis ("Horizontal") * sideForce);
		if (Input.GetKey (KeyCode.Space)) {
			chest.GetComponent<Rigidbody> ().AddForce (Vector3.up * upForce);
		}
	}

	void LateUpdate() {
		for (int i = 0; i < bodyParts.Count; i++) {
			bodyParts[i].GetComponent<Rigidbody> ().transform.position = bodyPartPositions [i];
			bodyParts [i].GetComponent<Rigidbody> ().transform.rotation = bodyPartRotations [i];
		}
	}
}
