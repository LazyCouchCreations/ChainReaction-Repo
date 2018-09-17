using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
//using UnityEngine.UI;

public class GameManager : MonoBehaviour {

	private GameObject currentPlayer;
	public GameObject castBar;
	public GameObject castBarFill;
	public GameObject textInfecting;
	public GameObject textInfected;
	public GameObject textInterrupted;
	public GameObject groundTargetMarker;

	public GameObject startMenu;
	public GameObject pauseMenu;
	public GameObject gameOverMenu;
	public GameObject victoryMenu;

	public List<GameObject> enemies;
	public List<GameObject> players;

	private void Awake()
	{
		Time.timeScale = 0;
		startMenu.SetActive(true);
	}

	private void Start()
	{
		//create initial player
		enemies[0].GetComponent<PlayerControl>().MakePlayer();
		players[0].GetComponent<PlayerControl>().isSelected = true;
		currentPlayer = players[0];
	}

	// Update is called once per frame
	void Update () {

		if (Input.GetKeyDown(KeyCode.Escape))
		{
			pauseMenu.SetActive(true);
			Time.timeScale = 0;
		}

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

		if (players.Count <= 0)
		{
			gameOverMenu.SetActive(true);
			Time.timeScale = 0;
		}
		if (enemies.Count <= 0 && players.Count > 0)
		{
			victoryMenu.SetActive(true);
			Time.timeScale = 0;
		}
	}

	public void ClearMenus()
	{
		pauseMenu.SetActive(false);
		startMenu.SetActive(false);
		gameOverMenu.SetActive(false);
		victoryMenu.SetActive(false);
		Time.timeScale = 1;
	}
	
	public void ResetLevel()
	{
		ClearMenus();
		SceneManager.LoadScene(0);
	}
}
