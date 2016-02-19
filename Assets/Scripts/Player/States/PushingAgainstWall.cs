using UnityEngine;
using System.Collections;

public class PushingAgainstWall : MoveState 
{
	public PushingAgainstWall(PlayerMovement playerMovement)
	{
		m_NextState = State;
		m_PlayerMovement = playerMovement;
		//m_PlayerMovement.anim.SetBool(m_PlayerMovement.hash.leaningBool, false);
	}
	
	public override MovementState State
	{
		get
		{
			return MoveState.MovementState.PushingAgainstWall;
		}
	}
	
	public override MoveState.MovementState HandleInput(float horizontal, float vertical)
	{
		return State;
	}
}