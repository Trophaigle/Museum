using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    public float distance = 2f;
    private IInteractable currentHover;

    public LayerMask interactLayer;

    public Camera cam;

    void Update()
    {
        if(GameManager.Instance.IsState(GameManager.GameState.Intro))
            return;

        Vector3 origin = cam.transform.position + cam.transform.forward * 0.5f;
        Vector3 direction = cam.transform.forward;  // Axé sur la caméra, tout droit
        Ray ray = new Ray(origin, direction);
        RaycastHit hit;
         Debug.DrawRay(origin, direction * distance, Color.red);

        if (Physics.Raycast(ray, out hit, distance, interactLayer))
        {
            if (hit.collider.TryGetComponent<IInteractable>(out var interactable))
            {
                if (currentHover != interactable)
                {
                    currentHover?.OnHoverExit();
                    currentHover = interactable;
                    currentHover.OnHoverEnter();
                }

                if (Input.GetMouseButtonDown(0))
                {
                    interactable.OnClick();
                }

                return;
            }
        }

        ClearHover();
    }

    void ClearHover()
    {
        if (currentHover != null)
        {
            currentHover.OnHoverExit();
            currentHover = null;
        }
    }
}
