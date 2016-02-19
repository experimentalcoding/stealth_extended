using UnityEngine;
using System.Collections;

// Create with a source and target translation
public class TranslateTransition : Transition
{
	public TranslateTransition()
	{
		m_PositionFrom = new Vector3();
		m_PositionTo = new Vector3();
	}

	public TranslateTransition(Vector3 positionFrom,
	                           Vector3 positionTo,
	                           float duration)
	{
		m_PositionFrom = positionFrom;
		m_PositionTo = positionTo;
	}

	public void Set(Vector3 positionFrom,
	                Vector3 positionTo,
	                float duration)
	{
		m_TransitionDuration = duration;
		m_LerpT = 0f;
		
		m_PositionFrom = positionFrom;
		m_PositionTo = positionTo;
	}

	// Interpolate returns true if still interpolating
	public bool Interpolate(float deltaTime)
	{
		m_Position = Vector3.Lerp(m_PositionFrom, m_PositionTo, m_LerpT);
		
		// increase lerp amount and return whether still lerping
		m_LerpT += deltaTime / m_TransitionDuration;
		return m_LerpT < 1f;
	}
	
	public Vector3 Position
	{
		get { return m_Position; }
	}
	
	private Vector3 m_Position;
	private Vector3 m_PositionFrom;
	private Vector3 m_PositionTo;
	
	private float m_TransitionDuration;
	private float m_LerpT;
}