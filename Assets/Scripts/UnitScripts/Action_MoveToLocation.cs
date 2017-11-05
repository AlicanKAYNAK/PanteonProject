using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Action_MoveToLocation : Action {

	UnitMovement um;
	Vector3 positionWeAreMovingTo;
	TileMasterClass targetNode;

	public override void initaliseLocation (Vector3 position) {
		//checking if the position is on the grid
		edgeChecker (ref position);
		//if the chosen walkable tile got surrounded by unwalkable tiles with no way in that gives an argument out of range error!!!
		targetNode = GridGenerator.me.getTile ((int)position.x, (int)position.y);

		while (!targetNode.isTileWalkable () || !targetNode.hasNoUnitOnIt()) {
			//adjust the tile to a new close one 
			position.x += Random.Range (-1, 2);
			position.y += Random.Range (-1, 2);
			edgeChecker (ref position);
			targetNode = GridGenerator.me.getTile ((int)position.x, (int)position.y);
		}
		positionWeAreMovingTo = position;
		targetNode.setTileStandable (false);
	}

	//starting the movement action and setting the tile we were to walkable again
	public override void doAction () {
		um = this.GetComponent<UnitMovement> ();
		targetNode = GridGenerator.me.getTile ((int)um.positionWeAreAt.x, (int)um.positionWeAreAt.y);
		targetNode.setTileWalkable (true);
		targetNode.setTileStandable (true);
		um.moveToLocation (positionWeAreMovingTo);
	}

	//set every changed tile to the previous position
	public override void cleanUp ()	{
		targetNode = GridGenerator.me.getTile ((int)um.positionWeAreAt.x, (int)um.positionWeAreAt.y);
		targetNode.setTileWalkable (false);
		targetNode.setTileStandable (false);
		targetNode = GridGenerator.me.getTile ((int)positionWeAreMovingTo.x, (int)positionWeAreMovingTo.y);
		targetNode.setTileStandable (true);
	}

	//check if the movement action is completed and if it is made the tile we are on to unwalkable to prevent soldiers stacking on top of each other and prevent placing buildings on top of soldiers
	public override bool getIsActionComplete () {
		if (Vector3.Distance (positionWeAreMovingTo, this.transform.position) < 2.0f) {
			TileMasterClass targetNode = GridGenerator.me.getTile ((int)positionWeAreMovingTo.x, (int)positionWeAreMovingTo.y);
			targetNode.setTileWalkable (false);
			um = this.GetComponent<UnitMovement> ();
			um.positionWeAreAt = positionWeAreMovingTo;
			return true;
		} else {
			return false;
		}
	}

	private void edgeChecker (ref Vector3 position) {
		//check if position is not on grid, tiles are only in 0-19 if coords exceed these values set them to the close edge of that row 
		if ((int)position.x < 0) {
			position.x = 0;
		} else if ((int)position.x > 19) {
			position.x = 19;
		}
		//check if position is not on grid, tiles are only in 0-29 if coords exceed these values set them to the close edge of that row 
		if ((int)position.y < 0) {
			position.y = 0;
		} else if ((int)position.y > 29) {
			position.y = 29;
		}
	}

	public override string getActionType () {
		return "Movement";
	}
}
