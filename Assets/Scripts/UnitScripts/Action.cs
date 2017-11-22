using UnityEngine;

namespace UnitScripts
{
    public class Action : MonoBehaviour {
        //base class for action system methods can be overriden this system is designed not only for movement action but for any action a strategy game could need

        public bool actionStarted = false;
        public virtual void initaliseLocation (Vector3 position) {
        }

        public virtual void doAction ()	{
        }

        public virtual void cleanUp () {
        }

        public virtual void deactivateTiles() {
        }

        public virtual bool getIsActionComplete () {
            return false;
        }

        public virtual string getActionType () {
            return "Default";
        }
    }
}
