using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerParty : MonoBehaviour
{
    public Unit[] playerUnits;

	private void Start()
	{
		playerUnits = GetComponentsInChildren<Unit>();
		SetPlayerName();
	}

	public List<Unit> GetPartyUnitsAsList()
	{
		return new List<Unit>(playerUnits);
	}

	public Unit[] GetPartyUnits()
	{
		return playerUnits;
	}

	// Called when a player chooses a name
	public void SetPlayerName()
	{
		string playerName = PlayerPrefs.GetString("PlayerName", "Player");
		playerUnits[0].unitName = playerName;
	}
}
