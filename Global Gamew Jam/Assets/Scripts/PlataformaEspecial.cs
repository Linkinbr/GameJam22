using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlataformaEspecial : MonoBehaviour
{
    public bool fogo;
    private Material matBase;
    private MeshRenderer mesh;
    private Collider colisor;
    private void Awake()
    {
        if(TryGetComponent<MeshRenderer>(out mesh))
        {
            matBase = mesh.material;
        }
        else
        {
            print("Plataforma " + name + " não encontrou mesh");
        }

        if(!TryGetComponent<Collider>(out colisor))
        {
            print("Plataforma "+name+" não encontrou colisor");
        }
    }

    public void Intensifica(bool value)
    {

    }

    public void Ativa(bool value)
    {
        if(value)
        {
            print("Ativou " + name);
            mesh.material = matBase;
            colisor.enabled = true;
        }
        else
        {
            print("Desativou" + name);
            colisor.enabled = false;

            if(fogo)
            {
                mesh.material = ControladorGeloFogo.instance.matFogo;
            }
            else
            {
                mesh.material = ControladorGeloFogo.instance.matGelo;
            }
        }
    }
}
