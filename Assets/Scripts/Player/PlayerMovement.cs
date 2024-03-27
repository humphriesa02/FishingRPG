using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerState
{
	Default,
	Menu
}

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float baseSpeed;
	[SerializeField] private float runSpeed;

	private float moveHorizontal = 0f;
	private float moveVertical = 0f;
	public Vector3 moveDirection { get; private set; }

	private PlayerState state;
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
		if (GameManager.Instance.PlayerManager.playerStartingDirection == Vector2.zero) anim.SetFloat("idleDirectionY", -1);
		else anim.SetFloat("idleDirectionY", GameManager.Instance.PlayerManager.playerStartingDirection.y);

		state = PlayerState.Default;
	}

	private void Update()
	{
		switch (state)
		{
			case PlayerState.Default:
				HandleDefaultState();
				break;
			case PlayerState.Menu:
				HandleMenuState();
				break;
		}

		PlayAnimations();
	}

	private void HandleDefaultState()
	{
		// Get Input
		moveHorizontal = Input.GetAxisRaw("Horizontal");
		moveVertical = Input.GetAxisRaw("Vertical");

		if (Mathf.Abs(moveHorizontal) > Mathf.Abs(moveVertical))
		{
			moveVertical = 0;
		}
		else
		{
			moveHorizontal = 0;
		}
		// Set move direction vector
		moveDirection = new Vector3(moveHorizontal, moveVertical, 0);
		moveDirection.Normalize();
	}

	private void PlayAnimations()
	{
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

	private void HandleMenuState()
	{
		// Menu Handles state
		moveDirection = Vector2.zero;
	}

	public void ChangePlayerState(PlayerState newState)
	{
		state = newState;
	}

	private void FixedUpdate()
	{
		switch (state)
		{
			case PlayerState.Default:
				rb.MovePosition(transform.position + moveDirection * baseSpeed * Time.deltaTime);
				break;
			case PlayerState.Menu:
				break;
		}
	}
}
