using UnityEngine;

//singleton canvas manager for menu prefabs
namespace CanvasScripts
{
    public class CanvasManager : MonoBehaviour {

        public GameObject productionMenu;
        public GameObject informationMenu;

        public static CanvasManager me { get; private set; }

        void Awake () {
            if (me != null && me != this) {
                Destroy (this);
            } else {
                me = this;
            }
        }
    }
}
