using UnityEngine;

public class MoveCamera : MonoBehaviour
{
    public Transform cameraPosition;

    /*[Header("Pour phase dev")]
    public Transform initialTransform;

    void Awake()
    {
        transform.position = initialTransform.position;
    }*/

    private void Update()
    {
        transform.position = cameraPosition.position;
    }
}
