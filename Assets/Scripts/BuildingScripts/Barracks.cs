using GridScripts;
using UnitScripts;
using UnityEngine;

namespace BuildingScripts
{
    public class Barracks : BuildingManager {

        public UnitMovement moveUnit;

        [SerializeField] private GameObject spawnUnit;
        [SerializeField] private GameObject spawnPoint;

        public override void Spawn (string unit) {
            if (!spawnPoint.activeSelf) {
                OnErrorOccured("Assign a spawn point");
                //ErrorMessage.me.PassErrorMessage ("Assign a spawn point");
            } else {
                var unitPos = new Vector3 (transform.position.x, transform.position.y - 2, -1f);
                //spawnUnit = Instantiate ((GameObject)Resources.Load ("Prefabs/" + spawnUnit.name.Replace("(Clone)","")), unitPos, Quaternion.identity);
                spawnUnit = Instantiate ((GameObject)Resources.Load ("Prefabs/" + unit), unitPos, Quaternion.identity);
                spawnUnit.name = unit;
                var spawnPos = new Vector3 (this.spawnPoint.transform.position.x, this.spawnPoint.transform.position.y, -1f);

                UnitMasterClass um = spawnUnit.GetComponent<UnitMasterClass> ();
                Action a = spawnUnit.AddComponent<Action_MoveToLocation> ();
                if (um.canWePerformAction (a)) {
                    a.initaliseLocation (spawnPos);
                    um.actionsQueue.Add (a);
                } else {
                    Destroy (a);
                }
            }
        }

        public override GameObject getUnit () {
            return spawnUnit;
        }

        //late update because if update used spawn point gets active before placing down and selecting it again
        private void LateUpdate () {
            if (SelectionManager.me.selected != null) {
                if (SelectionManager.me.selected.transform.position != transform.position) {
                    spawnPoint.SetActive (false);
                }
            } else {
                spawnPoint.SetActive (false);
            }
        }

        //activates spawn point of the barracks 
        internal virtual void OnMouseDown () {
            SelectionManager.RightClickBuilding += OnRightClickBuilding;
            spawnPoint.SetActive (true);
        }

        // setting spawn point as the given position which will be mouse position of when right clicked
        public void OnRightClickBuilding(Vector3 pos) {
            pos.z = -1;
            spawnPoint.transform.position = pos;
        }
    }
}
