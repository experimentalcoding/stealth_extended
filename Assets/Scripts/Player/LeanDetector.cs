using UnityEngine;
using System.Collections;

// Had meant for this class to contain more functionality
// but it ended up just being used to determine 
// whether an active collider should trigger a leaning state.
// This can be removed by setting the tag on the collider's 
// game object which would be an improvement since GetComponent 
// would not have to be called. (See PlayerMovement::OnTriggerEnter)
public class LeanDetector : MonoBehaviour
{
	public GameObject cameraAnchor;
	public GameObject characterAnchor;
}
