using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    private GameObject target;
	public Vector2 minRoomBounds;
	public Vector2 maxRoomBounds;

	// Update is called once per frame
	void Update()
    {
		if (target != null)
        {
            Vector3 newPosition = new Vector3(
                Mathf.Clamp(target.transform.position.x, minRoomBounds.x, maxRoomBounds.x),
				Mathf.Clamp(target.transform.position.y, minRoomBounds.y, maxRoomBounds.y),
                transform.position.z);
			transform.position = newPosition;
        }
    }

    public void AssignTarget(GameObject newTarget)
    {
        target = newTarget;
    }
}
