using UnityEngine;
using System.Collections;

public class ItemPickup : MonoBehaviour
{
	enum ItemType { Key, Mine };

    public AudioClip keyGrab;                       // Audioclip to play when the key is picked up.
	public string itemTypeString;

    private GameObject player;                      // Reference to the player.
    private PlayerInventory playerInventory;        // Reference to the player's inventory.
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