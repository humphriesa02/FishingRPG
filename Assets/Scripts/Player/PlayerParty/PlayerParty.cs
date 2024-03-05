using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerParty : MonoBehaviour
{
    private List<Unit> playerUnits;

	private void Start()
	{
		playerUnits = new List<Unit>(GetComponentsInChildren<Unit>());
	}

	public List<Unit> GetPartyUnits()
	{
		return playerUnits;
	}
}
