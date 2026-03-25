using TMPro;
using UnityEngine;

public class Gramophone : MonoBehaviour, IInteractable
{
    public AudioSource audioSource;

    public string displayText;
    public GameObject boxUI;
    public TextMeshProUGUI textMeshPro;
    public string textPlaying;
    private bool isPlaying = false;

    public void OnHoverEnter()
    {
        if(isPlaying)
            textMeshPro.text = textPlaying;  
        else
            textMeshPro.text = displayText;

        boxUI.SetActive(true);
        Debug.Log("Enter Gramophone");
    }
    public void OnHoverExit()
    {
        boxUI.SetActive(false);
        Debug.Log("Leave Gramophone");
    }

    public void OnClick()
    {
        if (!audioSource.isPlaying){
            Debug.Log("Play");
            audioSource.Play();
            isPlaying = true;
            textMeshPro.text = textPlaying;
        }
        else {
            Debug.Log("Stop");
            audioSource.Stop();
            isPlaying = false;
            textMeshPro.text = displayText;
        }
    }

    void Update()
    {
        if (isPlaying && !audioSource.isPlaying)
        {
            // La musique a fini
            isPlaying = false;
            textMeshPro.text = displayText; // Remet le texte normal
            Debug.Log("Music finished");
        }
    }
}
