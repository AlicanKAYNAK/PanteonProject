using System.Collections.Generic;
using CanvasScripts;
using UnityEngine;

namespace UnitScripts
{
    public class UnitMasterClass : MonoBehaviour {

        //base class of all units
        public List<Action> actionsQueue;

        public void Awake ()	{
            actionsQueue = new List<Action> ();
        }
	
        void Update () {
            queueMoniter ();
        }

        public void removeCurrentAction () {
            Action a = actionsQueue [0];
            actionsQueue.Remove (a);
            Destroy (a);
        }

        //queue check if there is something to do or not
        protected void queueMoniter () {
            if (actionsQueue.Count > 0) {
                if (actionsQueue [0].actionStarted == false) {
                    //using try catch here to catch when a walkable tile surrounded by unwalkable tiles selected as the moving position and set everything back to the previous version and soldier stays in its place
                    Action a = actionsQueue [0];
                    try {
                        actionsQueue [0].doAction ();
                        actionsQueue [0].actionStarted = true;
                    } catch {
                        //when spawning massive numbers of soldiers if they all choose a location that they cant go because its unreachable they would stack on each other
                        ErrorMessage.me.PassErrorMessage ("There is no avaliable way to this tile..");
                        a.cleanUp ();
                    }
                } else {
                    if (actionsQueue [0].getIsActionComplete () == true) {
                        removeCurrentAction ();
                    }
                }
            } 
        }

        //currently only movement actions are doable
        public virtual bool canWePerformAction (Action ac) {
            if (ac.getActionType ().Equals ("Movement")) {
                return true;
            } else if (ac.getActionType ().Equals ("Default")) {
                return false;
            } else {
                return false;
            }
        }
    }
}
