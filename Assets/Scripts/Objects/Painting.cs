using TMPro;
using UnityEngine;

public class Painting : MonoBehaviour, IInteractable
{
    public string paintingText;
    public GameObject boxUI;
    public TextMeshProUGUI textMeshPro;
    private bool canPlay = true;

    public void OnHoverEnter()
    {
        textMeshPro.text = paintingText; 
        Debug.Log("Enter Painting");
        boxUI.SetActive(true);
    }

    public void OnHoverExit()
    {
        boxUI.SetActive(false);
        Debug.Log("Leave painting");
    }

    public void OnClick()
    {
        Debug.Log("Tableau : " + paintingText);
    }
}
