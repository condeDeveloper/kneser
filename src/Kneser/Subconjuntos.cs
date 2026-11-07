namespace Conde.Kneser;

/// <summary>
/// Os subconjuntos de tamanho k de {0, ..., n-1}, guardados como mascara de
/// bits e listados sempre na mesma ordem: a ordem crescente da mascara.
/// </summary>
public static class Subconjuntos
{
    /// <summary>Todos os subconjuntos de tamanho <paramref name="k"/>, em ordem de mascara.</summary>
    public static List<uint> DeTamanho(int n, int k)
    {
        if (n < 0 || n > 31) throw new ArgumentOutOfRangeException(nameof(n));
        if (k < 0 || k > n) throw new ArgumentOutOfRangeException(nameof(k));

        var lista = new List<uint>(Binomial(n, k));
        for (uint m = 0; m < (1u << n); m++)
            if (System.Numerics.BitOperations.PopCount(m) == k) lista.Add(m);
        return lista;
    }

    /// <summary>C(n, k), montado pelo triangulo de Pascal para nao estourar.</summary>
    public static int Binomial(int n, int k)
    {
        if (k < 0 || k > n) return 0;
        var linha = new long[n + 1];
        linha[0] = 1;
        for (var i = 1; i <= n; i++)
            for (var j = Math.Min(i, k); j >= 1; j--)
                linha[j] += linha[j - 1];
        return checked((int)linha[k]);
    }

    /// <summary>O menor elemento do conjunto. Base da coloracao natural.</summary>
    public static int Menor(uint conjunto) => System.Numerics.BitOperations.TrailingZeroCount(conjunto);

    /// <summary>Os conjuntos sao disjuntos? E a aresta do grafo de Kneser.</summary>
    public static bool Disjuntos(uint a, uint b) => (a & b) == 0;

    /// <summary>A forma legivel, para mensagem de teste: {0,2,4}.</summary>
    public static string Escrever(uint conjunto)
    {
        var elementos = new List<int>();
        for (var i = 0; i < 32; i++)
            if ((conjunto & (1u << i)) != 0) elementos.Add(i);
        return "{" + string.Join(",", elementos) + "}";
    }
}
