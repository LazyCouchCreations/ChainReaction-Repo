using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Stickers : MonoBehaviour {

	private GameManager gameManager;
	public List<Image> alive;
	public List<Image> infected;

	// Use this for initialization
	void Start () {
		gameManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
	}
	
	// Update is called once per frame
	void Update () {

		//alive
		int aliveCount = gameManager.enemies.Count;
		foreach (Image sticker in alive)
		{
			if (alive.IndexOf(sticker) < aliveCount)
			{
				sticker.enabled = true;
			}
			else
			{
				sticker.enabled = false;
			}
		}

		//infected
		int infectedCount = gameManager.players.Count;
		foreach (Image sticker in infected)
		{
			if (infected.IndexOf(sticker) < infectedCount)
			{
				sticker.enabled = true;
			}
			else
			{
				sticker.enabled = false;
			}
		}
	}
}
