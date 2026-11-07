namespace Conde.Kneser;

/// <summary>O resultado de uma busca por coloracao: achou, com quantas, a que custo.</summary>
public readonly record struct Tentativa(int Cores, bool Coube, long Nos);

/// <summary>
/// Coloracao propria: vertices vizinhos nunca com a mesma cor. O numero
/// cromatico sai por busca exaustiva — primeiro tenta uma cor, depois duas, e
/// assim por diante. A primeira que couber e o numero cromatico, e a ultima que
/// NAO coube e a parte cara: para dizer que c-1 cores nao bastam e preciso
/// esgotar todas.
/// </summary>
public static class Coloracao
{
    /// <summary>
    /// A coloracao natural: cada conjunto recebe o seu MENOR elemento como cor,
    /// e todo conjunto cujo menor passa de n-2k+1 cai na ultima cor. Gasta
    /// n-2k+2 cores. E a construcao que da a cota de cima do teorema.
    /// </summary>
    public static int[] Natural(Grafo g)
    {
        var teto = g.N - 2 * g.K + 1;
        var cores = new int[g.Ordem];
        for (var v = 0; v < g.Ordem; v++)
            cores[v] = Math.Min(Subconjuntos.Menor(g.Conjuntos[v]), teto);
        return cores;
    }

    /// <summary>A coloracao e propria? Confere TODA aresta.</summary>
    public static bool EhPropria(Grafo g, int[] cores)
    {
        for (var v = 0; v < g.Ordem; v++)
            foreach (var w in g.Vizinhos(v))
                if (cores[v] == cores[w]) return false;
        return true;
    }

    public static int QuantasCores(int[] cores) => cores.Distinct().Count();

    /// <summary>
    /// Cabe em <paramref name="cores"/> cores? Busca com retrocesso.
    ///
    /// Duas coisas seguram o custo. A primeira e a ordem: a cada passo entra o
    /// vertice mais SATURADO, o que ja tem mais cores diferentes em volta — e o
    /// que tem menos saida, e e onde a contradicao aparece mais cedo. A segunda
    /// e a quebra de simetria: um vertice so estreia uma cor nova se todas as
    /// anteriores ja entraram, senao a busca refaz a mesma coloracao com os
    /// nomes das cores trocados.
    /// </summary>
    public static Tentativa Cabe(Grafo g, int cores)
    {
        var cor = new int[g.Ordem];
        Array.Fill(cor, -1);
        var quantos = new int[g.Ordem, cores];   // quantos vizinhos de v ja usam a cor c
        var proibidas = new int[g.Ordem];        // mascara das cores proibidas em v
        long nos = 0;
        var coube = Pintar(g, cor, quantos, proibidas, 0, cores, 0, ref nos);
        return new Tentativa(cores, coube, nos);
    }

    private static bool Pintar(Grafo g, int[] cor, int[,] quantos, int[] proibidas,
                               int pintados, int cores, int usadas, ref long nos)
    {
        if (pintados == g.Ordem) return true;

        // O mais saturado primeiro, desempatando pelo grau e depois pelo indice,
        // para a busca ser sempre a mesma em qualquer maquina.
        var v = -1;
        var melhorSat = -1;
        var melhorGrau = -1;
        for (var u = 0; u < g.Ordem; u++)
        {
            if (cor[u] >= 0) continue;
            var sat = System.Numerics.BitOperations.PopCount((uint)proibidas[u]);
            if (sat > melhorSat || (sat == melhorSat && g.Grau(u) > melhorGrau))
            {
                v = u;
                melhorSat = sat;
                melhorGrau = g.Grau(u);
            }
        }

        var teto = Math.Min(cores, usadas + 1);
        for (var c = 0; c < teto; c++)
        {
            if ((proibidas[v] & (1 << c)) != 0) continue;
            nos++;

            cor[v] = c;
            foreach (var w in g.Vizinhos(v))
            {
                if (quantos[w, c]++ == 0) proibidas[w] |= 1 << c;
            }

            if (Pintar(g, cor, quantos, proibidas, pintados + 1, cores,
                       Math.Max(usadas, c + 1), ref nos)) return true;

            foreach (var w in g.Vizinhos(v))
            {
                if (--quantos[w, c] == 0) proibidas[w] &= ~(1 << c);
            }
            cor[v] = -1;
        }
        return false;
    }

    /// <summary>
    /// A MESMA busca com a ordem fixa: os vertices entram do mais ligado para o
    /// menos ligado, decidido uma vez antes de comecar. Era a primeira versao.
    /// Ficou no repositorio porque e ela que mede o que a saturacao poupa.
    /// </summary>
    public static Tentativa CabeOrdemFixa(Grafo g, int cores)
    {
        var ordem = Enumerable.Range(0, g.Ordem).OrderByDescending(g.Grau).ThenBy(v => v).ToArray();
        var cor = new int[g.Ordem];
        Array.Fill(cor, -1);
        long nos = 0;
        var coube = PintarFixo(g, ordem, cor, 0, cores, 0, ref nos);
        return new Tentativa(cores, coube, nos);
    }

    private static bool PintarFixo(Grafo g, int[] ordem, int[] cor, int passo,
                                   int cores, int usadas, ref long nos)
    {
        if (passo == ordem.Length) return true;
        var v = ordem[passo];
        var teto = Math.Min(cores, usadas + 1);

        for (var c = 0; c < teto; c++)
        {
            nos++;
            var livre = true;
            foreach (var w in g.Vizinhos(v))
                if (cor[w] == c) { livre = false; break; }
            if (!livre) continue;

            cor[v] = c;
            if (PintarFixo(g, ordem, cor, passo + 1, cores, Math.Max(usadas, c + 1), ref nos)) return true;
            cor[v] = -1;
        }
        return false;
    }

    /// <summary>
    /// O numero cromatico medido: sobe de uma em uma ate caber. Devolve tambem o
    /// custo das duas metades — o que foi gasto para PROVAR que c-1 nao basta, e
    /// o que foi gasto para ACHAR a coloracao com c.
    /// </summary>
    public static (int Cromatico, long NosDaProva, long NosDaAchada) Cromatico(Grafo g)
    {
        long prova = 0;
        for (var c = 1; ; c++)
        {
            var t = Cabe(g, c);
            if (t.Coube) return (c, prova, t.Nos);
            prova += t.Nos;
        }
    }

    /// <summary>
    /// O guloso na ordem em que os conjuntos aparecem: primeira cor que nao
    /// conflita. Serve de contraponto — e barato e quase sempre erra para cima.
    /// </summary>
    public static int[] Guloso(Grafo g)
    {
        var cor = new int[g.Ordem];
        Array.Fill(cor, -1);
        for (var v = 0; v < g.Ordem; v++)
        {
            var proibidas = new HashSet<int>();
            foreach (var w in g.Vizinhos(v))
                if (cor[w] >= 0) proibidas.Add(cor[w]);
            var c = 0;
            while (proibidas.Contains(c)) c++;
            cor[v] = c;
        }
        return cor;
    }
}
