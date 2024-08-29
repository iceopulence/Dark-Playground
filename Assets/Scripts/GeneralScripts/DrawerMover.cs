using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DrawerMover : MonoBehaviour, IInteractable
{
    [SerializeField] private float moveDistance = 1f;
    [SerializeField] private float moveDuration = 1.0f;
    [SerializeField] private AudioClip openSound;
    [SerializeField] private AudioClip closeSound;

    private Vector3 startPosition;
    private Vector3 targetPosition;
    private bool isMoving = false;
    private bool isOpen = false;
    private float startTime;

    List<GameObject> itemsInDrawer;

    void Awake()
    {
        startPosition = transform.position;
        targetPosition = startPosition + -transform.up * moveDistance;

        if (transform.childCount > 0)
        {
            InitDrawerItems();
        }


    }

    void InitDrawerItems()
    {
        itemsInDrawer = new List<GameObject>();
        foreach (Transform child in transform)
        {
            if (child != this.transform)
            {
                itemsInDrawer.Add(child.gameObject);
              child.gameObject.SetActive(false);
              

              DropItemBehaviour drop = child.GetComponent<DropItemBehaviour>();
                if(drop == null)
                {
                    child.gameObject.AddComponent<DropItemBehaviour>();
                }

            }
        }

    }
        void ReleaseDrawerItems()
        {
            transform.DetachChildren();

            foreach( GameObject item in itemsInDrawer)
            {
                 item.SetActive(true);

            }
                itemsInDrawer = null;
        }


    void FixedUpdate()
    {
        if (isMoving)
        {
            float t = (Time.time - startTime) / moveDuration;
            transform.position = Vector3.Lerp(isOpen ? targetPosition : startPosition, isOpen ? startPosition : targetPosition, t);

            if (t > 1.0f)
            {
                isMoving = false;
                isOpen = !isOpen;
                if( isOpen && itemsInDrawer.Count > 0)
                {
                    ReleaseDrawerItems();
                }
            }
        }
    }

    public void OnInteract(PlayerInteraction interactor)
    {
        StartMoving();
    }

    private void StartMoving()
    {
        if (!isMoving)
        {
            isMoving = true;
            startTime = Time.time;

            AudioClip moveClip = isOpen ? closeSound : openSound;
            AudioSource.PlayClipAtPoint(moveClip, transform.position);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawLine(startPosition, targetPosition);
    }
}
