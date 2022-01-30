using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerMuroPulavel : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<Personagem>(out Personagem p))
        {
            p.EmParede(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent<Personagem>(out Personagem p))
        {
            p.EmParede(false);
        }
    }
}
