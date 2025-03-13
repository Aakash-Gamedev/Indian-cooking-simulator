using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerInteractionHandler : MonoBehaviour
{
    [Header("Settings")]
    public float raycastDistance = 100.0f;
    public LayerMask raycastLayerMask;

    [Header("References")]
    public Camera playerCamera;

    private void Update()
    {

        // Cast a ray from the center of the screen
        Ray ray = playerCamera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, raycastDistance, raycastLayerMask))
        {
            // Check if the hit object implements IInteractable
            if (Input.GetMouseButtonDown(0) && hit.collider.GetComponent<IInteractable>() != null)
            {
                var interactable = hit.collider.GetComponent<IInteractable>();
                interactable.Interact();
            }
        }
    }
}
