using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingPlacement : MonoBehaviour {
	
	//production menu buttons calls this script it places the selected building as a mouse cursor
	private MouseTracking me;

	[SerializeField] private GameObject building;

	public void Send()
	{
		me = MouseTracking.me;
		building = (GameObject) Instantiate(Resources.Load("Prefabs/" + transform.gameObject.name));
		building.name = transform.gameObject.name;
		me.Pick(building);
	}
}