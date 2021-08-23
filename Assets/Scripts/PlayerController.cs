using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

	public float movementSpeed;
	public Transform groundCheck;
	public float groundCheckRadius;
	public LayerMask groundLayer;
	public float minJumpPressure;
	public float maxJumpPressure;

	private bool isFacingRight = true;
	private float movementInputDirection;
	private Rigidbody2D rb;
	private bool isGrounded;
	private float jumpPressure;


    void Start()
    {
		rb = GetComponent<Rigidbody2D>();
    }


    void Update()
    {
		CheckInput();
		CheckMovementDirection();
    }

	void FixedUpdate()
	{
		ApplyMovement();
		CheckSurroundings();
	}

	/* Checks */

	private void CheckInput()
	{
		movementInputDirection = Input.GetAxisRaw("Horizontal");

		if(isGrounded)
		{
			if (Input.GetButton("Jump"))
			{
				if (jumpPressure < maxJumpPressure)
				{
					jumpPressure += Time.deltaTime * 10;
				}
				else
				{
					jumpPressure = maxJumpPressure;
				}
			}
			else
			{
				if (jumpPressure > 0)
				{
					jumpPressure = jumpPressure + minJumpPressure;
					Jump();
				}
			}
		}
	}

	private void CheckMovementDirection()
	{
		if(isFacingRight && movementInputDirection < 0)
		{
			Flip();
		}else if(!isFacingRight && movementInputDirection > 0)
		{
			Flip();
		}
	}

	private void CheckSurroundings()
	{
		isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
	}

	/* Movements */

	private void ApplyMovement()
	{
		rb.velocity = new Vector2(movementSpeed * movementInputDirection, rb.velocity.y);
	}

	private void Flip()
	{
		isFacingRight = !isFacingRight;
		transform.Rotate(0, 180, 0);
	}

	private void Jump()
	{
		rb.velocity = new Vector2(rb.velocity.x, -jumpPressure);
		jumpPressure = 0;
	}

	private void OnDrawGizmos()
	{
		Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
	}


}
