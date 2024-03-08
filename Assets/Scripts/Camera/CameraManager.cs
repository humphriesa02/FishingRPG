using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    private GameObject target;
	public Vector2 minRoomBounds;
	public Vector2 maxRoomBounds;

    public float shakeDuration = 1.5f;
    public float shakeMagnitude = 2f;

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

    public IEnumerator Shake (float duration, float magnitude)
    {
        Vector3 originalPos = transform.localPosition;

        float elapsed = 0f;

        while (elapsed < duration)
        {
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;

            transform.localPosition = new Vector3(x, y, originalPos.z);
            elapsed += Time.deltaTime;

            yield return null;
        }

        transform.localPosition = originalPos;
    }
}
