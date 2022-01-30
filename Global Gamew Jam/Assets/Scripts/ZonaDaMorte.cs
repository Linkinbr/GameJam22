using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZonaDaMorte : MonoBehaviour
{
    public static ZonaDaMorte instance;
    public float velocidade = 0.5f, velocidadeIncremento = 0.025f, velocidadeMax=3;
    public Material matGelo, matFogo;

    private MeshRenderer mesh;
    public float distanciaJogador;

    private void Awake()
    {
        if(!instance && TryGetComponent<MeshRenderer>(out mesh))
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        Physics.IgnoreLayerCollision(8, 9);
    }

    private void Start()
    {
        TrocaVersao(ControladorGeloFogo.instance.VersaoFogo);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.up * velocidade * Time.deltaTime);
        if(velocidade<velocidadeMax)
        {
            velocidade += velocidadeIncremento * Time.deltaTime;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.TryGetComponent<Personagem>(out Personagem f))
        {
            ControladorJogo.instance.CarregaCena("Procedural");
        }
        else
        {
            collision.collider.gameObject.SetActive(false);
        }
    }

    public void TrocaVersao(bool fogo)
    {
        mesh.material = fogo ? matFogo : matGelo;
    }
}
