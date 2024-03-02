using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Unit : MonoBehaviour, IComparable
{
	public string unitName;
	public int level;
	public float maxHealth;
	public float currentHealth;
	public float damage;
	public float speed;
	public float stamina;
	public float fishingPower;
	public Image hudImage;

	public int nextActTurn;
	private bool dead = false;
	public void calculateNextActTurn(int currentTurn)
	{
		this.nextActTurn = currentTurn + (int)Math.Ceiling(100.0f / this.speed);
	}
	public int CompareTo(object otherStats)
	{
		return nextActTurn.CompareTo(((Unit)otherStats).nextActTurn);
	}
	public bool isDead()
	{
		return this.dead;
	}
}
