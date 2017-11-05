using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

//when clicked on a building this script will fill the information menu
public class Information : MenuManager
{
	[SerializeField] private Image tempImage;
	[SerializeField] private Text tempText;

	//enabled shows details
	private void OnEnable()
	{
		//every building has a name and this creates it
		var title = Instantiate(tempText, transform);
		title.text = SelectionManager.me.selected.name; 

		//every building has a sprite and this creates it
		var image = Instantiate(tempImage, transform);
		image.sprite = SelectionManager.me.selected.GetComponentInChildren<SpriteRenderer>().sprite;

		//checks if a building can spawn unit and if it can this creates its image as a button under a production title
		if (SelectionManager.me.selected.GetComponent<BuildingManager>().canSpawnUnits()) {
			var productionText = Instantiate (tempText, transform);
			productionText.text = "Production";
			var unitButton = Instantiate (prefab, transform);
			unitButton.transform.localPosition = new Vector3 (0, startingPoint, 0);
			unitButton.GetComponent<Button> ().name = SelectionManager.me.selected.GetComponent<BuildingManager> ().getUnit ().name;
			startingPoint -= unitButton.GetComponent<RectTransform> ().rect.height;
		}
	}

	//disable clears everything
	private void OnDisable()
	{
		foreach (Transform child in transform)
		{
			Destroy(child.gameObject);
			startingPoint = 0;
		}
	}
}