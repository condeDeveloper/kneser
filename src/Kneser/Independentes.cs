namespace Conde.Kneser;

/// <summary>
/// O maior conjunto independente do grafo de Kneser: um conjunto de vertices
/// dois a dois NAO disjuntos, ou seja, uma familia de conjuntos que se cruzam.
/// E exatamente o objeto do teorema de Erdos, Ko e Rado (1961), que diz que o
/// maior tem C(n-1, k-1) membros.
///
/// Aqui ele e achado por busca com retrocesso, sem usar o teorema.
/// </summary>
public static class Independentes
{
    public static (int Tamanho, long Nos) Maior(Grafo g)
    {
        var melhor = 0;
        long nos = 0;
        Descer(g, 0, 0, new List<int>(), ref melhor, ref nos);
        return (melhor, nos);
    }

    private static void Descer(Grafo g, int inicio, int tamanho, List<int> escolhidos,
                               ref int melhor, ref long nos)
    {
        if (tamanho > melhor) melhor = tamanho;

        // O que sobra nao chega no melhor: ramo morto.
        if (tamanho + (g.Ordem - inicio) <= melhor) return;

        for (var v = inicio; v < g.Ordem; v++)
        {
            nos++;
            if (tamanho + (g.Ordem - v) <= melhor) return;

            var cabe = true;
            foreach (var u in escolhidos)
                if (Subconjuntos.Disjuntos(g.Conjuntos[u], g.Conjuntos[v])) { cabe = false; break; }
            if (!cabe) continue;

            escolhidos.Add(v);
            Descer(g, v + 1, tamanho + 1, escolhidos, ref melhor, ref nos);
            escolhidos.RemoveAt(escolhidos.Count - 1);
        }
    }

    /// <summary>
    /// A familia estrela: todo conjunto que contem o elemento 0. Tem C(n-1, k-1)
    /// membros e e independente de graca, porque dois conjuntos que contem o
    /// mesmo elemento nunca sao disjuntos.
    /// </summary>
    public static List<int> Estrela(Grafo g)
    {
        var lista = new List<int>();
        for (var v = 0; v < g.Ordem; v++)
            if ((g.Conjuntos[v] & 1u) != 0) lista.Add(v);
        return lista;
    }
}
