using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPickup : MonoBehaviour
{
    public Transform handTransform;
    public float pickupDistance = 2f;

    private GameObject heldObject;

    void Update()
    {
        // Check for pickup input
        if (Input.GetKeyDown(KeyCode.E))
        {
            // If holding an object, drop it
            if (heldObject != null)
            {
                heldObject.transform.parent = null;
                heldObject.GetComponent<Rigidbody>().isKinematic = false;
                heldObject = null;
            }
            else
            {
                // Otherwise, try to pick up an object
                RaycastHit hit;
                if (Physics.Raycast(transform.position, transform.forward, out hit, pickupDistance))
                {
                    GameObject hitObject = hit.collider.gameObject;
                    if (hitObject.CompareTag("Pickup"))
                    {
                        hitObject.transform.parent = handTransform;
                        hitObject.transform.localPosition = Vector3.zero;
                        hitObject.transform.localRotation = Quaternion.identity;
                        hitObject.GetComponent<Rigidbody>().isKinematic = true;
                        heldObject = hitObject;
                    }
                }
            }
        }
    }
}
