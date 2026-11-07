namespace Conde.Kneser;

/// <summary>
/// As tabelas do README. Todo numero de la sai daqui e esta preso em teste.
/// </summary>
public static class Medidas
{
    /// <summary>Os pares (n, k) que entram nas tabelas caras, do menor para o maior.</summary>
    public static readonly (int N, int K)[] Pares =
    {
        (4, 2), (5, 2), (6, 2), (7, 2), (8, 2), (9, 2), (10, 2),
        (6, 3), (7, 3), (8, 3), (9, 3), (10, 3),
        (8, 4), (9, 4), (10, 4),
        (10, 5), (11, 5),
    };

    /// <summary>
    /// Os pares da tabela do maior conjunto independente. Para a bem antes das
    /// outras: a busca de K(9,3) passa de dois bilhoes de nos e seis minutos, e
    /// foi cortada.
    /// </summary>
    public static readonly (int N, int K)[] ParesDoIndependente =
    {
        (4, 2), (5, 2), (6, 2), (7, 2), (8, 2), (9, 2), (10, 2),
        (6, 3), (7, 3), (8, 3),
    };

    /// <summary>
    /// Os pares da tabela que compara as duas ordens de busca. E curta de
    /// proposito: a ordem fixa nao passa de K(8,3) em tempo de rodar no CI.
    /// </summary>
    public static readonly (int N, int K)[] ParesDaOrdem =
    {
        (5, 2), (6, 2), (7, 2), (8, 2), (7, 3), (8, 3),
    };

    public static (int N, int K, int Vertices, int Arestas, int Grau, bool Regular) Forma(int n, int k)
    {
        var g = new Grafo(n, k);
        return (n, k, g.Ordem, g.Arestas(), g.Grau(0), g.EhRegular());
    }

    public static List<(int N, int K, int Vertices, int Arestas, int Grau, bool Regular)> Formas() =>
        Pares.Select(p => Forma(p.N, p.K)).ToList();

    /// <summary>A linha cara: o cromatico por busca, contra a formula de Lovasz.</summary>
    public static (int N, int K, int Cromatico, int Lovasz, long NosDaProva, long NosDaAchada)
        LinhaCromatico(int n, int k)
    {
        var g = new Grafo(n, k);
        var (cromatico, prova, achada) = Coloracao.Cromatico(g);
        return (n, k, cromatico, Juiz.Lovasz(n, k), prova, achada);
    }

    /// <summary>A linha do maior independente: busca contra Erdos, Ko e Rado.</summary>
    public static (int N, int K, int Maior, int Ekr, int Estrela, long Nos)
        LinhaIndependente(int n, int k)
    {
        var g = new Grafo(n, k);
        var (maior, nos) = Independentes.Maior(g);
        return (n, k, maior, Juiz.ErdosKoRado(n, k), Independentes.Estrela(g).Count, nos);
    }

    /// <summary>A coloracao natural e o guloso, contra o cromatico de verdade.</summary>
    public static (int N, int K, int Natural, int Guloso, int Lovasz) LinhaConstrucoes(int n, int k)
    {
        var g = new Grafo(n, k);
        var natural = Coloracao.Natural(g);
        var guloso = Coloracao.Guloso(g);
        if (!Coloracao.EhPropria(g, natural)) throw new InvalidOperationException("natural nao e propria");
        if (!Coloracao.EhPropria(g, guloso)) throw new InvalidOperationException("guloso nao e propria");
        return (n, k, Coloracao.QuantasCores(natural), Coloracao.QuantasCores(guloso), Juiz.Lovasz(n, k));
    }

    /// <summary>
    /// O que a ordem por saturacao poupa na parte CARA: provar que uma cor a
    /// menos que o cromatico nao basta.
    /// </summary>
    public static (int N, int K, int Cores, long Saturacao, long OrdemFixa, double Razao)
        LinhaOrdem(int n, int k)
    {
        var g = new Grafo(n, k);
        var cores = Juiz.Lovasz(n, k) - 1;
        var sat = Coloracao.Cabe(g, cores);
        var fixa = Coloracao.CabeOrdemFixa(g, cores);
        if (sat.Coube || fixa.Coube) throw new InvalidOperationException("uma cor a menos nao podia caber");
        return (n, k, cores, sat.Nos, fixa.Nos, (double)fixa.Nos / sat.Nos);
    }

    /// <summary>A cota que sai do independente, contra o valor certo.</summary>
    public static (int N, int K, int Lovasz, int Cota, int Diferenca) LinhaCota(int n, int k)
    {
        var cota = Juiz.CotaPorIndependente(n, k);
        var certo = Juiz.Lovasz(n, k);
        return (n, k, certo, cota, certo - cota);
    }
}
