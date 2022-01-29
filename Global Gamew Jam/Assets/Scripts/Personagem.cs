using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Personagem : MonoBehaviour
{
    public float velAnda = 6.25f, velPulo =3.5f, velQueda = -2f, temperatura, duracaoMaxPulo = 0.6f;
    public Vector3 dirMovimento;
    public GameObject modelo;

    private CharacterController character;
    private bool pulou, pulouDuplo, olhandoDireita=true;
    private float timerPuloAtual, timerQuedaAtual, alturaOriginal;
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
        character.height = alturaOriginal;
        ControladorGeloFogo.instance.IntensificaPrevisao(false);
        pulouDuplo = false;
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
        else
        {
            //Usa a duracaoMaxPulo só para criar arco perfeito com as curvas de aceleração/desaceleração, calma
            dirMovimento.x = Input.GetAxis("Horizontal") * velAnda * Calc.RetaDecrescente(timerQuedaAtual/duracaoMaxPulo);
            dirMovimento.y = Calc.RetaCrescente(timerQuedaAtual / duracaoMaxPulo) * velQueda;

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

    // Update is called once per frame
    void Update()
    {
        Comportamento();
    }

}
