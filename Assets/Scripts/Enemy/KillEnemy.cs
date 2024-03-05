using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillEnemy : MonoBehaviour
{
    public GameObject menuItem;

	private void OnDestroy()
	{
		if(menuItem != null)
		{
			Destroy(this.menuItem);
		}
	}
}
