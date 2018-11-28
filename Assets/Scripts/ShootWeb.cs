using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Obi;

public class ShootWeb : MonoBehaviour {

	public int side;

	public ObiCollider character;
	public float hookExtendRetractSpeed = 2;
	public Material material;

	private ObiRope rope;
	private ObiCatmullRomCurve curve;
	private ObiSolver solver;
	private ObiRopeCursor cursor;

	private RaycastHit hookAttachment;
	private bool attached = false;

	public GameObject chest;
	public Camera cam;

	LineRenderer[] lineRenderers;
	public LineRenderer referenceLine;

	Vector3 attachementPoint;

	void Awake () {
		lineRenderers = new LineRenderer[2];
		attachementPoint = transform.position;

		rope = gameObject.AddComponent<ObiRope>();
		curve = gameObject.AddComponent<ObiCatmullRomCurve>();
		solver = gameObject.AddComponent<ObiSolver>();

		rope.Solver = solver;
		rope.ropePath = curve;
		rope.GetComponent<MeshRenderer>().material = material;
		rope.gameObject.layer = 9;

		rope.resolution = 0.1f;
		rope.BendingConstraints.stiffness = 0.2f;
		rope.uvScale = new Vector2(1,5);
		rope.normalizeV = false;
		rope.uvAnchor = 1;

		solver.substeps = 3;
		solver.distanceConstraintParameters.iterations = 5;
		solver.pinConstraintParameters.iterations = 5;
		solver.bendingConstraintParameters.iterations = 1;
		solver.particleCollisionConstraintParameters.enabled = false;
		solver.volumeConstraintParameters.enabled = false;
		solver.densityConstraintParameters.enabled = false;
		solver.stitchConstraintParameters.enabled = false;
		solver.skinConstraintParameters.enabled = false;
		solver.tetherConstraintParameters.enabled = false;

		cursor = rope.gameObject.AddComponent<ObiRopeCursor>();	
		cursor.normalizedCoord = 0;
		cursor.direction = true;
	}

	private void LaunchHook(){
		LineRenderer newLine = Instantiate (referenceLine, Vector3.zero, Quaternion.identity, referenceLine.transform.parent);
		lineRenderers [side] = newLine;

		Vector3 mouse = Input.mousePosition;
		Ray ray = Camera.main.ScreenPointToRay(mouse);
		int layerMask = ~(1 << 8);
		if (Physics.Raycast(ray,out hookAttachment, Mathf.Infinity, layerMask)){
			attachementPoint = hookAttachment.point;
			StartCoroutine(AttachHook());
		}

	}

	private IEnumerator AttachHook(){
		Vector3 localHit = curve.transform.InverseTransformPoint(hookAttachment.point);

		curve.controlPoints.Clear();
		curve.controlPoints.Add(Vector3.zero);
		curve.controlPoints.Add(Vector3.zero);
		curve.controlPoints.Add(localHit);
		curve.controlPoints.Add(localHit);

		yield return rope.GeneratePhysicRepresentationForMesh();

		ObiPinConstraintBatch pinConstraints = rope.PinConstraints.GetFirstBatch();
		pinConstraints.AddConstraint(0,character,transform.localPosition,1000f);
		pinConstraints.AddConstraint(rope.UsedParticles-1,hookAttachment.collider.GetComponent<ObiColliderBase>(),
			hookAttachment.collider.transform.InverseTransformPoint(hookAttachment.point),1000f);

		rope.AddToSolver(null);
		rope.GetComponent<MeshRenderer>().enabled = false;

		attached = true;
	}

	private void DetachHook(){
		rope.RemoveFromSolver(null);
		rope.GetComponent<MeshRenderer>().enabled = false;

		attached = false;
	}


	void Update () {
		if (attached) {
			chest.GetComponent<Rigidbody> ().AddForce (cam.transform.forward * 200f);
			lineRenderers [side].SetPosition (0, transform.position);
			lineRenderers [side].SetPosition (1, attachementPoint);
		}

		if (Input.GetMouseButtonDown(side)){
			if (!attached) {
				LaunchHook ();
			} else {
				DetachHook ();
				Destroy (lineRenderers [side].gameObject);
			}
		}
	}
}
