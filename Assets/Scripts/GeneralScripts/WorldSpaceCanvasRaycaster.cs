using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;  // Include the TextMesh Pro namespace

public class WorldSpaceCanvasRaycaster : MonoBehaviour
{
    public Camera eventCamera; // the camera to cast the ray from

    void Update()
    {
        // Check if the left mouse button is clicked
        if (Input.GetMouseButtonDown(0))
        {
            // Create a ray from the camera to the mouse position
            Ray ray = eventCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            // Cast the ray into the scene
            if (Physics.Raycast(ray, out hit))
            {
                // Check if the object hit is part of the UI
                if (hit.collider != null)
                {
                    // Attempt to get the UI component (Button or TMP_Dropdown)
                    Button button = hit.collider.GetComponent<Button>();
                    // TMP_Dropdown tmpDropdown = hit.collider.GetComponent<TMP_Dropdown>();

                    // If it's a button, trigger the onClick event
                    if (button != null && button.interactable)
                    {
                        button.onClick.Invoke();
                    }
                    // If it's a TMP_Dropdown, toggle the dropdown list
                    // else if (tmpDropdown != null && tmpDropdown.interactable)
                    // {
                    //     tmpDropdown.onClick.Invoke();
                    // }
                    print(hit.collider.gameObject.name);
                }
            }
        }
    }
}