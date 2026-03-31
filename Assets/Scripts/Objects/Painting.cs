using TMPro;
using UnityEngine;

public class Painting : InteractableUIBase
{
     public string paintingText;

    public override void OnHoverEnter()
    {
        base.OnHoverEnter();
        textMeshPro.text = paintingText;
        Debug.Log("Enter Painting");
    }

    public override void OnClick()
    {
        Debug.Log("Tableau : " + paintingText);
    }
}
