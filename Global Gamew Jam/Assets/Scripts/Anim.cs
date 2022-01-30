using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Anim : MonoBehaviour
{
    public Animator animator;
    protected string ultimaAnim = "Idle";

    private void Awake()
    {
        TryGetComponent<Animator>(out animator);
    }

    public void ChamaAnim(string nome)
    {
        if (nome != ultimaAnim)
        {
            print("Ativou anim: " + nome);
            animator.Play(nome);
            ultimaAnim = nome;
        }
        else print("Ultima anim já era " + ultimaAnim + "!");
    }
}
