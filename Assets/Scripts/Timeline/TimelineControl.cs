using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Playables;

public class TimelineControl : MonoBehaviour
{
    public GameObject mainCamera;
    public GameObject cross;

    public void disableCinemachine()
    {
        mainCamera.GetComponent<CinemachineBrain>().enabled = false;
        Debug.Log("Cinemachine disabled on Main Camera");
        cross.SetActive(true);
        GameManager.Instance.SetState(GameManager.GameState.Game);
    }

}
