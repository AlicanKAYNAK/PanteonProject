using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//abstract menu for production and information to fill
public abstract class MenuManager : MonoBehaviour
{
	public float startingPoint;

	[SerializeField] internal GameObject prefab;
}