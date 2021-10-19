﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CloneMove : MonoBehaviour
{
    public bool isMovingSameDirection;
    public SpriteRenderer spriteRenderer;
    public Rigidbody2D rb;
    private bool canJump;
    private bool canMove;
	//public Text counterText; // Too dirty!

	public EventSystemCustom eventSystem;
	public Animator animator;
	private float speed;

	private void Awake()
    {
        canJump = true;
        canMove = true;
    }

	void Update()
	{
		animator.SetFloat("Speed", Mathf.Abs(speed));
		speed = 0;
	}

	public void Move(Vector3 vec, bool isDirRight)
    {
        if (!canMove)
            return;


		int factor = 1;
        if (isDirRight)
        {
            if (isMovingSameDirection)
            {
                spriteRenderer.flipX = false;
                factor = 1;
            }
            else
            {
                spriteRenderer.flipX = true;
                factor = -1;
            }
        }
        else
        {

            if (isMovingSameDirection)
            {
                spriteRenderer.flipX = true;
                factor = -1;
            }
            else
            {
                spriteRenderer.flipX = false;
                factor = 1;

            }
        }

        transform.position += vec * factor;
		speed += vec.x + 1;
	}

    public void Jump(float amount)
    {
        if (canJump)
            rb.AddForce(transform.up * amount, ForceMode2D.Impulse);
    }

	private void OnTriggerStay2D(Collider2D collision)
	{
		//if (collision.gameObject.CompareTag(TagNames.Key.ToString()))
		//{
		//	Debug.Log("clone near a key");
		//	if (Input.GetKey(KeyCode.E))
		//	{
		//		Debug.Log("clone entered E");
		//		eventSystem.OnKeyTrigger.Invoke();
		//		Debug.Log("OnKeyTrigger fired.");
		//		collision.gameObject.SetActive(false);
		//	}
		//}

		// check for teleport
		if (collision.gameObject.CompareTag(TagNames.Door.ToString()))
		{
			Door sourceDoor = collision.gameObject.GetComponent<Door>();
			Door destDoor = null;
			Debug.Log("clone near a door");

			if (sourceDoor.isSource)
			{
				// teleport without any condition
				Doors sourceNum = sourceDoor.doorNum; // source door number
				Debug.Log(sourceNum);

				// get all doors in order to find dest door
				GameObject[] objs = GameObject.FindGameObjectsWithTag("Door");
				foreach (var obj in objs)
				{
					Door d = obj.GetComponent<Door>();
					if (d.isSource)
						continue;

					// find dest door related to this door
					if (d.doorNum == sourceNum)
					{
						Debug.Log("found dest door for clone");
						destDoor = d;
						break;
					}
				}

				// transfer clone to destDoor
				this.transform.position = destDoor.transform.position;
			}
		}
	}

	private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag(TagNames.StickyPlatform.ToString()))
        {
            Debug.LogWarning("sticky for clone");

            // Updating the UI text. But this is not a clean way. We'll fix it later.
            /*int newTextValue = int.Parse(counterText.text) + 1;
            counterText.text = newTextValue.ToString();*/

            // This is used by UiManager
            eventSystem.OnCloneStickyPlatformEnter.Invoke();
            Debug.Log("OnCloneStickyPlatformEnter fired.");

            canJump = false;
			canMove = false;

		}
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag(TagNames.StickyPlatform.ToString()))
        {
            Debug.LogWarning("sticky no more for clone bruh");
            canJump = true;
        }
    }
}
