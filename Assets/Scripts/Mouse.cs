using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Mouse : MonoBehaviour {

	public GameObject cursorContainer;
	public Sprite defaultCursor;
	public Sprite enemyCursor;
	public Sprite playerCursor;

	// Use this for initialization
	void Start () {
		Cursor.visible = false;
	}
	
	// Update is called once per frame
	void Update () {
		cursorContainer.transform.position = Input.mousePosition;

		RaycastHit hit;
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		if (Physics.Raycast(ray, out hit))
		{			
			if (hit.collider.tag == "Player")
			{
				GetComponent<Image>().sprite = playerCursor;
			}
			else if (hit.collider.tag == "Enemy")
			{
				GetComponent<Image>().sprite = enemyCursor;
			}
			else
			{
				GetComponent<Image>().sprite = defaultCursor;
			}
		}
	}
}
