using TMPro;
using UnityEngine;

public abstract class InteractableUIBase : MonoBehaviour, IInteractable
{
   public GameObject boxUI;
    public TextMeshProUGUI textMeshPro;

    public virtual void OnHoverEnter()
    {
        boxUI.SetActive(true);
    }

    public virtual void OnHoverExit()
    {
        boxUI.SetActive(false);
    }

    public abstract void OnClick();
}
