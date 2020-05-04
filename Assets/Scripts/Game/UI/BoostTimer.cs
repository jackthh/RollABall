using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BoostTimer : MonoBehaviour
{

	private Animator animator;
	private Slider boostTimeSlider;
	public Text boostTimeText;
	public Text boostMultText;


    void Start()
    {
		animator = GetComponent<Animator>();  
		boostTimeSlider = GetComponentInChildren<Slider>();
	}


	public void SetMaxBoostTime(float _maxBoostTime)
	{
		this.boostTimeSlider.maxValue = _maxBoostTime;
	}


	public void OnBoostTimeRemainingChange(float _boostTimeRemaining)
	{
		float value = Mathf.Clamp(_boostTimeRemaining, 0f, boostTimeSlider.maxValue);

		this.boostTimeSlider.value = value;
		this.boostTimeText.text = value.ToString("0.00");

		if (value == 0)
		{
			OnTimeOver();
		}
	}


	public void SetBoostMult(float _boostMult)
	{
		this.boostMultText.text = "x " + _boostMult.ToString();
	}


	public void OnGameStart()
	{
		animator.SetTrigger("TimeStart");
	}


	public void OnTimeOver()
	{
		animator.SetTrigger("TimeOver");
	}


	public void OnReset()
	{
		boostTimeSlider.value = boostTimeSlider.maxValue;
		this.OnBoostTimeRemainingChange(boostTimeSlider.maxValue);
	}
}
