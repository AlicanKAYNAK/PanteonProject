using System.Collections.Generic;
using TileScripts;
using UnityEngine;

namespace GridScripts
{
    public class SelectionManager : MonoBehaviour {

        public static SelectionManager me { get; private set; }

        public delegate void ErrorOccuredEventHandler(string s); //delegate for error messages
        public static event ErrorOccuredEventHandler ErrorOccured;
        public delegate void LeftClickBuildingEventHandler(bool b); //delegate for leftclicks without building tag
        public static event LeftClickBuildingEventHandler LeftClickBuilding;
        public delegate void RightClickBuildingEcentHandler(Vector3 pos); //delegate for right click events
        public static event RightClickBuildingEcentHandler RightClickBuilding; 
        public GameObject selected; //used as storage what have I selected

        public List<GameObject> currentlySelected; //used in building placement to have an easy access to tiles I need to check

        private void Awake () {
            if (me != null && me != this)
                Destroy (this);
            else {
                me = this;
                currentlySelected = new List<GameObject> ();
                selected = null;
            }
        }

        //regular raycast on mouse position to check the objects and select them properly 
        private void Update () {
            var mousePos = new Vector2 (Camera.main.ScreenToWorldPoint (Input.mousePosition).x, Camera.main.ScreenToWorldPoint (Input.mousePosition).y);

            if (Input.GetMouseButtonDown (0)) {
                RaycastHit2D raycast = Physics2D.Raycast (mousePos, Vector2.zero, 0f);
                try {
                    var hitObject = raycast.collider.gameObject;
                    setSelected (hitObject);
                    if (hitObject.CompareTag("Building"))
                    {
                        OnLeftClickBuilding(false);
                        OnLeftClickBuilding(true);
                    }
                    else
                    {
                        OnLeftClickBuilding(false);
                    }
                } catch {
                    clearSelected();
                }
            } else if (Input.GetMouseButtonDown(1) && selected == null) {
                clearSelected();
            } else if (Input.GetMouseButtonDown(1) && selected.CompareTag("Building")) {
                var targetNode = GridGenerator.me.getTile ((int)mousePos.x, (int)mousePos.y);
                if (targetNode != null){
                    if (RightClickBuilding != null) {
                        RightClickBuilding(mousePos);
                        RightClickBuilding = null; //spawn point set is limited to 1 try if the user wants to change it again they should click on the building again
                    }
                } else {
                    OnErrorOccured("There is no tile to put the Spawn Point on");
                } 
            }
        }

        public void clearSelectedIfAnyNull () {
            foreach (GameObject g in currentlySelected) {
                if (g == null) {
                    clearSelected ();
                    break;
                }
            }
        }

        //get tiles multiples times to add them in to a list and checks if there is enough space or a null object(no tile) at the coords we want to place our building
        public void getMultipleTilesFromCoords (Vector3 mousePos, int width,int height) {
            try { 
                //leave space between building for units to move around
                width += 2;
                height += 2;
                GameObject tileAtMousePoint=null;
                tileAtMousePoint = GridGenerator.me.getTile((int)mousePos.x,(int)mousePos.y).gameObject;
                TileMasterClass tm = tileAtMousePoint.GetComponent<TileMasterClass> (); 
                var tileGridCoords = tm.getGridCoords ();

                if (isSelectionInGridRange (tileGridCoords, width, height) == true) {
                    var startPos = new Vector2 (tileGridCoords.x - (width / 2), tileGridCoords.y - (height / 2));
                    var endPos = new Vector2 (tileGridCoords.x + (width / 2), tileGridCoords.y + (height / 2));
                    setSelected(GridGenerator.me.getTiles(startPos,endPos),true);
                } else {
                    clearSelected ();
                    OnErrorOccured("Not enough space");
                }
            }
            catch {
                clearSelected ();
                OnErrorOccured("No tile at mouse position");
            }
        }

        //easy checking of the whether current position plus half of the width and height exceed our grid
        private bool isSelectionInGridRange (Vector2 centerCoords,int width,int height) {
            //half of these because our mouse pos is the center point of the building
            width /= 2;
            height /= 2;
            if ((centerCoords.x - width) < 0 || (centerCoords.y - height) < 0 || (centerCoords.x + width) >= GridGenerator.me.gridDimensions.x || (centerCoords.y + height) >= GridGenerator.me.gridDimensions.y) {
                return false;
            } else {
                return true;
            }
        }

        //for single object settings
        public void setSelected (GameObject toSet) {
            clearSelected ();
            currentlySelected.Add (toSet);
            foreach (GameObject obj in getSelected()) {
                if (obj.GetComponent<TileMasterClass> () == true) {
                    toSet.GetComponent<TileMasterClass> ().OnSelect ();
                }
            }
            clearSelectedIfAnyNull ();
            selected = toSet;
        }

        //for multiple object setting like tiles
        public void setSelected (List<GameObject> toSet, bool clearExisting)	{
            if (clearExisting) {
                clearSelected ();
            }
            currentlySelected = toSet;

            currentlySelected = toSet;

            foreach (GameObject obj in getSelected()) {
                if (obj.GetComponent<TileMasterClass> () == true) {
                    obj.GetComponent<TileMasterClass> ().OnSelect ();
                }
            }
        }

        public List<GameObject> getSelected () {
            return currentlySelected;
        }

        public void clearSelected () {
            foreach (GameObject obj in getSelected()) {
                if (obj.GetComponent<TileMasterClass> () == true) {
                    obj.GetComponent<TileMasterClass> ().OnDeselect ();
                }
            }
            currentlySelected = new List<GameObject> ();
        }
 
        protected virtual void OnErrorOccured(string s)
        {
            if (ErrorOccured != null)
            {
                ErrorOccured.Invoke(s);
            }
        }

        protected virtual void OnLeftClickBuilding(bool b)
        {
            if (LeftClickBuilding != null)
            {
                LeftClickBuilding.Invoke(b);
            }
        }

        protected virtual void OnRightClickBuilding(Vector3 pos)
        {
            if (RightClickBuilding != null)
            {
                RightClickBuilding.Invoke(pos);
            }
        }
    }
}