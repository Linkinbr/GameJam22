using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlataformaEspecial : MonoBehaviour
{
    public bool fogo;
    private Material matBase;
    private MeshRenderer mesh;
    private void Awake()
    {
        if(TryGetComponent<MeshRenderer>(out mesh))
        {
            matBase = mesh.material;
        }
    }

    private void Start()
    {
        ControladorGeloFogo.instance.plataformas.Add(this);
        Ativa(fogo == ControladorGeloFogo.instance.VersaoFogo);
    }

    public void Ativa(bool value)
    {
        if (mesh)
        {
            if (value)
            {
                print("Ativou " + name);
                mesh.material = matBase;
                //colisor.enabled = true;
            }
            else
            {
                print("Desativou" + name);
                //colisor.enabled = false;

                if (fogo)
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

    private void OnCollisionStay(Collision collision)
    {
        if (collision.collider.TryGetComponent<Personagem>(out Personagem p))
        {
            p.Temperatura += fogo ? p.modTempe * Time.deltaTime : -Time.deltaTime * p.modTempe;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.TryGetComponent<Personagem>(out Personagem p))
        {
            p.Temperatura += fogo ? 1.5f : -1.5f;
        }
    }

    private void OnDisable()
    {
        ControladorGeloFogo.instance.plataformas.Remove(this);
    }
}
