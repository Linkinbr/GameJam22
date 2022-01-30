using System.Collections.Generic;
using UnityEngine;

public class ControladorFase : MonoBehaviour
{
    public static ControladorFase instance;
    public List<Sanduiche> reservaDeSanduiches;

    public List<Sanduiche> sanduichesFaceis, sanduichesMedios, sanduichesDificeis;

    private float topoDoUltimoSanduiche = 0;

    //A fase é dividida em cinco segmentos
    //0: 100% sanduiches/módulos de fase fáceis
    //1: 50/50 sanduiches fáceis e médios (alternando)
    //2: 100% sanduiches médios
    //3: 50/50 sanduiches médios e difíceis (alternando)
    //4: 100% sanduiches difíceis
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
        //Usando módulo, sabemos em qual segmento de fase o próximo sanduíche aparecerá e, consequentemente, sua dificuldade.
        switch(numSanduiches/sanduichesPorSegmento)
        {
            case 0:
                return DIFICULDADE.FACIL;
            case 1:
                print("C#, então: numSand = " + numSanduiches + " e sandPorSeg = " + sanduichesPorSegmento);
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

        //Pega um sanduíche, procurando na reserva caso não haja sanduiches disponíveis
        //Cuidado: Se você tentar montar uma torre de babel de sanduíches e não houver sanduíches em lugar nenhum, o mundo previsivelmente explode.
        if(deOnde.Count<1)
        {
            OrganizaReserva();
        } 
        int sanduicheSorteado = Random.Range(0, deOnde.Count);
        Sanduiche sanduiche = deOnde[sanduicheSorteado];
        deOnde.Remove(sanduiche);

        //Calcular o posicionamento do sanduíche usando a fatia de cima do último sanduíche como a fatia de baixo deste.
        float posY = topoDoUltimoSanduiche + sanduiche.altura;
        Vector3 posSanduiche = new Vector3(0, posY, 0);
        //print("Achou pos");

        //Se o sanduíche está ativo, quer dizer que é um preFab não instanciado. 
        if(sanduiche.gameObject.activeSelf)
        {
            Instantiate(sanduiche, posSanduiche, Quaternion.identity);
        }
        //Se estiver desativo, quer dizer que é uma instância prévia que está sendo reaproveitada.
        else
        {
            sanduiche.transform.position = posSanduiche;
            sanduiche.ReativaModulos();
        }
        //print("pôs sanduba");

        //A altura da pilha aumenta
        topoDoUltimoSanduiche += sanduiche.altura;
        numSanduiches++;
        //print("final do rolê");
    }

    private void AceleraZonaDeMorte()
    {
        //TODO: Teleportar Zona de Morte um sanduiche para cima e depois "avançar" as salas geradas
    }
}
