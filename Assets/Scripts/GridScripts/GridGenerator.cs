using System.Collections.Generic;
using TileScripts;
using UnityEngine;

//creating a grid of tiles to place soldiers and buildings on used tile grid instead of spriteless matrix for much more game like visuals 
namespace GridScripts
{
    public class GridGenerator : MonoBehaviour {
	
        public static GridGenerator me { get; private set; }

        public GameObject grass; //cell pixel sized basic tile
        public Vector2 gridDimensions;  //grid or map has 20tiles width and 30tiles height

        TileMasterClass[,] gridOfTiles;

        void Awake () {
            if (me != null && me != this)
                Destroy (this);
            else {
                me = this;
                gridOfTiles = new TileMasterClass[(int)gridDimensions.x,(int)gridDimensions.y];
            }
        }

        void Start () {
            generateGrid ();
        }

        public void setGrid (TileMasterClass[,] newGrid)	{
            gridOfTiles = newGrid;
        }
		
        //creates a grid based on the dimensions provided
        void generateGrid () {
            for (int x = 0; x < gridDimensions.x; x++) {
                for (int y = 0; y < gridDimensions.y; y++) {
                    Vector3 posToCreateTile = new Vector3 (x, y, 0);
                    GameObject mostRecentTile = (GameObject)Instantiate (grass, posToCreateTile, Quaternion.Euler (0, 0, 0));
                    mostRecentTile.GetComponent<TileMasterClass> ().setGridCoords (new Vector2 (x, y));
                    mostRecentTile.transform.parent = this.gameObject.transform;
                    mostRecentTile.name = "Tile " + mostRecentTile.GetComponent<TileMasterClass> ().getGridCoords ().ToString ();
                    gridOfTiles [x, y] = mostRecentTile.GetComponent<TileMasterClass> ();
                }
            }
        }
		
        //try catch incase player clicks out of grid
        public TileMasterClass getTile (int x,int y) {
            try{
                return gridOfTiles [x, y];
            }
            catch{
                return null;
            }
        }

        //gets a section of the grid based on the coords passed in
        public List<GameObject> getTiles (Vector2 startPos,Vector2 endPos) {
            int lowestX, lowestY,highestX,highestY;
            List<GameObject> retVal = new List<GameObject>();
            if (startPos.x <= endPos.x) {
                lowestX = (int)startPos.x;
                highestX = (int)endPos.x;
            } else {
                lowestX = (int)endPos.x;
                highestX = (int)startPos.x;
            }

            if (startPos.y <= endPos.y) {
                lowestY = (int)startPos.y;
                highestY = (int)endPos.y;
            } else {
                lowestY = (int)endPos.y;
                highestY = (int)startPos.y;
            }


            for(int x = (int)lowestX;x<=(int)highestX;x++)
            {
                for (int y = (int)lowestY; y <= (int)highestY; y++) {
                    retVal.Add (gridOfTiles [x, y].gameObject);
                }
            }
            return retVal;

        }
		
        //get the neighbors of the tile passed in as a list
        public List<TileMasterClass> getTileNeighbors (TileMasterClass Tile) {
            List<TileMasterClass> retVal = new List<TileMasterClass>();
            TileMasterClass t = Tile;

            Vector2 pos = t.getGridCoords ();

            //passed in tile is at left edge
            if (pos.x == 0) {
			
                if (pos.y == 0) {
                    //passed in tile is at bottom left
                    retVal.Add(gridOfTiles[(int)pos.x+1,(int)pos.y]);
                    retVal.Add(gridOfTiles[(int)pos.x,(int)pos.y+1]);

                } else if (pos.y == gridDimensions.y - 1) {
                    //passed in tile is at top left
                    retVal.Add(gridOfTiles[(int)pos.x+1,(int)pos.y]);
                    retVal.Add(gridOfTiles[(int)pos.x,(int)pos.y-1]);
                } else {
                    retVal.Add(gridOfTiles[(int)pos.x+1,(int)pos.y]);
                    retVal.Add(gridOfTiles[(int)pos.x,(int)pos.y+1]);
                    retVal.Add(gridOfTiles[(int)pos.x,(int)pos.y-1]);
                }

                //passed in tile is at right edge
            } else if (pos.x == gridDimensions.x - 1) {
			
                if (pos.y == 0) {
                    //passed in tile is at  bottom right y
                    retVal.Add(gridOfTiles[(int)pos.x-1,(int)pos.y]);
                    retVal.Add(gridOfTiles[(int)pos.x,(int)pos.y+1]);
                } else if (pos.y == gridDimensions.y - 1) {
                    //passed in tile is at top right y
                    retVal.Add(gridOfTiles[(int)pos.x-1,(int)pos.y]);
                    retVal.Add(gridOfTiles[(int)pos.x,(int)pos.y-1]);
                } else {
                    retVal.Add(gridOfTiles[(int)pos.x-1,(int)pos.y]);
                    retVal.Add(gridOfTiles[(int)pos.x,(int)pos.y+1]);
                    retVal.Add(gridOfTiles[(int)pos.x,(int)pos.y-1]);
                }

            } else {
                //tile position x is not on the edge
                if (pos.y == 0) {
                    //passed in tile is at edge left y
                    retVal.Add (gridOfTiles [(int)pos.x - 1,(int) pos.y]);
                    retVal.Add (gridOfTiles [(int)pos.x, (int)pos.y + 1]);
                    retVal.Add (gridOfTiles [(int)pos.x+1, (int)pos.y]);
                } else if (pos.y == gridDimensions.y - 1) {
                    //passed in tile is at edge right y
                    retVal.Add (gridOfTiles [(int)pos.x - 1,(int) pos.y]);
                    retVal.Add (gridOfTiles [(int)pos.x, (int)pos.y - 1]);
                    retVal.Add (gridOfTiles [(int)pos.x+1, (int)pos.y]);
                } else {
                    //both x and y coordinates are not on the edge
                    retVal.Add (gridOfTiles [(int)pos.x - 1, (int)pos.y]);
                    retVal.Add (gridOfTiles [(int)pos.x, (int)pos.y - 1]);
                    retVal.Add (gridOfTiles [(int)pos.x+1, (int)pos.y]);
                    retVal.Add (gridOfTiles [(int)pos.x, (int)pos.y +1]);
                }
            }

            return retVal;
        }
    }
}
