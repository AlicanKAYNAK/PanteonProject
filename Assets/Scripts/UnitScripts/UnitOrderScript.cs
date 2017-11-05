using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UnitOrderScript : MonoBehaviour {
	
	void Update () {
		commandUnitsToMove ();
	}

	//checks if a unit is selected and if there is a right click it that unit can perform that action it sets the mouse position as the location and add the action to the queue
	void commandUnitsToMove()
	{
		if (areAnyUnitsSelected() == true) {
			if (Input.GetMouseButtonDown (1)) {
				Vector3 mousePos = Input.mousePosition;
				Vector3 mouseInWorld = Camera.main.ScreenToWorldPoint (mousePos);
				mouseInWorld.z = 0;

				foreach (GameObject g in SelectionManager.me.getSelected()) {
					if (g.GetComponent<UnitMasterClass> () != null) {
						UnitMasterClass um = g.GetComponent<UnitMasterClass> ();
						Action a = g.AddComponent<Action_MoveToLocation> ();
						if (um.canWePerformAction (a) == true) {
							a.initaliseLocation (mouseInWorld);
							um.actionsQueue.Add (a);
						} else {
							Destroy (a);
						}
					}
				}
			}
		}
	}

	bool areAnyUnitsSelected()
	{
		if (SelectionManager.me.getSelected ().Count > 0) {
			return true;
		} else {
			return false;
		}
	}
}
