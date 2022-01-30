using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ControladorJogo : MonoBehaviour
{
    public static ControladorJogo instance;

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

    public void CarregaCena(string nome)
    {
        SceneManager.LoadScene(nome);
    }
}
