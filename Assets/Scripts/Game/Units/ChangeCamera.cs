using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeCamera : MonoBehaviour {

	public Camera mainCamera;
	public Camera viewCamera;

	// Use this for initialization
	void Start () {

		if (mainCamera != null &&  viewCamera != null) {
			mainCamera.gameObject.SetActive(false);
			viewCamera.gameObject.SetActive(true);
		} else {
			Debug.Break();
			Debug.LogError ("Camers is null");
		}
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
