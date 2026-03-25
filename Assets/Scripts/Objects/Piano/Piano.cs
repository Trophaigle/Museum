using System;
using TMPro;
using UnityEngine;

public class Piano : MonoBehaviour, IInteractable
{
    //public AudioSource audioSource;

    public string displayText;
    public GameObject boxUI;
    public TextMeshProUGUI textMeshPro;
    public string textPlaying;

     // ✅ événement pour notifier la fin
    public Action OnMusicFinished;

     public void OnHoverEnter()
    {
        if(GameManager.Instance.isPianoPlaying)
            textMeshPro.text = textPlaying;  
        else
            textMeshPro.text = displayText;

        boxUI.SetActive(true);
        Debug.Log("Enter Piano");
    }
    public void OnHoverExit()
    {
        boxUI.SetActive(false);
        Debug.Log("Leave Piano");
    }

    public void OnClick()
    {
        if (!GameManager.Instance.isPianoPlaying){
            Debug.Log("Play");
            GameManager.Instance.StartPlayPiano();
           // audioSource.Play();

            textMeshPro.text = textPlaying;
        }
        else {
            Debug.Log("Stop");
           // audioSource.Stop();
           GameManager.Instance.StopPlayPiano();
            textMeshPro.text = displayText;
        }
    }

    public void MarkMusicFinished() //appeler par Unity Event de MIDIPianoPlayer
    {
        GameManager.Instance.isPianoPlaying = false;
        textMeshPro.text = displayText; // Remet le texte normal
        Debug.Log("Music finished");
    } 
}
