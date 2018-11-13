using UnityEngine;
using UnityEngine.UI;
//using UnityEngine.EventSystems;

public class ActionButton : MonoBehaviour {


	[SerializeField]
	private Button jumpButton;
	public bool jump = false;
	// Use this for initialization
	void Start () {

		jumpButton = GetComponent<Button> ();
		jumpButton.onClick.AddListener (TaskOnClick);
	}
	
	public void TaskOnClick()
	{
		jump = true;
		Debug.Log ("action");
	}
}
