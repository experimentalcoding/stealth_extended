using UnityEngine;
using System.Collections;

public class LaserSwitchDeactivation : MonoBehaviour
{
	public GameObject laser;                
	public Material unlockedMat;            
	
	private GameObject player;              
	private PlayerInventory playerInventory;   

	void Awake ()
	{
		// Setting up the reference.
		player = GameObject.FindGameObjectWithTag(Tags.player);
		playerInventory = player.GetComponent<PlayerInventory> ();
	}

	void OnTriggerEnter (Collider other)
	{
		if (other.gameObject == player) 
		{
			playerInventory.nearSwitch = true;
		}
	}
	
	void OnTriggerStay (Collider other)
	{
		if (other.gameObject == player) 
		{
			if(Input.GetButton("Switch"))
			{
				LaserDeactivation();
			}
		}
	}

	void OnTriggerExit (Collider other)
	{
		if (other.gameObject == player) 
		{
			playerInventory.nearSwitch = false;
		}
	}
	
	void LaserDeactivation ()
	{
		// Deactivate the laser GameObject.
		laser.SetActive(false);
		
		// Store the renderer component of the screen.
		Renderer screen = transform.Find("prop_switchUnit_screen_001").renderer;
		
		// Change the material of the screen to the unlocked material.
		screen.material = unlockedMat;
		
		// Play switch deactivation audio clip.
		audio.Play();
	}
}