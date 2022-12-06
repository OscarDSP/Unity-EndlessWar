using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
	[SerializeField] Transform orientation;
	[Header("movement properties")]
	[SerializeField] private float moveSpeed = 6f;
	[SerializeField] private float moveMultiplier = 10f;
	[SerializeField] private float airMultiplier = 0.4f;
	[SerializeField] private float groundDrag = 6f;
	[SerializeField] private float airGround = 2f;
	[SerializeField] private AudioSource walking;
	[SerializeField] private Rigidbody rb;
	private Vector3 direction;
	private Vector3 slopeMoveDir;
	private float horizontal;
	private float playerHeight = 2f;
	private float vertical;
	public bool isGrounded;

	[Header("Sprinting properties")]
	[SerializeField] private float walkSpeed = 4f;
	[SerializeField] private float sprintSpeed = 6f;
	[SerializeField] private float acc = 10f;

	[Header("Jump properties")]
	[SerializeField] private float jumpForce = 5f;
	[SerializeField] private float groundDistance = 0.4f;
	[SerializeField] private Transform groundCheck;
	[SerializeField] private LayerMask WhatIsGround;

	[Header("Slope properties")]
	RaycastHit slopeHit;

	private void Start()
	{
		rb = GetComponent<Rigidbody>();
		rb.freezeRotation= true;
	}

	private void ControlDrag()
	{
		if (isGrounded)
			rb.drag = groundDrag;
		else
			rb.drag = airGround;
	}

	private void Update()
	{
		isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, WhatIsGround);
		InputSystem();
		ControlDrag();
		ControlSpeed();
		if(horizontal > 0 || horizontal < 0 || vertical > 0 || vertical < 0)
		{
			if(!walking.isPlaying)
				walking.Play();
		}
		else
		{
			walking.Stop();
		}

		slopeMoveDir = Vector3.ProjectOnPlane(direction, slopeHit.normal);
	}

	private void FixedUpdate()
	{
		MovementSystem();
	}

	private void InputSystem()
	{
		horizontal = Input.GetAxisRaw("Horizontal");
		vertical = Input.GetAxisRaw("Vertical");

		direction = orientation.forward * vertical + orientation.right * horizontal;
	}

	private void MovementSystem()
	{
		if(isGrounded && !OnSlope())
			rb.AddForce(direction.normalized * moveSpeed * moveMultiplier, ForceMode.Acceleration);
		else if(isGrounded && OnSlope())
			rb.AddForce(slopeMoveDir.normalized * moveSpeed * moveMultiplier, ForceMode.Acceleration);
		else if (!isGrounded)
			rb.AddForce(direction.normalized * moveSpeed * moveMultiplier * airMultiplier, ForceMode.Acceleration);
	}

	private void ControlSpeed()
	{
		if(Input.GetKey(KeyCode.LeftShift) && isGrounded)
			moveSpeed = Mathf.Lerp(moveSpeed, sprintSpeed, acc * Time.deltaTime);
		else
			moveSpeed = Mathf.Lerp(moveSpeed, walkSpeed, acc * Time.deltaTime);
	}

	private void JumpSystem()
	{
		rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
		rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
	}

	private bool OnSlope()
	{
		if (Physics.Raycast(transform.position, Vector3.down, out slopeHit, playerHeight / 2 + 0.5f))
		{
			if (slopeHit.normal != Vector3.up)
				return true;
		}
		else
			return false;
		return false;
	}
}
