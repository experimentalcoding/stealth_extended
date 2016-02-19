using UnityEngine;
using System.Collections;

// PlayerInventory class from stealth tutorial,
// For original file see: https://unity3d.com/learn/tutorials/projects/stealth/the-key
// Main addition is to support tracking of mine ownership.
// Update function was added too which allows player to tap 'Z' key to place/activate a mine
public class PlayerInventory : MonoBehaviour
{
    public bool hasKey;         	// Whether or not the player has the key.
	public bool hasMines;			// Whether or not player has picked up the mines yet
	public bool nearSwitch;			// Can't place mines near a laser deactivation switch

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