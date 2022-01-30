using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DIFICULDADE
{
    FACIL,
    MEDIO,
    DIFICIL
}

public class Sanduiche : MonoBehaviour
{
    private List<GameObject> modulos;

    /// <summary>Distancia entre este objeto (pai e pão de cima) e a base do sanduiche (filho e pão de baixo) </summary>
    public float altura;
    public GameObject paoBase;

    public DIFICULDADE dificuldade;

    private void Awake()
    {
        altura = Mathf.Abs(paoBase.transform.localPosition.y);
        modulos = new List<GameObject>();
        for(int i=0; i<paoBase.transform.childCount; i++)
        {
            modulos.Add(paoBase.transform.GetChild(i).gameObject);
        }
    }

    public void ReativaModulos()
    {
        foreach(GameObject go in modulos)
        {
            go.SetActive(true);
        }   
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<Personagem>(out Personagem p))
        {
            //TODO: Marcar progresso, exigir novo sanduiche, etc
        }
        else if (other.tag == "ZonaDaMorte")
        {
            gameObject.SetActive(false);
        }
    }

    private void OnDisable()
    {
        ControladorFase.instance.reservaDeSanduiches.Add(this);
    }
}
