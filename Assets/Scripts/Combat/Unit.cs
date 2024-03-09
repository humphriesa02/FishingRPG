using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Unit : MonoBehaviour
{
	public string unitName;
	public int level;
	public float maxHealth;
	public float currentHealth;
	public float damage;
	public float speed;
	public float stamina;
	public float fishingPower;
	public Sprite hudImage;

	public bool isDead;

	public bool TakeDamage(float dmg)
	{
		currentHealth -= dmg;

		if (currentHealth <= 0)
		{
			isDead = true;
		}
		else
		{
			isDead = false;
		}

		return isDead;
	}
}
