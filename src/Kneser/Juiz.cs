namespace Conde.Kneser;

/// <summary>
/// O juiz nao usa o <see cref="Grafo"/>, nao usa lista de vizinhos e nao usa a
/// <see cref="Coloracao"/>. Ele monta os conjuntos por conta propria, confere a
/// coloracao olhando PAR A PAR se os conjuntos sao disjuntos, e traz as duas
/// formulas fechadas: a de Lovasz para o numero cromatico e a de Erdos, Ko e
/// Rado para o maior independente.
/// </summary>
public static class Juiz
{
    /// <summary>O numero cromatico pelo teorema de Lovasz (1978): n - 2k + 2.</summary>
    public static int Lovasz(int n, int k) => n - 2 * k + 2;

    /// <summary>O maior independente pelo teorema de Erdos, Ko e Rado: C(n-1, k-1).</summary>
    public static int ErdosKoRado(int n, int k) => Subconjuntos.Binomial(n - 1, k - 1);

    /// <summary>
    /// A cota que sai sozinha de um conjunto independente maximo: o grafo tem
    /// C(n,k) vertices e nenhuma cor cobre mais que C(n-1,k-1) deles, entao
    /// sao precisas pelo menos C(n,k)/C(n-1,k-1) = n/k cores, arredondando para
    /// cima.
    /// </summary>
    public static int CotaPorIndependente(int n, int k) =>
        (int)Math.Ceiling((double)Subconjuntos.Binomial(n, k) / Subconjuntos.Binomial(n - 1, k - 1));

    /// <summary>
    /// A coloracao e propria, lida ao pe da letra: para cada par de conjuntos
    /// disjuntos, as cores tem que diferir. Monta a lista de conjuntos de novo,
    /// sem pedir nada ao grafo.
    /// </summary>
    public static bool EhPropriaPelaDefinicao(int n, int k, int[] cores)
    {
        var conjuntos = Subconjuntos.DeTamanho(n, k);
        if (cores.Length != conjuntos.Count) return false;

        for (var a = 0; a < conjuntos.Count; a++)
            for (var b = a + 1; b < conjuntos.Count; b++)
                if ((conjuntos[a] & conjuntos[b]) == 0 && cores[a] == cores[b]) return false;
        return true;
    }

    /// <summary>
    /// O conjunto de vertices e independente, lido ao pe da letra: nenhum par
    /// disjunto dentro dele.
    /// </summary>
    public static bool EhIndependentePelaDefinicao(int n, int k, IEnumerable<int> vertices)
    {
        var conjuntos = Subconjuntos.DeTamanho(n, k);
        var lista = vertices.ToList();
        for (var a = 0; a < lista.Count; a++)
            for (var b = a + 1; b < lista.Count; b++)
                if ((conjuntos[lista[a]] & conjuntos[lista[b]]) == 0) return false;
        return true;
    }

    /// <summary>
    /// O grau de todo vertice, pela contagem: um conjunto de tamanho k deixa
    /// n-k elementos de fora, e cabe C(n-k, k) conjuntos disjuntos dele.
    /// </summary>
    public static int GrauEsperado(int n, int k) => Subconjuntos.Binomial(n - k, k);

    /// <summary>As arestas pela contagem: C(n,k) * C(n-k,k) / 2.</summary>
    public static long ArestasEsperadas(int n, int k) =>
        (long)Subconjuntos.Binomial(n, k) * Subconjuntos.Binomial(n - k, k) / 2;
}
