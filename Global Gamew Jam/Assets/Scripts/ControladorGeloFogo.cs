using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControladorGeloFogo : MonoBehaviour
{
    public static ControladorGeloFogo instance;
    public List<PlataformaEspecial> plataformas;
    public Material matFogo, matGelo;
    public float modIntensidade=2f;

    private float opacidadeBaseFogo, opacidadeBaseGelo;
    private bool versaoFogo, previsaoIntensificada;
    private void Awake()
    {
        if (!instance)
            instance = this;
        else
            Destroy(gameObject);

        opacidadeBaseFogo = 0.3f;
        opacidadeBaseGelo = 0.3f;
    }

    private void Start()
    {
        foreach (PlataformaEspecial plat in plataformas)
        {
            print("Plat " + name + "é fogo?" + plat.fogo + ". Versão fogo atual é " + versaoFogo);
            plat.Ativa(plat.fogo == versaoFogo);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public bool VersaoFogo
    {
        get { return versaoFogo; }
    }

    public void TrocaVersao()
    {
        versaoFogo = !versaoFogo;
        foreach (PlataformaEspecial plat in plataformas)
        {
            plat.Ativa(plat.fogo == versaoFogo);
        }
    }

    /// <summary>
    /// Deixa o shader das plataformas especiais desativadas um pouco mais opaco durante o pulo e queda do jogador
    /// </summary>
    public void IntensificaPrevisao(bool intensificado)
    {
        if (previsaoIntensificada != intensificado)
        {
            previsaoIntensificada = intensificado;
            if (intensificado)
            {
                matFogo.color = new Vector4(matFogo.color.r, matFogo.color.g, matFogo.color.b, opacidadeBaseFogo * modIntensidade);
                matGelo.color = new Vector4(matGelo.color.r, matGelo.color.g, matGelo.color.b, opacidadeBaseGelo * modIntensidade);
            }
            else
            {
                matFogo.color = new Vector4(matFogo.color.r, matFogo.color.g, matFogo.color.b, opacidadeBaseFogo);
                matGelo.color = new Vector4(matGelo.color.r, matGelo.color.g, matGelo.color.b, opacidadeBaseGelo);
            }
        }
    }
}
