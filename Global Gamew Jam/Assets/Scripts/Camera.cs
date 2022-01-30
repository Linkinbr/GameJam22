using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera : MonoBehaviour
{
    public GameObject pivot;
    public Personagem personagem;
    public float velCamera = 5, offsetVert = 0.02f;
    private Vector3 posAtual, pivotOffset;

    private void Update()
    {
        pivotOffset.y = offsetVert * Input.GetAxis("Vertical");
        posAtual = Vector3.Lerp(transform.position, pivot.transform.position+pivotOffset, Time.deltaTime * velCamera);

        transform.position = posAtual;
    }
}
