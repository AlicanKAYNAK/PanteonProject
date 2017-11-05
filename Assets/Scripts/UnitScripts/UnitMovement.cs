using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//moves the unit along a path provided by the pathfinding class
public class UnitMovement : MonoBehaviour {
	
	public bool areWeMoving = false;
	public Vector3 positionWeAreAt;
	public List<Vector3> curPath;

	int pathCounter = 0;

	void Start () {
		positionWeAreAt = this.GetComponent<UnitMasterClass> ().transform.position;
	}

	void Update () {
		if (areWeMoving == true) {
			moveAlongPath ();
		}
	}

	public Vector3 getFinalPosition () {
		return curPath [curPath.Count - 1];
	}

	public void moveToLocation (Vector3 positionTo) {
		pathCounter = 0;
		curPath = Pathfind.me.getPath (this.transform.position, positionTo);
		areWeMoving = true;

		if (curPath.Count == 0) {
			areWeMoving = false;
			this.GetComponent<UnitMasterClass> ().removeCurrentAction ();
		}

	}

	void moveAlongPath () {

		if (Vector3.Distance (this.transform.position, curPath [pathCounter]) > 0.5f) {
			Vector3 dir = curPath [pathCounter] - transform.position;
			transform.Translate (dir * 5 * Time.deltaTime);
		} else {
			if (pathCounter < curPath.Count - 1) {
				pathCounter++;
			} else {
				areWeMoving = false;
			}
		}
	}

	
}
