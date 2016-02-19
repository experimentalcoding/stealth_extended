using UnityEngine;
using System.Collections;

// The PlayerMovement class is used to both control the players movement in each direction,
// but it also manages changing states from moving freely to leaning up against a wall 
public class PlayerMovement : MonoBehaviour
{
	// Event handler that dispatches events that the player has changed state
	public delegate void MovementStateChangedEventHandler (PlayerMovement playerMovement);

    public AudioClip shoutingClip;      
    public float turnSmoothing = 15f;   
    public float speedDampTime = 0.1f;  
	public float leanStartRotateThreshold = 2.5f;
	public float leanEndRotateThreshold = 30f;

	[HideInInspector]
	public Animator anim;              // Reference to the animator component.

	[HideInInspector]
	public HashIDs hash;               // Reference to the HashIDs.
	public FollowCamera camera;		   // Reference to camera, if camera is transitioning then can't change state

	// The LeanDetector component is attached to a box collider which 
	public LeanDetector ActiveLeanDetector { get; private set; }

	// Used for calculating how long player has pushed against wall to trigger lean
	private float m_LeanStartTime;			

	// MoveState property, each state is a separate class and is updated in MovementManagement function
	private MoveState moveState;
	public MoveState CurrentState 
	{
		get { return moveState; }
	}

	// Trigger function for 
	protected virtual void OnMovementStateChanged()
	{
		if (MovementStateChanged != null) 
		{
			MovementStateChanged(this);
		}
	}

	// Event for state changing
	public event MovementStateChangedEventHandler MovementStateChanged;

    void Awake()
    {
        // Setting up the references.
        anim = GetComponent<Animator>();
        hash = GameObject.FindGameObjectWithTag(Tags.gameController).GetComponent<HashIDs>();

        // Set the weight of the shouting layer to 1.
        anim.SetLayerWeight(1, 1f);

		// Set initial state
		moveState = new FreeMove(this);
   }

    void FixedUpdate()
    {
        // Cache the inputs.
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
		bool sneak = Input.GetButton("Sneak");

        MovementManagement(h, v, sneak);
    }

    void Update()
    {
        // Cache the attention attracting input.
        bool shout = Input.GetButtonDown("Attract");

        // Set the animator shouting parameter.
        anim.SetBool(hash.shoutingBool, shout);

        AudioManagement(shout);
    }

    void MovementManagement(float horizontal, float vertical, bool sneaking)
    {
        // Set the sneaking parameter to the sneak input.
        anim.SetBool(hash.sneakingBool, sneaking);

		// If have a state set, then update input and see if this triggers a state change
		if (moveState != null) 
		{
			// SetState will only change state if different to current
			if (!camera.IsTransitioning()) 
			{
				MoveState.MovementState nextState = moveState.HandleInput(horizontal, vertical);
				SetState(nextState);
			}
		}
    }

	// Factory function which returns a new state instance based on enum passed in.
	// Can be improved by caching each state instance on startup and setting to active/inactive,
	// which would these unnecessary allocations on every state change
	public MoveState GetNewState(MoveState.MovementState state)
	{
		MoveState newState = null;
		switch (state) 
		{
			case MoveState.MovementState.Free:
				newState = new FreeMove(this);
				break;
			case MoveState.MovementState.RotatingToLean:
				newState = new RotatingToLean(this);
				break;
			case MoveState.MovementState.PushingAgainstWall:
				newState = new PushingAgainstWall(this);
				break;
			case MoveState.MovementState.Leaning:
				newState = new Leaning(this);
				break;
			default:
					break;
			}

		return newState;
	}

	public void Rotating(float horizontal, float vertical, float extraSmoothScale = 1f)
	{
		// Create a new vector of the horizontal and vertical inputs.
		Vector3 targetDirection = new Vector3(horizontal, 0f, vertical);
		
		// Create a rotation based on this new vector assuming that up is the global y axis.
		Quaternion targetRotation = Quaternion.LookRotation(targetDirection, Vector3.up);
		Quaternion newRotation = Quaternion.Lerp(rigidbody.rotation, targetRotation, turnSmoothing * extraSmoothScale * Time.deltaTime);
		
		// Change the players rotation to this new rotation.
		rigidbody.MoveRotation(newRotation);
	}

	// SetState switches to a new state if it doesn't match the current state.
	// It also calls OnMovementStateChanged which triggers event handler to 
	// inform FollowCamera component of the change in state
	public void SetState(MoveState.MovementState state)
	{
		if (moveState == null || state != moveState.State) 
		{
			moveState = GetNewState(state);
			OnMovementStateChanged ();
		}
	}

	public void SetSpeed(float speed)
	{
		anim.SetFloat(hash.speedFloat, speed, speedDampTime, Time.deltaTime);
	}

    void AudioManagement(bool shout)
    {
        // If the player is currently in the run state...
        if (anim.GetCurrentAnimatorStateInfo(0).nameHash == hash.locomotionState)
        {
            // ... and if the footsteps are not playing...
            if (!audio.isPlaying)
                // ... play them.
                audio.Play();
        }
        else
            // Otherwise stop the footsteps.
            audio.Stop();

        // If the shout input has been pressed...
        if (shout)
            // ... play the shouting clip where we are.
            AudioSource.PlayClipAtPoint(shoutingClip, transform.position);
    }

	public float RadiansToRotateToTarget(Vector3 targetDirection)
	{
		return RadiansToRotateToTarget(targetDirection, rigidbody.transform.forward.normalized); // todo: normalized needed?
	}

	public float RadiansToRotateToTarget(Vector3 targetDirection, Vector3 directionToCompare)
	{
		// Get dot product and clamp it before calling Mathf.Acos, can get NaN if slightly outside of range
		float dotProduct = Vector3.Dot(targetDirection, directionToCompare); 
		return Mathf.Acos(Mathf.Clamp(dotProduct, -1f, 1f));
	}

	public void OnTriggerEnter(Collider other)
	{
		LeanDetector detector = other.GetComponent<LeanDetector>();
		if (detector)
		{
			ActiveLeanDetector = detector;
		}
	}

	public void OnTriggerStay(Collider other)
	{
		LeanDetector detector = other.GetComponent<LeanDetector>();
		if (detector)
		{
			ActiveLeanDetector = detector;
		}
	}
	
	public void OnTriggerExit(Collider other)
	{
		LeanDetector detector = other.GetComponent<LeanDetector>();
		if (detector == ActiveLeanDetector)
		{
			ActiveLeanDetector = null;
		}
	}
}