using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    // Camera Settings
    public Vector2 minRoomBounds;
    public Vector2 maxRoomBounds;

    public GameObject target;

	private void Start()
	{
		if (target == null)
		{
			// Get the player
			target = FindObjectOfType<PlayerMovement>().gameObject;
			GameManager.Instance.CameraManager.AssignTarget(target);
		}
		GameManager.Instance.CameraManager.minRoomBounds = minRoomBounds;
		GameManager.Instance.CameraManager.maxRoomBounds = maxRoomBounds;
	}

	// More possible items here (not implemented yet)

	// How many enemies needed to continue

	// How many enemies should spawn here

	// Can enemies spawn here
}
