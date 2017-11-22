using UnityEngine;
using UnityEngine.UI;

namespace CanvasScripts
{
    public class Production : MenuManager {
        public float cellSizeY; // height of the buttons
        public float paddingY; // distance between buttons
        public float upperLimit; // top of the recttransform 

        private GameObject[] firstTwoButton;
        private GameObject[] lastTwoButton;
        private GameObject[] tempTwoButton;
        private float tempLocation;
        private int totalLineCount;

        private void Start () {
            //production script is attached to content which has a y variable of top of the screen hence it is the upper limit
            upperLimit = transform.position.y;

            paddingY = GetComponent<GridLayoutGroup>().spacing.y;
            cellSizeY = GetComponent<GridLayoutGroup>().cellSize.y;

            firstTwoButton = new GameObject[2];//for both first and last array 0 counts as Barracks
            lastTwoButton = new GameObject[2];//for both first and last array 1 counts as PowerPlant
            tempTwoButton = new GameObject[2];//temp holder for a line of 2 buildings

            totalLineCount = transform.childCount / 2;

            //for initializing first two children on the list are also the last two children on the list
            SetFirstButtons();
            SetLastButtons();

            for (int i = 0; i < Screen.height / cellSizeY - totalLineCount; i++) { // screen height divided by height of a button gives us how many object that we are gonna need to fill the screen
                SetTempButtons(i%totalLineCount);//divide by 2 because there are 2 building on every line, modulo to find the corresponding line
                for (int j = 0; j < 2; j++){
                    createButtons (j);
                }
                SetLastButtons();
            }
        }

        private void Update () {
            //top 2 buttons y position are same so it is enough to check for only 1. if their position is greater than padding + top of the screen first two becomes last two
            if (firstTwoButton[0].transform.position.y > upperLimit + paddingY) {
                tempLocation = 0 - cellSizeY - paddingY;
                for (int i = 0; i < 2; i++) {
                    scrollButton (i, tempLocation);	
                }
                //first and last buttons changed so resetting them
                SetFirstButtons();
                SetLastButtons();
            } else if (lastTwoButton[0].transform.position.y < -2 ) {
                tempLocation = cellSizeY + paddingY;
                for (int i = 1; i > -1; i--) {//reverse order to not to disturb the order of the list
                    scrollButton (i, tempLocation);	
                }
                //first and last buttons changed so resetting them
                SetFirstButtons();
                SetLastButtons();
            }
        }

        //sets up the content's first two child for easy access
        private void SetFirstButtons () {
            firstTwoButton[0] = transform.GetChild(0).gameObject;
            firstTwoButton[1] = transform.GetChild(1).gameObject;
        }

        //easy access to last two child of content
        private void SetLastButtons () {
            lastTwoButton[0] = transform.GetChild(transform.childCount - 2).gameObject;
            lastTwoButton[1] = transform.GetChild(transform.childCount - 1).gameObject;
        }

        //easy access to last two child of content
        private void SetTempButtons(int i)
        {
            i = i + (i * 1); //a formula to get the appropriate building
            tempTwoButton[0] = transform.GetChild(i).gameObject;
            tempTwoButton[1] = transform.GetChild(i+1).gameObject;
        }

        private void createButtons (int i) {
            var clone = Instantiate(tempTwoButton[i], transform);
            //changing its position to a to bottom of the child last created
            clone.GetComponent<RectTransform> ().anchoredPosition = new Vector2(lastTwoButton[i].GetComponent<RectTransform>().anchoredPosition.x, lastTwoButton[i].GetComponent<RectTransform>().anchoredPosition.y - cellSizeY - paddingY);
            clone.name = tempTwoButton[i].name; //when we call it later in building a building we use its name so we give it a proper name here
            clone.transform.SetAsLastSibling(); //evey time a button gets created it goes at the bottom of the list
        }

        private void scrollButton (int i, float location) {
            if (location > 0) {
                //buttons which are out of the screen from the bottom gets teleported to the top of the screen and gets to set as the first child
                lastTwoButton[i].GetComponent<RectTransform>().anchoredPosition = new Vector2(lastTwoButton[i].GetComponent<RectTransform>().anchoredPosition.x, firstTwoButton[i].GetComponent<RectTransform>().anchoredPosition.y + location);
                lastTwoButton [i].transform.SetAsFirstSibling ();
            } else if (location < 0) {
                //buttons which are out of the screen from the top gets teleported to the bottom of the screen and gets to set as the last child
                firstTwoButton[i].GetComponent<RectTransform>().anchoredPosition = new Vector2(firstTwoButton[i].GetComponent<RectTransform>().anchoredPosition.x, lastTwoButton[i].GetComponent<RectTransform>().anchoredPosition.y + location);
                firstTwoButton[i].transform.SetAsLastSibling();
            }
        }
    }
}