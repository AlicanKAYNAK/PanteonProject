using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrassTile : TileMasterClass {

	//just a demo tile to make the game pretty
	void Awake () {
		type = "Grass";
		walkable = true;
	}
}
