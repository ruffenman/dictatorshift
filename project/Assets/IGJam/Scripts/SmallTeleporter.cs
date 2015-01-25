using UnityEngine;
using System.Collections;

public class SmallTeleporter : MonoBehaviour 
{
    public bool isActive;

    private Animator animator;

    private void Start()
    {
        animator = GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        if(isActive)
        {
            animator.Play("Active");
        }
        else
        {
            animator.Play("Idle");
        }
    }

    private void OnTeleporterEntered()
    {
        if (isActive)
        {
            // YOU WINNN!
            JamGame.instance.player.ShuffleInputs();
        }
    }
}
