using UnityEngine;
using System.Collections;

// Create with a source and target translation
public class TranslateRotateTransition : Transition
{
	public TranslateRotateTransition()
	{
		m_TranslateTransition = new TranslateTransition();
		m_RotateTransition = new RotateTransition();
	}
	
	public TranslateRotateTransition(TranslateTransition translateTransition,
	                                 RotateTransition rotateTransition)
	{
		m_TranslateTransition = translateTransition;
		m_RotateTransition = rotateTransition;
	}
	
	public void Set(TranslateTransition translateTransition,
	                RotateTransition rotateTransition)
	{
		m_TranslateTransition = translateTransition;
		m_RotateTransition = rotateTransition;
	}
	
	public void Set(Vector3 positionFrom,
	                Quaternion rotationFrom,
	                Vector3 positionTo,
	                Quaternion rotationTo,
	                float duration)
	{
		m_TranslateTransition.Set(positionFrom, positionTo, duration);
		m_RotateTransition.Set(rotationFrom, rotationTo, duration);
	}
	
	public bool Interpolate(float deltaTime)
	{
		bool stillLerping = m_TranslateTransition.Interpolate(deltaTime);
		stillLerping = m_RotateTransition.Interpolate(deltaTime) || stillLerping;
		
		return stillLerping;
	}
	
	public TranslateTransition Translate
	{
		get { return m_TranslateTransition; }
	}
	
	public RotateTransition Rotate
	{
		get { return m_RotateTransition; }
	}
	
	private TranslateTransition m_TranslateTransition;
	private RotateTransition m_RotateTransition;
}
