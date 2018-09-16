using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class PlayerControl : MonoBehaviour {

	private NavMeshAgent agent;
	public GameObject selectionCircle;
	private ArtificialIntelligence artificialIntelligence;
	private bool isSelected = false;
	public float playerSpeed;
	public float playerTurnSpeed;
	public Light flashLight;
	public Light infectedLight;
	
	public float playerAccel;
	private GameObject target;
	private GameObject victim;
	public float maxInfectedLightAngle;
	public float minInfectedLightAngle;
	public Color maxInfectedColor;
	public Color minInfectedColor;
	public float deathTime;
	public float deathTimeMod;
	public bool isInfecting;
	public float infectionDuration;
	public float infectionTime;

	private GameManager gameManager;

	// Use this for initialization
	void Awake () {
		gameManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
		selectionCircle.SetActive(false);
		agent = GetComponent<NavMeshAgent>();
		artificialIntelligence = GetComponent<ArtificialIntelligence>();
		target = null;
	}
	
	// Update is called once per frame
	void Update () {
		if (tag == "Player")
		{
			selectionCircle.SetActive(isSelected);

			infectedLight.spotAngle = Mathf.Lerp(maxInfectedLightAngle, minInfectedLightAngle, deathTime);
			infectedLight.color = Color.Lerp(maxInfectedColor, minInfectedColor, deathTime);
			deathTime += deathTimeMod * Time.deltaTime;

			if (deathTime >= 1)
			{
				Die();
			}

			if (target != null && agent != null)
			{
				agent.SetDestination(target.transform.position);
			}

			if (isInfecting)
			{
				infectionTime += Time.deltaTime;

				//stop the player				
				if (target != null)
				{
					agent.ResetPath();
					victim = target;
					target = null;
				}				

				if (infectionTime >= infectionDuration)
				{
					infectionTime = 0;
					isInfecting = false;
					victim.GetComponent<PlayerControl>().MakePlayer();
				}
			}

			if (isSelected)
			{
				if (Input.GetButtonDown("Fire2"))
				{
					RaycastHit hit;
					Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
					if (Physics.Raycast(ray, out hit))
					{
						isInfecting = false;
						infectionTime = 0;
						if (hit.collider.tag == "Enemy" || hit.collider.tag == "Player")
						{
							target = hit.collider.gameObject;
						}
						else
						{
							target = null;
							agent.SetDestination(hit.point);
						}
					}
				}
			}
		}
	}

	public void MakePlayer()
	{
		
		gameManager.enemies.Remove(gameObject);
		gameManager.players.Add(gameObject);

		tag = "Player";
		agent.speed = playerSpeed;
		agent.angularSpeed = playerTurnSpeed;
		agent.acceleration = playerAccel;
		flashLight.enabled = false;
		infectedLight.enabled = true;
		infectedLight.spotAngle = maxInfectedLightAngle;
		deathTime = 0;
	}

	public void Die()
	{
		gameManager.players.Remove(gameObject);
		Destroy(gameObject);
	}

	public void Select()
	{
		isSelected = true;
	}

	public void Deselect()
	{
		isSelected = false;
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject == target)
		{
			infectionTime = 0;
			isInfecting = true;
			other.GetComponent<ArtificialIntelligence>().Stun();			
		}			
	}
}
