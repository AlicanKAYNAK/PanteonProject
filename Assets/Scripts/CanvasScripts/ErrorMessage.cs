using System.Collections;
using BuildingScripts;
using GridScripts;
using UnitScripts;
using UnityEngine;
using UnityEngine.UI;

namespace CanvasScripts
{
    public class ErrorMessage : MonoBehaviour {
        
        public static ErrorMessage me { get; private set; }

        void Awake () {
            if (me != null && me != this) {
                Destroy (this);
            } else {
                me = this;
            }
        }

        private void Start () {
            GetComponent<Text>().text = string.Empty;
            BuildingManager.ErrorOccured += OnErrorOccured;
            SelectionManager.ErrorOccured += OnErrorOccured;
            UnitMasterClass.ErrorOccured += OnErrorOccured;
            Pathfind.ErrorOccured += OnErrorOccured;
        }

        private IEnumerator ShowError () {
            while (true) {
                yield return new WaitForSeconds(0.1f);
                if(GetComponent<Text>().text != string.Empty){
                    yield return new WaitForSeconds (0.5f);
                    GetComponent<Text>().text = string.Empty;
                }
            }
        }

        public void OnErrorOccured(string s)
        {
            GetComponent<Text>().text = s;
            StartCoroutine(ShowError());
        }
    }
}
