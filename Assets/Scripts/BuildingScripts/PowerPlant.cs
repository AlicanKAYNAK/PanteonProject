using GridScripts;
using UnityEngine;

namespace BuildingScripts
{
    public class PowerPlant : BuildingManager {
        //powerplant script basically does nothing...

        public override void Spawn (string unit) {
        }

        public override GameObject getUnit () {
            return null;
        }

        //left click on powerplant
        internal virtual void OnMouseDown()
        {
            SelectionManager.RightClickBuilding += OnRightClickBuilding;
        }

        // has no spawn point
        public void OnRightClickBuilding(Vector3 pos)
        {
            OnErrorOccured("There is no Spawn point for this building!");
        }
    }
}
