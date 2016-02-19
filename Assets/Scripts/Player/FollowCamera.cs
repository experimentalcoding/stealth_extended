using UnityEngine;
using System.Collections;
using System;

// The FollowCamera class is used to both follow the player while he runs,
// as-well as smoothly translating and rotating to a desired transformation.
// This is used to provide a good view around a corner while 
// the player leans up against a wall
public class FollowCamera : MonoBehaviour
{
	public float followSpeed = 0f;
	public float transitionDuration = .5f;
	public GameObject targetObject = null;

	private Transform player;
	private Vector3 defaultOffset;
	private Quaternion defaultRotation;
	private Vector3 offset;
	private TranslateRotateTransition currentTransition;		
	
	public enum CameraState
	{
		FollowingPlayer,
		TransitioningToPeeking,
		LeaningView,
		TransitioningToFollowing
	};
	
	public CameraState State { get; set; }

	public bool IsTransitioning()
	{
		return  State == CameraState.TransitioningToFollowing || 
				State == CameraState.TransitioningToPeeking;
	}

	void Awake()
	{
		// Setting up the reference.
		player = GameObject.FindGameObjectWithTag(Tags.player).transform;
	}

	void Start()
	{		
		// Default to original offset from player
		defaultOffset = offset = transform.position - player.transform.position;
		defaultRotation = transform.rotation;
		currentTransition = new TranslateRotateTransition();
		
		// Subscribe to lean state changed events
		player.GetComponent<PlayerMovement>().MovementStateChanged += FollowCamera_MovementStateChanged;
	}
	
	private void FollowCamera_MovementStateChanged(PlayerMovement character)
	{
		// Can only trigger a new camera transition if not transitioning currently
		if (!IsTransitioning())
		{
			if (character.CurrentState.State == MoveState.MovementState.Leaning)
			{
				// Set target object with transform to transition to
				LeanDetector detector = character.ActiveLeanDetector;
				targetObject = detector.cameraAnchor;
				
				// cache default values
				defaultOffset = offset;
				defaultRotation = transform.rotation;
				
				// calculate new offset from players position
				Vector3 newOffset = targetObject.transform.position - player.transform.position;
				
				// set the current transition instance to interpolate from
				// the default offset/rotation to that of the target object
				currentTransition.Set(defaultOffset,
				                        defaultRotation,
				                        newOffset,
				                        targetObject.transform.rotation,
				                        transitionDuration);
				
				State = CameraState.TransitioningToPeeking;
			}
			else if (character.CurrentState.State == MoveState.MovementState.Free)
			{
				if (State == CameraState.LeaningView)
				{
					// set the current transition instance to interpolate from
					// the default offset/rotation to that of the target object
					currentTransition.Set(offset,
					                        transform.rotation,
					                        defaultOffset,
					                        defaultRotation,
					                        transitionDuration);
					targetObject = null;
					State = CameraState.TransitioningToFollowing;
				}
			}
		}
	}

	// Perform any updates in LateUpdate to wait for physics to end
	// If in a transitioning state, then update the transition instance
	// by calling Interpolate
	void LateUpdate()
	{
		switch (State)
		{
			case CameraState.FollowingPlayer:
				break;
			case CameraState.TransitioningToFollowing:
				goto case CameraState.TransitioningToPeeking;
			case CameraState.TransitioningToPeeking:
				
				// If transitioning, need to update the current transition instance to apply interpolations
				ProcessTransition();
				break;
			case CameraState.LeaningView:
				break;
			default:
				break;
		}

		// Ensure smooth camera positioning by lerping to desired target
		Vector3 desiredPosition = offset + player.position;
		Vector3 position = Vector3.Lerp(transform.position, desiredPosition, Time.deltaTime * followSpeed);

		transform.position = position;
	}
	
	void ProcessTransition()
	{
		if (currentTransition.Interpolate(Time.deltaTime))
		{
			offset = currentTransition.Translate.Position;
			transform.localRotation = currentTransition.Rotate.Rotation;
		}
		else
		{
			OnTransitionFinished();
		}
	}
	
	void OnTransitionFinished()
	{
		if (State == CameraState.TransitioningToFollowing)
		{
			State = CameraState.FollowingPlayer;
		}
		else if (State == CameraState.TransitioningToPeeking)
		{
			State = CameraState.LeaningView;
		}
	}
}