using GridScripts;
using UnityEngine;

//singleton canvas manager for menu prefabs
namespace CanvasScripts
{
    public class CanvasManager : MonoBehaviour
    {

        public GameObject productionMenu;
        public GameObject informationMenu;

        public static CanvasManager me { get; private set; }

        void Start()
        {
            MouseTracking.BuildingCreation += OnBuildingCreation;
            SelectionManager.LeftClickBuilding += OnLeftClickBuilding;
        }

        void Awake () {
            if (me != null && me != this) {
                Destroy (this);
            } else {
                me = this;
            }
        }

        public void OnBuildingCreation(bool b)
        {
            if (b)
            {
                productionMenu.SetActive(!b);
                informationMenu.SetActive(!b);
            }
            else
            {
                productionMenu.SetActive(!b);
            }
        }

        public void OnLeftClickBuilding(bool b)
        {
            informationMenu.SetActive(b);
        }
    }
}
