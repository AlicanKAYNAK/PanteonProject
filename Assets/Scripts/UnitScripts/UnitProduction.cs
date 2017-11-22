using BuildingScripts;
using GridScripts;
using UnityEngine;

namespace UnitScripts
{
    public class UnitProduction : MonoBehaviour {
	
        //unit production button calls this script
        public void Send () {
            SelectionManager.me.selected.GetComponentInChildren<BuildingManager>().Spawn(transform.gameObject.name);
        }
    }
}