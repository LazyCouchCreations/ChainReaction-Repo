using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using UnityEngine.UI;

public class PlayerControl : MonoBehaviour {

	private NavMeshAgent agent;
	public GameObject selectionCircle;
	private ArtificialIntelligence artificialIntelligence;
	public bool isSelected = false;
	public float playerSpeed;
	public float playerTurnSpeed;
	public Light flashLight;
	public Light infectedLight;
	public float myVelocity;

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
	public Animator anim;

	public GameObject bloodPrefab;

	private GameManager gameManager;

	// Use this for initialization
	void Awake () {
		gameManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
		selectionCircle.SetActive(false);
		agent = GetComponent<NavMeshAgent>();
		artificialIntelligence = GetComponent<ArtificialIntelligence>();
		anim = GetComponent<Animator>();
		target = null;
	}
	
	// Update is called once per frame
	void Update () {
		try
		{
			if (tag == "Player")
			{
				myVelocity = agent.velocity.magnitude;
				anim.SetFloat("velocity", myVelocity);

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

					//update UI
					gameManager.textInfecting.SetActive(true);
					gameManager.castBar.SetActive(true);
					gameManager.castBarFill.GetComponent<Image>().fillAmount = infectionTime / infectionDuration;

					//stop the player				
					if (target != null)
					{
						agent.ResetPath();
						victim = target;
						target = null;

						//animate the victim
						victim.GetComponent<Animator>().SetTrigger("isBeingInfected");
						anim.SetTrigger("isInfecting");
					}

					if (infectionTime >= infectionDuration)
					{
						gameManager.castBar.SetActive(false);
						gameManager.textInfecting.SetActive(false);
						infectionTime = 0;
						isInfecting = false;
						victim.GetComponent<PlayerControl>().MakePlayer();
						victim = null;
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
							gameManager.textInfecting.SetActive(false);
							gameManager.castBar.SetActive(false);
							if (victim != null)
							{
								victim.GetComponent<ArtificialIntelligence>().Unstun();
								victim = null;
							}
							
							if (hit.collider.tag == "Enemy" || hit.collider.tag == "Player")
							{
								target = hit.collider.gameObject;
								GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>().groundTargetMarker.transform.position = new Vector3(0,-10,0);
							}
							else
							{
								target = null;
								GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>().groundTargetMarker.transform.position = hit.point;
								agent.SetDestination(hit.point);
							}
						}
					}
				}
			}
		}
		catch (System.Exception)
		{
			//do nothing
		}
		
	}

	public void MakePlayer()
	{
		anim.SetBool("isPlayer", true);
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
		gameManager.textInfected.SetActive(true);
	}

	public void Die()
	{
		gameManager.players.Remove(gameObject);
		Destroy(gameObject);
		gameManager.textInfecting.SetActive(false);
		gameManager.castBar.SetActive(false);
		Instantiate(bloodPrefab, transform.position, Quaternion.Euler(0f, Random.Range(0, 360), 0));
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
