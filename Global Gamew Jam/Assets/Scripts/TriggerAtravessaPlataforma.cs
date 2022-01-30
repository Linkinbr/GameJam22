using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerAtravessaPlataforma : TriggerDePlataforma
{
    protected Collider[] colisoresPlataforma;
    protected void Start()
    {
        colisoresPlataforma = plataforma.GetComponents<Collider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent<CharacterController>(out CharacterController character))
        {
            foreach(Collider c in colisoresPlataforma)
            {
                Physics.IgnoreCollision(character, c);
            }
        }
            
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent<CharacterController>(out CharacterController character))
        {
            foreach (Collider c in colisoresPlataforma)
            {
                Physics.IgnoreCollision(character, c, false);
            }
        }
    }
}
