using Conde.Kneser;
using Xunit;

namespace Conde.Kneser.Testes;

/// <summary>
/// Os numeros das tabelas do README, presos. Todos saem do medidor; nenhum foi
/// digitado de cabeca.
/// </summary>
public class MedidasTestes
{
    [Fact]
    public void ATabelaDaForma()
    {
        var linhas = Medidas.Formas();
        Assert.Equal(17, linhas.Count);
        Assert.All(linhas, l => Assert.True(l.Regular));

        Assert.Equal((4, 2, 6, 3, 1, true), linhas[0]);
        Assert.Equal((5, 2, 10, 15, 3, true), linhas[1]);
        Assert.Equal((10, 2, 45, 630, 28, true), linhas[6]);
        Assert.Equal((8, 3, 56, 280, 10, true), linhas[9]);
        Assert.Equal((10, 4, 210, 1575, 15, true), linhas[14]);
        Assert.Equal((11, 5, 462, 1386, 6, true), linhas[16]);
    }

    [Fact]
    public void OCromaticoMedidoBateComLovaszNosDezessete()
    {
        foreach (var (n, k) in Medidas.Pares)
        {
            var l = Medidas.LinhaCromatico(n, k);
            Assert.Equal(l.Lovasz, l.Cromatico);
        }
    }

    [Fact]
    public void ATabelaDoCromatico()
    {
        Assert.Equal((2, 1L, 6L), Corte(Medidas.LinhaCromatico(4, 2)));
        Assert.Equal((3, 5L, 10L), Corte(Medidas.LinhaCromatico(5, 2)));
        Assert.Equal((6, 225L, 28L), Corte(Medidas.LinhaCromatico(8, 2)));
        Assert.Equal((8, 36901L, 45L), Corte(Medidas.LinhaCromatico(10, 2)));
        Assert.Equal((4, 72L, 56L), Corte(Medidas.LinhaCromatico(8, 3)));
        Assert.Equal((5, 2972L, 84L), Corte(Medidas.LinhaCromatico(9, 3)));
        Assert.Equal((6, 225716L, 156L), Corte(Medidas.LinhaCromatico(10, 3)));
        Assert.Equal((4, 2642L, 210L), Corte(Medidas.LinhaCromatico(10, 4)));
        Assert.Equal((3, 150L, 462L), Corte(Medidas.LinhaCromatico(11, 5)));

        // O achado: provar que uma cor a menos nao basta custa 1.446 vezes mais
        // que achar a coloracao certa em K(10,3).
        var l = Medidas.LinhaCromatico(10, 3);
        Assert.Equal(1446, l.NosDaProva / l.NosDaAchada);
    }

    private static (int, long, long) Corte(
        (int N, int K, int Cromatico, int Lovasz, long NosDaProva, long NosDaAchada) l) =>
        (l.Cromatico, l.NosDaProva, l.NosDaAchada);

    [Fact]
    public void ATabelaDoMaiorIndependente()
    {
        var esperado = new (int N, int K, int Maior, long Nos)[]
        {
            (4, 2, 3, 34), (5, 2, 4, 155), (6, 2, 5, 553), (7, 2, 6, 1732),
            (8, 2, 7, 4977), (9, 2, 8, 13476), (10, 2, 9, 34974),
            (6, 3, 10, 38066), (7, 3, 15, 1240080), (8, 3, 21, 34802823),
        };
        Assert.Equal(esperado.Length, Medidas.ParesDoIndependente.Length);

        foreach (var (n, k, maior, nos) in esperado)
        {
            var l = Medidas.LinhaIndependente(n, k);
            Assert.Equal(maior, l.Maior);
            Assert.Equal(nos, l.Nos);
            // A busca, o teorema de Erdos-Ko-Rado e a estrela: tres caminhos,
            // um numero so.
            Assert.Equal(maior, l.Ekr);
            Assert.Equal(maior, l.Estrela);
        }
    }

    [Fact]
    public void ATabelaDaOrdemDeBusca()
    {
        var esperado = new (int N, int K, int Cores, long Saturacao, long Fixa)[]
        {
            (5, 2, 2, 4, 41),
            (6, 2, 3, 9, 538),
            (7, 2, 4, 33, 10878),
            (8, 2, 5, 209, 347863),
            (7, 3, 2, 11, 12433),
            (8, 3, 3, 67, 105415176),
        };
        Assert.Equal(esperado.Length, Medidas.ParesDaOrdem.Length);

        foreach (var (n, k, cores, saturacao, fixa) in esperado)
        {
            var l = Medidas.LinhaOrdem(n, k);
            Assert.Equal(cores, l.Cores);
            Assert.Equal(saturacao, l.Saturacao);
            Assert.Equal(fixa, l.OrdemFixa);
        }

        // O maior achado do repositorio: em K(8,3) a ordem por saturacao faz a
        // mesma prova com 67 nos no lugar de 105.415.176.
        Assert.Equal(1573360, (long)Medidas.LinhaOrdem(8, 3).Razao);
    }

    [Fact]
    public void ATabelaDasConstrucoes()
    {
        foreach (var (n, k) in Medidas.Pares)
        {
            var l = Medidas.LinhaConstrucoes(n, k);
            Assert.Equal(l.Lovasz, l.Natural);
            // O guloso na ordem crescente da mascara acerta em todos os
            // dezessete: visitar os conjuntos pelo menor elemento refaz, sem
            // saber, a coloracao natural.
            Assert.Equal(l.Lovasz, l.Guloso);
        }
    }

    [Fact]
    public void ATabelaDaCotaPorIndependente()
    {
        var esperado = new (int N, int K, int Lovasz, int Cota, int Diferenca)[]
        {
            (4, 2, 2, 2, 0), (5, 2, 3, 3, 0), (6, 2, 4, 3, 1), (7, 2, 5, 4, 1),
            (8, 2, 6, 4, 2), (9, 2, 7, 5, 2), (10, 2, 8, 5, 3),
            (6, 3, 2, 2, 0), (7, 3, 3, 3, 0), (8, 3, 4, 3, 1), (9, 3, 5, 3, 2),
            (10, 3, 6, 4, 2),
            (8, 4, 2, 2, 0), (9, 4, 3, 3, 0), (10, 4, 4, 3, 1),
            (10, 5, 2, 2, 0), (11, 5, 3, 3, 0),
        };
        Assert.Equal(esperado.Length, Medidas.Pares.Length);
        foreach (var (n, k, lovasz, cota, diferenca) in esperado)
            Assert.Equal((n, k, lovasz, cota, diferenca), Medidas.LinhaCota(n, k));

        // A cota de contagem nunca passa do valor certo, e a distancia cresce.
        Assert.All(esperado, e => Assert.True(e.Diferenca >= 0));
        Assert.Equal(3, esperado.Max(e => e.Diferenca));
    }
}
