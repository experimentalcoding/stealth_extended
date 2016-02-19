using UnityEngine;
using System.Collections;

// Create with a source and target rotation
public class RotateTransition : Transition
{
	public RotateTransition()
	{
		m_RotationFrom = new Quaternion();
		m_RotationTo = new Quaternion();
	}
	
	public RotateTransition(Quaternion rotationFrom,
	                        Quaternion rotationTo,
	                        float duration)
	{
		m_RotationFrom = rotationFrom;
		m_RotationTo = rotationTo;
	}
	
	public void Set(Quaternion rotationFrom,
	                Quaternion rotationTo,
	                float duration)
	{
		m_TransitionDuration = duration;
		m_LerpT = 0f;
		m_RotationFrom = rotationFrom;
		m_RotationTo = rotationTo;
	}
	
	// Interpolate returns true if still interpolating
	public bool Interpolate(float deltaTime)
	{
		m_Rotation = Quaternion.Slerp(m_RotationFrom, m_RotationTo, m_LerpT);
		m_LerpT += deltaTime / m_TransitionDuration;
		
		return m_LerpT < 1f;
	}
	
	public Quaternion Rotation
	{
		get { return m_Rotation; }
	}
	
	private Quaternion m_Rotation;
	private Quaternion m_RotationFrom;
	private Quaternion m_RotationTo;
	
	private float m_TransitionDuration;
	private float m_LerpT;
}