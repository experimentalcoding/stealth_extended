using UnityEngine;
using System.Collections;

public class FreeMove : MoveState 
{
	public FreeMove(PlayerMovement playerMovement)
	{
		m_LeanStartTime = 0f;
		m_NextState = State;
		m_PlayerMovement = playerMovement;
		//m_PlayerMovement.anim.SetBool(m_PlayerMovement.hash.leaningBool, false);
	}

	public override MovementState State
	{
		get
		{
			return MoveState.MovementState.Free;
		}
	}

	private float m_LeanStartTime;

	public override MoveState.MovementState HandleInput(float horizontal, float vertical)
	{
		MoveState.MovementState nextState = State;
		if (CheckPushAgainstWall(horizontal, vertical))
		{
			//m_LeanTargetPosition = rigidbody.transform.position - (m_TargetRotation * m_PushToWallMagnitude);
			//m_PositionBeforeLean = rigidbody.transform.position;
			nextState = MoveState.MovementState.RotatingToLean;
		}
		
		if (HasDirectionalInput(horizontal, vertical)) 
		{
			// ... set the players rotation and set the speed parameter to 5.5f.
			m_PlayerMovement.Rotating(horizontal, vertical);
			m_PlayerMovement.SetSpeed(5.5f);
		} 
		else 
		{
			// Otherwise set the speed parameter to 0.
			m_PlayerMovement.anim.SetFloat(m_PlayerMovement.hash.speedFloat, 0);
		}	

		return nextState;
	}

	bool CheckPushAgainstWall(float horizontal, float vertical)
	{
		LeanDetector activeLeanDetector = m_PlayerMovement.ActiveLeanDetector;
		if (activeLeanDetector && HasDirectionalInput(horizontal, vertical)) 
		{
			Vector3 targetDirection = activeLeanDetector.transform.forward;
			Debug.Log (targetDirection.ToString());

			// If colliding with lean detector and pointing in opposite direction, mark time
			Vector3 playerToWall = -targetDirection;
			float toRotate = Mathf.Rad2Deg * m_PlayerMovement.RadiansToRotateToTarget(playerToWall);
			bool pointingTowardsWall = toRotate <= m_PlayerMovement.leanStartRotateThreshold;

			// Can start leaning if colliding with lean detector, and are pushing in against the wall
			bool startedToLean = 	activeLeanDetector && 
									HasDirectionalInput(horizontal, vertical) && 
									pointingTowardsWall ;
			
			// If not started to lean against wall yet, cache current time
			if (!startedToLean) 
			{
				m_LeanStartTime = Time.time;
			} 
			else// if (Time.time - m_LeanStartTime >= playerMovement.leanTriggerThreshold) 
			{
				return true;
			}
		}

		return false;
	}
}
