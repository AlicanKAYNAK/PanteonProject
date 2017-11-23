using System.Collections.Generic;
using TileScripts;
using UnityEngine;

namespace GridScripts
{
    public class Pathfind : MonoBehaviour {
	
        public static Pathfind me { get; private set; }

        public delegate void ErrorOccuredEventHandler(string s);
        public static event ErrorOccuredEventHandler ErrorOccured;

        private void Awake () {
            if (me != null && me != this)
                Destroy (this);
            else {
                me = this;
            }
        }

        //easy call method with less variables
        public List<Vector3> getPath (Vector3 startPos, Vector3 endPos) {
            List<TileMasterClass> listOfTiles = new List<TileMasterClass> ();
            getPath (startPos, endPos, ref listOfTiles);
            List<Vector3> retVal = convertToVectorPath (listOfTiles);
            return retVal;
        }

        //actual pathfinding uses reference for convenience sets parents of the tiles to form a path
        void getPath (Vector3 startPos, Vector3 endPos, ref List<TileMasterClass> store) {
            var sPos = new Vector2 ((int)startPos.x, (int)startPos.y);//rough tile coords
            var ePos = new Vector2 ((int)endPos.x, (int)endPos.y);

            var startNode = GridGenerator.me.getTile ((int)sPos.x, (int)sPos.y); //tiles from the coords
            var targetNode = GridGenerator.me.getTile ((int)ePos.x, (int)ePos.y);

            if (startNode==null || targetNode==null || targetNode.isTileWalkable () == false ) {
                OnErrorOccured("One of the tiles is not walkable");
                return;
            }

            List<TileMasterClass> openSet = new List<TileMasterClass>(); //tiles we want to check
            List<TileMasterClass> closedSet = new List<TileMasterClass>();//tiles we checked and don't want in path
            openSet.Add(startNode);

            while (openSet.Count > 0) { //cycle through the open set
                var node = openSet[0];

                for (var i = 1; i < openSet.Count; i ++) {
                    if (openSet[i].fCost < node.fCost || openSet[i].fCost == node.fCost) { //check if we can find a tile with lower or equal distance to target from start including this tile
                        if (openSet[i].getH() < node.getH())//if the tile has a lower distance to the target tile than the current one in node
                            node = openSet[i];//sets node to be this closer tile
                    }
                }

                openSet.Remove(node);//takes node from the open set and puts it in the closed set
                closedSet.Add(node);

                if (node == targetNode) {//check to see if we arrived the tile we want to go
                    RetracePath(startNode,targetNode,ref store);//retrace steps
                    return;
                }

                foreach (var neighbour in GridGenerator.me.getTileNeighbors(node)) { //goes through each of the neighbors of the node

                    if (!neighbour.isTileWalkable()  || closedSet.Contains(neighbour) || neighbour==null || node==null) {//if the neighbor is not accessable or the node is null go onto next neighbor
                        continue;
                    }

                    var newCostToNeighbour = node.getG () + GetDistance(node, neighbour);//calculates the cost of going to the neighbor from the start of the path
                    if (newCostToNeighbour < neighbour.getG() || !openSet.Contains(neighbour)) {//if the cost is shorter and the open set doesnt contain the neighbor
                        neighbour.setG(newCostToNeighbour);
                        neighbour.setH(GetDistance(neighbour, targetNode));//set the g and h values of the neighbor
                        neighbour.setParent(node);//sets the parent of the neighbor to be the node signifying that in the path you'll go from the node to the neighbor
                        if (!openSet.Contains(neighbour))//adds the neighbor to the open set if not already there
                            openSet.Add(neighbour);
                    }
                }
            }
        }

        //converts the path found to a list of vector3 as the objects moving don't need all the tile info
        List<Vector3> convertToVectorPath (List<TileMasterClass> tiles) {
            List<Vector3> retVal = new List<Vector3> ();
            foreach (TileMasterClass tile in tiles) {
                retVal.Add (tile.gameObject.transform.position);
            }
            return retVal;
        }

        //goes through the path via the parent variable and puts it in a list 
        void RetracePath (TileMasterClass startNode,TileMasterClass targetNode,ref List<TileMasterClass> store) { 
            List<TileMasterClass> path = new List<TileMasterClass>();
            var currentNode = targetNode;

            while (currentNode != startNode) {
                path.Add(currentNode);
                currentNode = currentNode.getParent();
            }
            path.Reverse();//have to reverse it because it if currently finish to start
            store = path;
        }

        //gets the distance between two grid coords and returns them multiplied
        int GetDistance (TileMasterClass nodeA,TileMasterClass nodeB) {
            var dstX = Mathf.Abs((int)nodeA.getGridCoords().x - (int)nodeB.getGridCoords().x);
            var dstY = Mathf.Abs((int)nodeA.getGridCoords().y - (int)nodeB.getGridCoords().y);

            if (dstX > dstY)//to make sure that the final number is positive
                return 14*dstY + 10* (dstX-dstY);
            return 14*dstX + 10 * (dstY-dstX);
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
