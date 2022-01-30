using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Calc
{
    // ____________________________
    // |    CÁLCULOS DE CURVAS    |
    // ‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾

    /// <summary>Cria uma curva de desaceleração, usando de uma função exponencial simples</summary>
    /// <param name="X">Valor de 0 à 1, com 0 sendo o começo da curva (p ou t=0) e 1 sendo o final (p ou t=Destino ou TempoFinal)</param>
    /// <returns>Valor que deve ser multiplicado à velocidade</returns>
    public static float CurvaDesacelPow(float X)
    {
        if (X > 0.99f)
            return 1 - Mathf.Pow(0.99f, 2);
        else
            return 1 - Mathf.Pow(X, 2);
    }

    /// <summary>Cria uma curva de desaceleração, usando de uma divisão simples</summary>
    /// <param name="X">Valor de 0 à 1, com 0 sendo o começo da curva (p ou t=0) e 1 sendo o final (p ou t=Destino ou TempoFinal)</param>
    /// <returns>Valor que deve ser multiplicado à velocidade</returns>
    public static float CurvaDesacelDiv(float X)
    {
        return (1 - X) / (1 + X);
    }

    /// <summary>Cria uma curva de aceleração, usando de uma função de raiz simples</summary>
    /// <param name="X">Valor de 0 à 1, com 0 sendo o começo da curva (p ou t=0) e 1 sendo o final (p ou t=Destino ou TempoFinal)</param>
    /// <returns>Valor que deve ser multiplicado à velocidade</returns>
    public static float CurvaAcelSqrt(float X)
    {
        if (X > 0.99f)
            return Mathf.Sqrt(0.99f);
        else
            return Mathf.Sqrt(X);
    }

    /// <summary>Cria uma parábola, usando de uma função de 2º grau simples</summary>
    /// <param name="X">Valor de 0 à 1, com 0 sendo o começo da curva (p ou t=0) e 1 sendo o final (p ou t=Destino ou TempoFinal)</param>
    /// <param name="offSet">Aumenta a velocidade inicial e final do movimento.</param>
    /// <returns>Valor que deve ser multiplicado à velocidade</returns>
    public static float CurvaParabola(float X, float offSet)
    {
        //if (offSet + X >= 1)
        //  X = 1 - offSet;
        return offSet + (4 * X - 4 * Mathf.Pow(X, 2));
    }

    public static float CurvaParabolaAcel(float X)
    {
        return X > 1 ? 1 : X*X;
    }

    public static float RetaDecrescente(float X)
    {
        return X>1? 0.5f : -X + 1.5f;
    }
    public static float RetaCrescente(float X)
    {
        return X>1? 1.5f : X + 0.5f;
    }
}
