using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastColorChange : MonoBehaviour
{
     public Color hitColor = Color.green;
    public Color defaultColor = Color.red;

    private GameObject previousHitObject;

    void Update()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit))
        {
            if (hit.transform.gameObject != previousHitObject)
            {
                if (previousHitObject != null)
                {
                    Renderer renderer = previousHitObject.GetComponent<Renderer>();
                    if (renderer != null)
                    {
                        renderer.material.color = defaultColor;
                    }
                }

                previousHitObject = hit.transform.gameObject;

                Renderer hitRenderer = hit.transform.GetComponent<Renderer>();
                if (hitRenderer != null)
                {
                    hitRenderer.material.color = hitColor;
                }
            }
        }
        else
        {
            if (previousHitObject != null)
            {
                Renderer renderer = previousHitObject.GetComponent<Renderer>();
                if (renderer != null)
                {
                    renderer.material.color = defaultColor;
                }

                previousHitObject = null;
            }
        }
    }
}
