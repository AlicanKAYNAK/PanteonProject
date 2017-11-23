using CanvasScripts;
using GridScripts;
using TileScripts;
using UnityEngine;

namespace BuildingScripts
{
    public abstract class BuildingManager : MonoBehaviour {

        public delegate void ErrorOccuredEventHandler(string s);
        public static event ErrorOccuredEventHandler ErrorOccured;

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

        public abstract GameObject getUnit();

        //basicly goes through the current selection of tiles and checks if they are a valid place to build a building
        public void isCurrentLocAValidForConstruction () { 
            if (SelectionManager.me.currentlySelected.Count <= 0)
            {
                OnErrorOccured("There isn't a tile at the mouse cursor for the script to use");
                return;
            }
            //for a building to be placed at a location there has to be 2 conditions met all the tiles must be walkable & the building must have a ring of walkable tiles around it
            foreach (var tile in SelectionManager.me.currentlySelected) {
                var tm = tile.GetComponent<TileMasterClass> ();
                if (tm.isTileWalkable()) continue;
                OnErrorOccured("There is an object colliding with your building");
                return;
            }
            createBuildingAtLocation ();
        }

        //creates a building from the one currently selected at the mouse position
        private void createBuildingAtLocation() { 
            //gets the lowest and highest of each axis, works cause the selected tiles are in increasing order
            var xLowBound = (int)SelectionManager.me.currentlySelected[0].GetComponent<TileMasterClass> ().getGridCoords ().x;
            var xHighBound = (int)SelectionManager.me.currentlySelected[SelectionManager.me.currentlySelected.Count-1].GetComponent<TileMasterClass> ().getGridCoords ().x;
            var yLowBound = (int)SelectionManager.me.currentlySelected[0].GetComponent<TileMasterClass> ().getGridCoords ().y;
            var yHighBound = (int)SelectionManager.me.currentlySelected[SelectionManager.me.currentlySelected.Count-1].GetComponent<TileMasterClass> ().getGridCoords ().y;

            foreach (var tile in SelectionManager.me.currentlySelected)
            {
                var tm = tile.GetComponent<TileMasterClass> ();

                var curTileGrid = tm.getGridCoords ();
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

        protected virtual void OnErrorOccured(string s)
        {
            if (ErrorOccured != null)
            {
                ErrorOccured.Invoke(s);
            }
        }
    }
}
