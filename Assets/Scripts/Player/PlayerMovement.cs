using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float baseSpeed;
	[SerializeField] private float runSpeed;

	private float moveHorizontal = 0f;
	private float moveVertical = 0f;
	public Vector3 moveDirection { get; private set; }

	/**
     * Components
     */
	private Animator anim;
	private Rigidbody2D rb;

	private void Awake()
	{
		if (anim == null)
        {
            anim = GetComponent<Animator>();
        }
		if (rb == null)
		{
			rb = GetComponent<Rigidbody2D>();
		}
		anim.SetFloat("idleDirectionX", GameManager.Instance.PlayerManager.playerStartingDirection.x);
		anim.SetFloat("idleDirectionY", GameManager.Instance.PlayerManager.playerStartingDirection.y);
	}

	private void Update()
	{
		// Get Input
		moveHorizontal = Input.GetAxisRaw("Horizontal");
		moveVertical = Input.GetAxisRaw("Vertical");

		// Set move direction vector
		moveDirection = new Vector3(moveHorizontal, moveVertical, 0);
		moveDirection.Normalize();

		// Set direction for anim
		anim.SetFloat("moveDirectionX", moveHorizontal);
		anim.SetFloat("moveDirectionY", moveVertical);
		if (moveDirection.magnitude > 0)
		{
			anim.SetFloat("idleDirectionX", moveHorizontal);
			anim.SetFloat("idleDirectionY", moveVertical);
		}
		anim.SetFloat("velocity", moveDirection.magnitude);
	}

	private void FixedUpdate()
	{
		rb.MovePosition(transform.position + moveDirection * baseSpeed * Time.deltaTime);
	}
}
