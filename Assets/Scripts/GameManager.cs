using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

	private GameObject currentPlayer;
	public GameObject castBar;
	public GameObject castBarFill;
	public GameObject textInfecting;
	public GameObject textInfected;
	public GameObject textInterrupted;

	public List<GameObject> enemies;
	public List<GameObject> players;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetButtonDown("Fire1"))
		{
			RaycastHit hit;
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			if (Physics.Raycast(ray, out hit))
			{
				if(hit.collider.tag == "Player")
				{
					if (currentPlayer != null)
					{
						currentPlayer.GetComponent<PlayerControl>().Deselect();
					}					
					hit.collider.GetComponent<PlayerControl>().Select();
					currentPlayer = hit.collider.gameObject;
				}
			}
		}
	}
}
