using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiamondsController : MonoBehaviour
{

	private List<Transform> pickUpsList = new List<Transform>();


	void Start()
    {
		for (int i = 0; i < this.transform.childCount; i++)
		{
			pickUpsList.Add(this.transform.GetChild(i));
		}
		Debug.Log("diamonds count = " + pickUpsList.Count);
	}


	public int TotalCoinsCount()
	{
		return pickUpsList.Count;
	}


	public void SetIsPaused(bool value)
	{
		foreach (Transform item in pickUpsList)
		{
			item.GetComponent<Diamonds>().SetIsPaused(value);
		}
	}


	public void OnTouchPlayer(Collider _collider)
	{
		foreach (Transform item in pickUpsList)
		{
			if (item.gameObject.name == _collider.gameObject.name)
			{
				item.transform.localScale = Vector3.zero;
				Debug.Log("Diamond handled: " + item.gameObject.name);
				break;
			}
		}
	}


	public void OnReset()
	{
		foreach (Transform item in pickUpsList)
		{
			item.transform.localScale = Vector3.one;
		}
	}
}
