using UnityEngine;

namespace TileScripts
{
    public class TileMasterClass : MonoBehaviour {

        //tile generic class for all tile types unwalkable water tile or wheat tile with resource value can be derived from this
        public float gridX,gridY; //tile 2x2 matrix coordinates
        public bool walkable = true; //decides if the tile can be walked on
        public bool noUnitStanding = true; //check for if there is a unit on it
        public string type; //basically name of the tile

        protected int gCost;//cost for moving from the start tile to this tile
        protected int hCost;//an estimate of the distance between this tile and the tile you want to get a path to

        TileMasterClass parent; //used in the pathfinding to retrace steps and give the final path

        public void setG (int val) {
            gCost = val;
        }

        public void setH (int val) {
            hCost = val;
        }

        public int getH () {
            return hCost;
        }

        public int getG () {
            return gCost;
        }

        public virtual void OnSelect () {
		
        }

        public virtual void OnDeselect () {	
		
        }

        public bool isTileWalkable () {
            return walkable;
        }

        public bool hasNoUnitOnIt () {
            return noUnitStanding;
        }
		
        public void setTileWalkable (bool canWalk) {
            walkable = canWalk;
            if (!canWalk) {
                this.GetComponent<SpriteRenderer> ().color = Color.cyan;
            } else {
                this.GetComponent<SpriteRenderer> ().color = Color.white;
            }
        }

        public void setTileStandable (bool unitComes) {
            noUnitStanding = unitComes;	
        }

        public TileMasterClass getParent () {
            return parent;
        }

        public void setParent (TileMasterClass val) {
            parent = val;
        }

        public Vector2 getGridCoords () {
            return new Vector2 (gridX, gridY);
        }

        public void setGridCoords (Vector2 coords) {
            gridX = coords.x;
            gridY = coords.y;
        }
		
        public virtual int fCost { //made virtual to alter path finding
            get{
                return gCost + hCost; //estimation of the total route to destination if this tile is used
            }
        }
    }
}
