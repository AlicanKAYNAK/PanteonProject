using UnityEngine;

//abstract menu for production and information to fill
namespace CanvasScripts
{
    public abstract class MenuManager : MonoBehaviour
    {
        public float startingPoint;

        [SerializeField] internal GameObject prefab;
    }
}