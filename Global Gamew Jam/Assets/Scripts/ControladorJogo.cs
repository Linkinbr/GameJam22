using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ControladorJogo : MonoBehaviour
{
    public static ControladorJogo instance;
    public float alturaMax, alturaAtual;

    private void Awake()
    {
        if(!instance)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {

    }

    public void recarregaCenas()
    {
        SceneManager.LoadSceneAsync("Procedural", LoadSceneMode.Single);
        SceneManager.LoadSceneAsync("Interface", LoadSceneMode.Additive);
    }
}
