using System.Collections.Generic;
using UnityEngine;

public class ControladorFase : MonoBehaviour
{
    public static ControladorFase instance;
    public List<Sanduiche> reservaDeSanduiches;

    public List<Sanduiche> sanduichesFaceis, sanduichesMedios, sanduichesDificeis;

    private float topoDoUltimoSanduiche = 0;

    //A fase � dividida em cinco segmentos
    //0: 100% sanduiches/m�dulos de fase f�ceis
    //1: 50/50 sanduiches f�ceis e m�dios (alternando)
    //2: 100% sanduiches m�dios
    //3: 50/50 sanduiches m�dios e dif�ceis (alternando)
    //4: 100% sanduiches dif�ceis
    private int numSanduiches=0, sanduichesPorSegmento=8, sanduichesSimultaneos = 5;
    private Sanduiche[] sanduichesAtivos;


    private void Awake()
    {
        if (!instance)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        //OrganizaReserva();
    }

    private void Start()
    {
        //sanduichesAtivos = new Sanduiche[sanduichesSimultaneos];
        for (int i = 0; i < sanduichesSimultaneos; i++)
        {
            PosicionaSanduiche();
        }
    }

    private void OrganizaReserva()
    {
        foreach(Sanduiche mdf in reservaDeSanduiches)
        {
            switch (mdf.dificuldade)
            {
                case DIFICULDADE.FACIL:
                    sanduichesFaceis.Add(mdf);
                    break;

                case DIFICULDADE.MEDIO:
                    sanduichesMedios.Add(mdf);
                    break;

                case DIFICULDADE.DIFICIL:
                    sanduichesDificeis.Add(mdf);
                    break;

                default:
                    Destroy(mdf.gameObject);
                    break;
            }
        }

        reservaDeSanduiches.Clear();
    }

    private DIFICULDADE DificuldadeProxSanduiche()
    {
        //Usando m�dulo, sabemos em qual segmento de fase o pr�ximo sandu�che aparecer� e, consequentemente, sua dificuldade.
        switch(numSanduiches/sanduichesPorSegmento)
        {
            case 0:
                return DIFICULDADE.FACIL;
            case 1:
                print("C#, ent�o: numSand = " + numSanduiches + " e sandPorSeg = " + sanduichesPorSegmento);
                return numSanduiches % 2 == 0 ? DIFICULDADE.MEDIO : DIFICULDADE.FACIL;
            case 2:
                return DIFICULDADE.MEDIO;
            case 3:
                return numSanduiches % 2 == 0 ? DIFICULDADE.DIFICIL : DIFICULDADE.MEDIO;
            case 4:
                return DIFICULDADE.DIFICIL;
            default:
                return DIFICULDADE.MEDIO;
        }
    }

    /// <summary>Cria um sanduiche no topo da torre baseado na dificuldade atual</summary>
    private void PosicionaSanduiche()
    {
        List<Sanduiche> deOnde;

        if (DificuldadeProxSanduiche() == DIFICULDADE.FACIL)
        {
            deOnde = sanduichesFaceis;
        }   
        else if (DificuldadeProxSanduiche() == DIFICULDADE.MEDIO)
        {
            deOnde = sanduichesMedios;
        }
        else
        {
            deOnde = sanduichesDificeis;
        }

        //print("deOnde: " + deOnde);

        //Pega um sandu�che, procurando na reserva caso n�o haja sanduiches dispon�veis
        //Cuidado: Se voc� tentar montar uma torre de babel de sandu�ches e n�o houver sandu�ches em lugar nenhum, o mundo previsivelmente explode.
        if(deOnde.Count<1)
        {
            OrganizaReserva();
        } 
        int sanduicheSorteado = Random.Range(0, deOnde.Count);
        Sanduiche sanduiche = deOnde[sanduicheSorteado];
        deOnde.Remove(sanduiche);

        //Calcular o posicionamento do sandu�che usando a fatia de cima do �ltimo sandu�che como a fatia de baixo deste.
        float posY = topoDoUltimoSanduiche + sanduiche.altura;
        Vector3 posSanduiche = new Vector3(0, posY, 0);
        //print("Achou pos");

        //Se o sandu�che est� ativo, quer dizer que � um preFab n�o instanciado. 
        if(sanduiche.gameObject.activeSelf)
        {
            Instantiate(sanduiche, posSanduiche, Quaternion.identity);
        }
        //Se estiver desativo, quer dizer que � uma inst�ncia pr�via que est� sendo reaproveitada.
        else
        {
            sanduiche.transform.position = posSanduiche;
            sanduiche.ReativaModulos();
        }
        //print("p�s sanduba");

        //A altura da pilha aumenta
        topoDoUltimoSanduiche += sanduiche.altura;
        numSanduiches++;
        //print("final do rol�");
    }

    private void AceleraZonaDeMorte()
    {
        //TODO: Teleportar Zona de Morte um sanduiche para cima e depois "avan�ar" as salas geradas
    }
}
