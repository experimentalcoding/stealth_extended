using UnityEngine;
using System.Collections;

public class PlayerInventory : MonoBehaviour
{
    public bool hasKey;         	// Whether or not the player has the key.
	public bool hasMines;			// Whether or not player has picked up the mines yet
	public bool nearSwitch;			// Can't place mines near a laser deactivation switch
	//public MineState mineState;		// Can place mine by pressing 'Z' if not near laser deactivation button

	[HideInInspector]
	public MineTrigger currentMine;	
	public GameObject minePrefab;

	private Transform playerTransform;

	void Awake()
	{
		playerTransform = transform;
	}

	void Update () 
	{
		if(Input.GetButtonDown("Switch") && !nearSwitch)
		{
			if (currentMine == null && hasMines) 
			{
				// Make sure mine position is set low to the ground
				Vector3 position = playerTransform.position;
				position.y = .01f;

				// Create mine instance
				Rigidbody mineInstance;
				mineInstance = Instantiate(minePrefab, playerTransform.position, new Quaternion()) as Rigidbody;
			}
			else
			{
				// If an active mine is set, then activate it
				currentMine.Activate();
			}
		}
	}
}