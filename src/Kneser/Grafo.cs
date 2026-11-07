namespace Conde.Kneser;

/// <summary>
/// O grafo de Kneser K(n, k): um vertice para cada subconjunto de tamanho k de
/// {0, ..., n-1}, e uma aresta entre dois vertices quando os conjuntos sao
/// DISJUNTOS.
///
/// K(n, 1) e o grafo completo de n vertices. K(5, 2) e o Petersen.
/// </summary>
public sealed class Grafo
{
    private readonly List<int>[] vizinhos;

    public Grafo(int n, int k)
    {
        if (n < 2 * k) throw new ArgumentException("K(n,k) so interessa com n >= 2k");
        N = n;
        K = k;
        Conjuntos = Subconjuntos.DeTamanho(n, k);
        vizinhos = new List<int>[Conjuntos.Count];
        for (var v = 0; v < Conjuntos.Count; v++) vizinhos[v] = new List<int>();

        for (var a = 0; a < Conjuntos.Count; a++)
            for (var b = a + 1; b < Conjuntos.Count; b++)
                if (Subconjuntos.Disjuntos(Conjuntos[a], Conjuntos[b]))
                {
                    vizinhos[a].Add(b);
                    vizinhos[b].Add(a);
                }
    }

    public int N { get; }
    public int K { get; }
    public List<uint> Conjuntos { get; }
    public int Ordem => Conjuntos.Count;

    public IReadOnlyList<int> Vizinhos(int v) => vizinhos[v];
    public int Grau(int v) => vizinhos[v].Count;

    public int Arestas()
    {
        var soma = 0;
        for (var v = 0; v < Ordem; v++) soma += vizinhos[v].Count;
        return soma / 2;
    }

    /// <summary>
    /// Todo vertice tem o mesmo grau: C(n-k, k), o numero de conjuntos de
    /// tamanho k que cabem no complemento. O grafo e regular.
    /// </summary>
    public bool EhRegular()
    {
        for (var v = 1; v < Ordem; v++)
            if (vizinhos[v].Count != vizinhos[0].Count) return false;
        return true;
    }
}
