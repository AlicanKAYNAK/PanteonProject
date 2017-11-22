using CanvasScripts;
using GridScripts;
using TileScripts;
using UnityEngine;

namespace BuildingScripts
{
    public abstract class BuildingManager : MonoBehaviour {

        //abstract building base 
        public bool dropCheck = false; //checker if the build is placed or not
        public int widthInTiles, heightInTiles; //building size in tiles
        public bool canSpawn; //checker if the building can spawn units or not

        public bool canSpawnUnits () {
            return canSpawn;
        }

        void Awake () {
            dropCheck = false;
        }

        //building's unit spawner implementation
        public abstract void Spawn (string unit);

        public abstract GameObject getUnit ();
		
        //building left click 
        internal virtual void OnMouseDown () {
            //dropcheck is for not opening menu automatics as it gets placed mouse tracking cursor check is for not selecting the buildings already dropped on tiles
            if(dropCheck && !MouseTracking.me.cursorActive) {
                TileMasterClass targetNode = GridGenerator.me.getTile ((int)transform.position.x, (int)transform.position.y);
                if (targetNode != null) {
                    CanvasManager.me.informationMenu.SetActive (false); // to clear info menu
                    SelectionManager.me.clearDelegate (); // to clear previous methods in the delegate
                    SelectionManager.me.selected = transform.gameObject;
                    CanvasManager.me.informationMenu.SetActive (true); // to show info menu
                }
            }
        }

        //basicly goes through the current selection of tiles and checks if they are a valid place to build a building
        public void isCurrentLocAValidForConstruction () { 
            if (SelectionManager.me.currentlySelected.Count <= 0) {
                ErrorMessage.me.PassErrorMessage ("There isn't a tile at the mouse cursor for the script to use");
                return;
            }
            TileMasterClass tm = null;
            //for a building to be placed at a location there has to be 2 conditions met all the tiles must be walkable & the building must have a ring of walkable tiles around it
            foreach (GameObject tile in SelectionManager.me.currentlySelected) {
                tm = tile.GetComponent<TileMasterClass> ();
                if (!tm.isTileWalkable ()) {
                    ErrorMessage.me.PassErrorMessage ("There is an object colliding with your building");
                    return;
                }
            }
            createBuildingAtLocation ();
        }

        //creates a building from the one currently selected at the mouse position
        private void createBuildingAtLocation() { 
            //gets the lowest and highest of each axis, works cause the selected tiles are in increasing order
            int xLowBound = (int)SelectionManager.me.currentlySelected[0].GetComponent<TileMasterClass> ().getGridCoords ().x;
            int xHighBound = (int)SelectionManager.me.currentlySelected[SelectionManager.me.currentlySelected.Count-1].GetComponent<TileMasterClass> ().getGridCoords ().x;
            int yLowBound = (int)SelectionManager.me.currentlySelected[0].GetComponent<TileMasterClass> ().getGridCoords ().y;
            int yHighBound = (int)SelectionManager.me.currentlySelected[SelectionManager.me.currentlySelected.Count-1].GetComponent<TileMasterClass> ().getGridCoords ().y;

            for (int x = 0; x < SelectionManager.me.currentlySelected.Count ; x++) { //goes through all the tiles and makes the ones building will be placed unwalkable
                GameObject tile = SelectionManager.me.currentlySelected [x];
                TileMasterClass tm = tile.GetComponent<TileMasterClass> ();

                Vector2 curTileGrid = tm.getGridCoords ();
                if (curTileGrid.x == xLowBound || curTileGrid.x == xHighBound || curTileGrid.y == yLowBound || curTileGrid.y == yHighBound) {
                    if (curTileGrid.y == yLowBound || curTileGrid.y == yHighBound || curTileGrid.x == xLowBound || curTileGrid.x == xHighBound) {
                        //Debug.LogError ("Keeping " + tile.name + " Walkable"); //a ring of walkable tiles around the building
                    }
                } else {
                    tm.setTileWalkable (false);
                }
            }
            MouseTracking.me.Drop();
            SelectionManager.me.clearSelected ();
        }
    }
}
