using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UnitProduction : MonoBehaviour {
	
	//unit production button calls this script
	public void Send () {
		SelectionManager.me.selected.GetComponentInChildren<BuildingManager>().Spawn(transform.gameObject.name);
	}
}