using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed;
    public LayerMask solidObjectsLayer;
    public LayerMask grassLayer;

    public event Action OnEncountered;

    private bool isMoving;
    private Vector2 input;

    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    //changed name to handle update so it won't be called automatically by unity
    public void HandleUpdate()
    {
        if (!isMoving)
        {
            input.x = Input.GetAxisRaw("Horizontal");
            input.y = Input.GetAxisRaw("Vertical");

            //possibly remove/stops diagonal movement
           
            if(input.x != 0)
            {
                input.y = 0;
            }
           

            if(input != Vector2.zero)
            {
                //sets the integers for the tranitions parameters for the animator object
                animator.SetFloat("moveX", input.x);
                animator.SetFloat("moveY", input.y);

                var targetPos = transform.position;
                targetPos.x += input.x;
                targetPos.y += input.y;

                if (isWalkable(targetPos))
                {
                    StartCoroutine(Move(targetPos));
                }
            }
        }

        //sets the bool for the transition parameter 
        animator.SetBool("isMoving", isMoving);
    }


    IEnumerator Move(Vector3 targetPos)
    {
        isMoving = true;

        while((targetPos - transform.position).sqrMagnitude > Mathf.Epsilon)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);
            yield return null;
        }
        transform.position = targetPos;

        isMoving = false;

        CheckForEncounters();
    }

    /*
     * This function checks if the position passed is within a certain radius of something on the solid objects layer,
     * then it can't be walked on. 
     */
    private bool isWalkable(Vector3 targetPos)
    {
        if(Physics2D.OverlapCircle(targetPos, 0.2f, solidObjectsLayer) != null){
            return false;
        }

        return true;
    }


    /*
     * This function checks for encounters, set for line 95 making it a 10% chance you will run into a pokemon!
     * This uses the grass layer as the layer to determine if it is on it.
     */
    private void CheckForEncounters()
    {
        //This checks the position of the player is not on grass or the grass layer made.
        if(Physics2D.OverlapCircle(transform.position, 0.2f, grassLayer) != null)
        {
            if(UnityEngine.Random.Range(1,101) % 10 == 0)
            {
                //stops user from moving
                animator.SetBool("isMoving", false);
                OnEncountered();
            }
        }
    }
}
