using UnityEngine;
using System.Collections;

// ItemPickup class was originally KeyPickup,
// For original file see: https://unity3d.com/learn/tutorials/projects/stealth/the-key
//
// Main change was to rename to ItemPickup and for it to store an enum representing the item type.
// This is set as a string in the editor, and parsed in Awake. 
// It is used to use the same code as picking up the key to pickup the mines 
public class ItemPickup : MonoBehaviour
{
	enum ItemType { Key, Mine };

    public AudioClip keyGrab;                    // Audioclip to play when the key is picked up.
	public string itemTypeString;

    private GameObject player;                      
    private PlayerInventory playerInventory;        
	private ItemType itemType;

    void Awake()
    {
        // Setting up the references.
        player = GameObject.FindGameObjectWithTag(Tags.player);
        playerInventory = player.GetComponent<PlayerInventory>();

		try 
		{
			itemType = (ItemType)System.Enum.Parse (typeof(ItemType), itemTypeString);
		} 
		catch (System.Exception ex) 
		{
			Debug.LogError(string.Format("Could not parse item type, has it been set in the editor? Exception: {0}", ex.ToString()));
		}
    }

    void OnTriggerEnter(Collider other)
    {
        // If the colliding gameobject is the player...
        if (other.gameObject == player)
        {
            // ... play the clip at the position of the key...
            AudioSource.PlayClipAtPoint(keyGrab, transform.position);

            // update player inventory
			switch (itemType) 
			{
				case ItemType.Key:
					playerInventory.hasKey = true;
					break;
				case ItemType.Mine:
					playerInventory.hasMines = true;
					break;
				default:
				break;
			}

            // ... and destroy this gameobject.
            Destroy(gameObject);
        }
    }
}