using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Personagem : MonoBehaviour
{
    public float velAnda = 6.25f, velPulo =10f, velQueda = -8f, duracaoMaxPulo = 0.3625f, duracaoQuedaAcel=0.8f;
    public Vector3 dirMovimento;
    public GameObject modelo;
    public float modTempe = 3f;
    public MeshRenderer meshTermometro;

    private CharacterController character;
    private bool pulou, pulouDuplo, emParede, olhandoDireita=true;
    private float timerPuloAtual, timerQuedaAtual, alturaOriginal, temperatura;
    private int estado;

    private void Awake()
    {
        if(!TryGetComponent<CharacterController>(out character))
        {
            print("Erro! Sem CharacterController");
        }
        else
        {
            alturaOriginal = character.height;
        }
    }

    private void Vira(char dir)
    {
        if(dir=='d' && !olhandoDireita)
        {
            olhandoDireita = true;
            modelo.transform.Rotate(new Vector3(0, 180, 0));
        }

        if(dir=='e' && olhandoDireita)
        {
            olhandoDireita = false;
            modelo.transform.Rotate(new Vector3(0, -180, 0));
        }
    }

    public float Temperatura
    {
        get { return temperatura; }
        set
        {
            //Mudanças de interface nessas chaves (lugar específico dependende)

            temperatura = value;
            print("Value novo = " + temperatura);
            if(temperatura>1)
            {
                print("Quente");
                meshTermometro.material.color = new Color(0.9f, 0.9f-(temperatura/16), 0.9f-(temperatura/14));
                if (temperatura > 8)
                {
                    print("Quente demais");
                    StartCoroutine(Morte(true));
                }
            }
            else if(temperatura<-1)
            {
                print("Frio");
                meshTermometro.material.color = new Color(0.9f-(Mathf.Abs(temperatura)/14), 0.9f-(Mathf.Abs(temperatura)/16), 0.9f);
                if (temperatura<-8)
                {
                    print("Frio demais");
                    StartCoroutine(Morte(false));
                }
            }
            else
            {
                print("Morno");
                meshTermometro.material.color = new Color(0.9f, 0.9f, 0.9f);
            }
        }
    }

    private IEnumerator Morte(bool fogo)
    {
        yield return new WaitForEndOfFrame();
        //No futuro, o bool de "fogo" indicará se a anim/vfx de morte por calor ou por frio deverá tocar
        ControladorJogo.instance.CarregaCena("Implementacao");

    }

    public void EmParede(bool value)
    {
        emParede = value;
        if(emParede)
        {
            pulou = false;
            pulouDuplo = false;
        }
    }

    #region StateMachine
    public int Estado
    {
        get
        { return estado; }
        set
        {
            if(value!=estado)
            {
                int estadoVelho = estado;

                ChecaSaida();

                if (estado == estadoVelho)
                    estado = value;

                ChecaEntrada();
            }
        }
    }

    private void ChecaEntrada()
    {
        switch (estado)
        {
            case 0:
                print("Entrou Idle");
                EntradaIdle();
                break;
            case 1:
                print("Entrou Anda");
                EntradaAnda();
                break;
            case 2:
                print("Entrou Pula");
                EntradaPula();
                break;
            case 3:
                print("Entrou Queda");
                EntradaQueda();
                break;
            case 4:
                print("Entrou PuloDuplo");
                EntradaPuloDuplo();
                break;
            case 5:
                print("Entrou PresoEmParede");
                EntradaEmParede();
                break;
        }
    }

    private void Comportamento()
    {
        switch (estado)
        {
            case 0:
                ComportamentoIdle();
                break;
            case 1:
                ComportamentoAnda();
                break;
            case 2:
                ComportamentoPula();
                break;
            case 3:
                ComportamentoQueda();
                break;
            case 4:
                ComportamentoPuloDuplo();
                break;
            case 5:
                ComportamentoEmParede();
                break;
        }
    }

    private void ChecaSaida()
    {
        switch (estado)
        {
            case 0:
                SaidaIdle();
                break;
            case 1:
                SaidaAnda();
                break;
            case 2:
                SaidaPula();
                break;
            case 3:
                SaidaQueda();
                break;
            case 4:
                SaidaPuloDuplo();
                break;
            case 5:
                SaidaEmParede();
                break;
        }
    }
    #endregion

    #region StateIdle 0
    private void ComportamentoIdle()
    {
        if(Input.GetButtonDown("Pulo"))
        {
            Estado = 2; //Pulo
        }
        else if (Input.GetButton("Horizontal"))
        {
            Estado = 1; //Anda
        }
        else
        {
            character.Move(Vector3.down * 0.001f);
            if(!character.isGrounded)
            {
                Estado = 3; //Queda
            }
        }
    }

    private void EntradaIdle()
    {
        pulou = false;
        pulouDuplo = false;
        character.height = alturaOriginal;
        ControladorGeloFogo.instance.IntensificaPrevisao(false);
        dirMovimento.y = 0;
        
    }

    private void SaidaIdle()
    {

    }
    #endregion

    #region StateAnda 1
    private void ComportamentoAnda()
    {
        if(Input.GetButtonDown("Pulo"))
        {
            Estado = 2; //Pulo
        }
        else
        {
            character.Move(Vector3.down * 0.001f);
            if (!character.isGrounded)
            {
                Estado = 3; //Queda
            }
        }

        Movimenta();
    }
    private void Movimenta()
    {
        dirMovimento.x = Input.GetAxis("Horizontal") * velAnda;
        if (Input.GetButton("Horizontal"))
        {
            if (Input.GetButton("Direita"))
            {
                Vira('d');
            }
            else
            {
                Vira('e');
            }
        }
        else
        {
            //Desacelera rápido se nenhuma tecla estiver pressionado, eventualmente parando
            dirMovimento.x /= 2.5f;
            if (dirMovimento.magnitude <= 0.225)
            {
                Estado = 0; //Idle
                return;
            }
        }
        character.Move(dirMovimento * Time.deltaTime);
    }

    private void EntradaAnda()
    {
        pulou = false;
        pulouDuplo = false;
        dirMovimento.y = 0;
        ControladorGeloFogo.instance.IntensificaPrevisao(false);
        character.height = alturaOriginal;
    }

    private void SaidaAnda()
    { 

    }
    #endregion

    #region StatePula 2
    private void ComportamentoPula()
    {

        if(Input.GetButtonDown("Pulo"))
        {
            print("Pulou duplo de pulo");
            Estado = 4; //PuloDuplo
        }
        else if (timerPuloAtual < duracaoMaxPulo && Input.GetButton("Pulo"))
        {
            dirMovimento.x = Input.GetAxis("Horizontal") * velAnda * Calc.RetaCrescente(timerPuloAtual/duracaoMaxPulo);
            dirMovimento.y = Calc.RetaDecrescente(timerPuloAtual / duracaoMaxPulo) * velPulo;

            character.Move(dirMovimento * Time.deltaTime);
            timerPuloAtual += Time.deltaTime;
        }
        else
        {
            Estado = 3; //Queda
        }

    }

    private void EntradaPula()
    {
        timerPuloAtual = 0;
        //character.height = alturaOriginal * 0.5f;
        ControladorGeloFogo.instance.IntensificaPrevisao(true);
        pulou = true;
    }

    private void SaidaPula()
    {

    }
    #endregion

    #region StateQueda 3
    private void ComportamentoQueda()
    {
        if(timerQuedaAtual<=0.15f && Input.GetButtonDown("Pulo") && !pulou)
        {
            Estado = 2; //Pulo
        }
        else if(Input.GetButtonDown("Pulo") && !pulouDuplo)
        {
            Estado = 4; //PuloDuplo
        }
        else if(emParede)
        {
            Estado = 5; //EmParede
        }
        else
        {
            dirMovimento.x = Input.GetAxis("Horizontal") * velAnda * Calc.RetaDecrescente(timerQuedaAtual/duracaoQuedaAcel);
            dirMovimento.y = (0.05f + Calc.CurvaAcelSqrt(timerQuedaAtual / duracaoQuedaAcel)) * velQueda;

            if (Input.GetButton("Baixo"))
            {
                dirMovimento.y *= 1.5f;
            }
            if (timerQuedaAtual > duracaoQuedaAcel)
                dirMovimento.y *= 1.5f;

            character.Move(dirMovimento * Time.deltaTime);
            timerQuedaAtual += Time.deltaTime;
            if(character.isGrounded)
            {
                Estado = 0; //Idle
            }
        }
    }

    private void EntradaQueda()
    {
        timerQuedaAtual = 0f;
        ControladorGeloFogo.instance.IntensificaPrevisao(true);
        //character.height = alturaOriginal * 0.5f;
    }

    private void SaidaQueda()
    {

    }
    #endregion

    #region StatePuloDuplo 4
    private void ComportamentoPuloDuplo()
    {
        //OBS: Metade da duração do pulo normal.
        if (timerPuloAtual < duracaoMaxPulo*0.5f && Input.GetButton("Pulo"))
        {
            dirMovimento.x = Input.GetAxis("Horizontal") * velAnda * 1.2f;
            dirMovimento.y = Calc.RetaDecrescente(timerPuloAtual / duracaoMaxPulo*0.5f) * velPulo;

            character.Move(dirMovimento * Time.deltaTime);
            timerPuloAtual += Time.deltaTime;
        }
        else
        {
            Estado = 3; //Queda
        }
    }

    private void EntradaPuloDuplo()
    {
        pulouDuplo = true;
        //character.height = alturaOriginal * 0.5f;
        timerPuloAtual = 0;
        ControladorGeloFogo.instance.TrocaVersao();
    }

    private void SaidaPuloDuplo()
    {

    }
    #endregion

    #region StateEmParede 5
    private void ComportamentoEmParede()
    {
        if(Input.GetButtonDown("Pulo"))
        {
            if(!pulou)
            {
                Estado = 2; //Pulo
                return;
            }
            else if(!pulouDuplo)
            {
                Estado = 4; //PuloDuplo
                return;
            }
        }

        dirMovimento.x = Input.GetAxis("Horizontal") * velAnda;
        dirMovimento.y = Calc.CurvaAcelSqrt(timerQuedaAtual / duracaoQuedaAcel*2f) * velQueda* 0.5f;

        if (Input.GetButton("Baixo"))
        {
            dirMovimento.y *= 1.5f;
        }
        if (timerQuedaAtual > duracaoQuedaAcel*2f)
            dirMovimento.y *= 1.25f;

        character.Move(dirMovimento * Time.deltaTime);
        timerQuedaAtual += Time.deltaTime;

        if (character.isGrounded)
        {
            Estado = 0; //Idle
        }
        else if(!emParede)
        {
            Estado=3; //Queda
        }
    }

    private void EntradaEmParede()
    {
        timerQuedaAtual = 0;
        ControladorGeloFogo.instance.IntensificaPrevisao(true);
    }

    private void SaidaEmParede()
    {
        
    }
    #endregion

    // Update is called once per frame
    void Update()
    {
        Comportamento();
        if(temperatura>0.3f)
        {
            Temperatura -= modTempe * 0.2f * Time.deltaTime;
        }
        else if(temperatura<-0.3f)
        {
            Temperatura += modTempe * 0.2f * Time.deltaTime;
        }
    }

}
