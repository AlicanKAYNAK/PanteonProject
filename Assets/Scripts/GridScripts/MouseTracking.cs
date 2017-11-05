using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//mouse tracking is used in building construction
public class MouseTracking : MonoBehaviour {

	public static MouseTracking me { get; private set; }

	public GameObject buildingCursor;
	public bool cursorActive;

	private Vector3 mousePos;

	private void Awake () {
		if (me != null && me != this)
			Destroy(this);
		else
			me = this;
	}

	//if a building is picked from the production menu cursor becomes that object and it starts to move with move
	private void Update () {
		if (buildingCursor != null)
		{
			mousePos = Input.mousePosition;
			mousePos.z = buildingCursor.transform.position.z - Camera.main.transform.position.z;
			mousePos = new Vector3((int)Camera.main.ScreenToWorldPoint(mousePos).x, (int)Camera.main.ScreenToWorldPoint(mousePos).y, (int)Camera.main.ScreenToWorldPoint(mousePos).z);
			buildingCursor.transform.localPosition = mousePos;

			if (Input.GetMouseButtonDown (0)) {
				CreatingBuildingsLeftMouseClick (buildingCursor);
			}
			if (Input.GetMouseButtonDown(1))
				DestroyBuilding();
		}
	}

	//mouse tracking left click calls this to check the conditions of the location and if those are valid building drops
	public void CreatingBuildingsLeftMouseClick (GameObject selectedBuilding) {
		if (selectedBuilding != null) {
			BuildingManager selectedBuildScript = selectedBuilding.GetComponent<BuildingManager> ();
			if (selectedBuildScript != null) { 
				SelectionManager.me.getMultipleTilesFromCoords (selectedBuilding.transform.localPosition, selectedBuildScript.widthInTiles, selectedBuildScript.heightInTiles); 
				selectedBuildScript.isCurrentLocAValidForConstruction ();
			}
		} 
	}

	//when a building is selected from production menu other selection actions, production menu and information menu becomes deactive for convenience
	public void Pick (GameObject b) {
		SelectionManager.me.selected = null; //if the last object selected is barracks this disables its spawn point image
		SelectionManager.me.enabled = false;
		cursorActive = true;
		buildingCursor = b;
		CanvasManager.me.productionMenu.SetActive(false);
		CanvasManager.me.informationMenu.SetActive (false);
	}

	////when a building is placed on grid production menu other selection actions and production menu active again and that bulding is checked as dropped for information menu access
	public void Drop () {
		SelectionManager.me.enabled = true;
		cursorActive = false;
		buildingCursor.GetComponentInChildren<BuildingManager> ().dropCheck = true;
		buildingCursor = null;
		CanvasManager.me.productionMenu.SetActive(true);
	}

	//removes the object stuck on cursor and actives the menus again
	public void DestroyBuilding () {
		if (buildingCursor != null)
		{
			SelectionManager.me.enabled = true;
			Destroy(buildingCursor);
			CanvasManager.me.productionMenu.SetActive(true);
		}
	}
}