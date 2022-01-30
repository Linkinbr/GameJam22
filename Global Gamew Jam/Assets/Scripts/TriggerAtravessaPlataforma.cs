using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerAtravessaPlataforma : TriggerDePlataforma
{
    protected Collider colisorPlataforma;
    protected void Start()
    {
        colisorPlataforma = plataforma.GetComponent<Collider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent<CharacterController>(out CharacterController character))
            Physics.IgnoreCollision(character, colisorPlataforma);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent<CharacterController>(out CharacterController character))
            Physics.IgnoreCollision(character, colisorPlataforma, false);
    }
}
