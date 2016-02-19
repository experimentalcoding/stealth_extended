using UnityEngine;
using System.Collections;


public class Leaning : MoveState 
{
	public Leaning(PlayerMovement playerMovement)
	{
		m_NextState = State;
		m_PlayerMovement = playerMovement;
		//m_PlayerMovement.anim.SetBool(m_PlayerMovement.hash.leaningBool, true);
	}
	
	public override MovementState State
	{
		get
		{
			return MoveState.MovementState.Leaning;
		}
	}
	
	public override MoveState.MovementState HandleInput(float horizontal, float vertical)
	{
		// Default to stay in current state
		m_NextState = State;

		// Make sure player's not playing walk/run animation.
		m_PlayerMovement.anim.SetFloat(m_PlayerMovement.hash.speedFloat, 0);
	
		LeanDetector activeLeanDetector = m_PlayerMovement.ActiveLeanDetector;
		if (activeLeanDetector && HasDirectionalInput(horizontal, vertical)) 
		{
			Vector3 targetDirection = activeLeanDetector.transform.forward;
			Vector3 inputDir = new Vector3(horizontal, 0f, vertical);
			float rotateTo = Mathf.Rad2Deg * m_PlayerMovement.RadiansToRotateToTarget(targetDirection, inputDir);

			if (HasDirectionalInput(horizontal, vertical) && rotateTo <= m_PlayerMovement.leanEndRotateThreshold) 
			{
				// todo: get out of state
				m_NextState = MovementState.Free;
			}
		}

		return m_NextState;
	}
}
