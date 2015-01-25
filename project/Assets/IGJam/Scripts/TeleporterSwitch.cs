using UnityEngine;
using System.Collections;

public class TeleporterSwitch : MonoBehaviour 
{
    public bool isOn = false;
    public SmallTeleporter teleporterToControl;

    private Animator animator;
    private int triggererCount = 0;

    private void Start()
    {
        animator = GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        if(isOn)
        {
            animator.Play("Active");
        }
        else
        {
            animator.Play("Idle");
        }
    }

    private void OnSwitchTriggered(Collider other)
    {
        WorldObject worldObject = other.GetComponent<WorldObject>();
        if (worldObject && worldObject.objectType == WorldObject.ObjectType.INTERACTIVE)
        {
            triggererCount++;
            teleporterToControl.isActive = true;
            isOn = true;
            worldObject.OnDestroyed += OnTriggererDestroyed;
        }
    }

    private void OnSwitchTriggeredExit(Collider other)
    {
        WorldObject worldObject = other.GetComponent<WorldObject>();
        if (worldObject && worldObject.objectType == WorldObject.ObjectType.INTERACTIVE)
        {
            triggererCount = Mathf.Max(0, --triggererCount);
            worldObject.OnDestroyed -= OnTriggererDestroyed;
            if (triggererCount == 0)
            {
                teleporterToControl.isActive = false;
                isOn = false;
            }
        }
    }

    private void OnTriggererDestroyed()
    {
        triggererCount = Mathf.Max(0, --triggererCount); 
        if (triggererCount == 0)
        {
            teleporterToControl.isActive = true;
            isOn = true;
        }
    }
}
