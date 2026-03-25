using System.Collections;
using UnityEngine;

public class PianoKey : MonoBehaviour
{
    private Animator animator;

    void Awake()
    {
        animator = GetComponent<Animator>();

        if (animator == null)
        {
            Debug.LogError($"❌ Pas d'Animator sur {gameObject.name}");
        }

        animator.Play("Idle", 0, 0f);
        animator.Update(0f);

        animator.SetBool("isPressed", false);
    }

    public void Press(double dspNoteTime, float duration)
    {
        animator.SetBool("isPressed", true);
        StartCoroutine(ReleaseAfterDSP(dspNoteTime, duration));
    }

    IEnumerator ReleaseAfterDSP(double noteTime, float duration)
    {
        //double visualOffset = 0.02;

        double endTime = noteTime + duration /*- visualOffset*/;

        double delay = endTime - AudioSettings.dspTime;
        //while (AudioSettings.dspTime < endTime)
         //   yield return null;
        if(delay > 0)
        {
            yield return new WaitForSeconds((float) delay);
        }

        animator.SetBool("isPressed", false);
    }
}
