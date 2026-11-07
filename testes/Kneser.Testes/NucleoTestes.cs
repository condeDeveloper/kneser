using Conde.Kneser;
using Xunit;

namespace Conde.Kneser.Testes;

/// <summary>
/// O comportamento: a forma do grafo, o que e coloracao propria e o que e
/// familia que se cruza. Os numeros das tabelas ficam no <c>MedidasTestes</c>.
/// </summary>
public class NucleoTestes
{
    [Fact]
    public void KDeCincoDoisEOPetersen()
    {
        var g = new Grafo(5, 2);
        Assert.Equal(10, g.Ordem);
        Assert.Equal(15, g.Arestas());
        Assert.True(g.EhRegular());
        Assert.Equal(3, g.Grau(0));
    }

    [Theory]
    [InlineData(3)]
    [InlineData(5)]
    [InlineData(8)]
    public void KDeNUmEOCompleto(int n)
    {
        var g = new Grafo(n, 1);
        Assert.Equal(n, g.Ordem);
        Assert.Equal(n * (n - 1) / 2, g.Arestas());
        Assert.Equal(n, Coloracao.QuantasCores(Coloracao.Natural(g)));
        Assert.Equal(n, Juiz.Lovasz(n, 1));
    }

    [Fact]
    public void AbaixoDeDoisKNaoEGrafoDeKneser()
    {
        Assert.Throws<ArgumentException>(() => new Grafo(5, 3));
    }

    [Theory]
    [InlineData(5, 2)]
    [InlineData(7, 3)]
    [InlineData(9, 4)]
    [InlineData(10, 3)]
    public void AColoracaoNaturalEPropriaEGastaOQueOTeoremaDiz(int n, int k)
    {
        var g = new Grafo(n, k);
        var cores = Coloracao.Natural(g);
        Assert.True(Coloracao.EhPropria(g, cores));
        Assert.True(Juiz.EhPropriaPelaDefinicao(n, k, cores));
        Assert.Equal(Juiz.Lovasz(n, k), Coloracao.QuantasCores(cores));
    }

    [Theory]
    [InlineData(6, 2)]
    [InlineData(8, 3)]
    [InlineData(10, 4)]
    public void OGulosoTambemDaUmaColoracaoPropria(int n, int k)
    {
        var g = new Grafo(n, k);
        var cores = Coloracao.Guloso(g);
        Assert.True(Coloracao.EhPropria(g, cores));
        Assert.True(Juiz.EhPropriaPelaDefinicao(n, k, cores));
    }

    [Theory]
    [InlineData(6, 2)]
    [InlineData(7, 3)]
    [InlineData(9, 4)]
    public void AEstrelaEIndependenteETemOTamanhoDeErdosKoRado(int n, int k)
    {
        var g = new Grafo(n, k);
        var estrela = Independentes.Estrela(g);
        Assert.Equal(Juiz.ErdosKoRado(n, k), estrela.Count);
        Assert.True(Juiz.EhIndependentePelaDefinicao(n, k, estrela));
    }

    [Theory]
    [InlineData(6, 2)]
    [InlineData(7, 3)]
    [InlineData(9, 4)]
    public void OGrauEAsArestasBatemComAContagem(int n, int k)
    {
        var g = new Grafo(n, k);
        Assert.Equal(Juiz.GrauEsperado(n, k), g.Grau(0));
        Assert.Equal(Juiz.ArestasEsperadas(n, k), g.Arestas());
        Assert.True(g.EhRegular());
    }

    [Theory]
    [InlineData(5, 2)]
    [InlineData(7, 3)]
    [InlineData(9, 4)]
    public void UmaCorAMenosNaoCabe(int n, int k)
    {
        var g = new Grafo(n, k);
        var certo = Juiz.Lovasz(n, k);
        Assert.True(Coloracao.Cabe(g, certo).Coube);
        Assert.False(Coloracao.Cabe(g, certo - 1).Coube);
    }

    [Fact]
    public void AsDuasOrdensDeBuscaRespondemOMesmo()
    {
        foreach (var (n, k) in new[] { (5, 2), (6, 2), (7, 3) })
        {
            var g = new Grafo(n, k);
            for (var c = 1; c <= Juiz.Lovasz(n, k) + 1; c++)
                Assert.Equal(Coloracao.Cabe(g, c).Coube, Coloracao.CabeOrdemFixa(g, c).Coube);
        }
    }

    [Fact]
    public void OsSubconjuntosSaemNaQuantidadeCerta()
    {
        for (var n = 0; n <= 12; n++)
            for (var k = 0; k <= n; k++)
                Assert.Equal(Subconjuntos.Binomial(n, k), Subconjuntos.DeTamanho(n, k).Count);
    }

    [Fact]
    public void OBinomialNasBordas()
    {
        Assert.Equal(1, Subconjuntos.Binomial(7, 0));
        Assert.Equal(1, Subconjuntos.Binomial(7, 7));
        Assert.Equal(0, Subconjuntos.Binomial(7, 8));
        Assert.Equal(0, Subconjuntos.Binomial(7, -1));
        Assert.Equal(35, Subconjuntos.Binomial(7, 3));
    }

    [Fact]
    public void OMenorElementoEAEscrita()
    {
        var conjunto = (1u << 2) | (1u << 5) | (1u << 9);
        Assert.Equal(2, Subconjuntos.Menor(conjunto));
        Assert.Equal("{2,5,9}", Subconjuntos.Escrever(conjunto));
        Assert.True(Subconjuntos.Disjuntos(conjunto, (1u << 3) | (1u << 4)));
        Assert.False(Subconjuntos.Disjuntos(conjunto, (1u << 3) | (1u << 5)));
    }

    [Fact]
    public void AMaiorFamiliaQueSeCruzaEIndependentePelaDefinicao()
    {
        var g = new Grafo(7, 3);
        var (tamanho, _) = Independentes.Maior(g);
        Assert.Equal(Juiz.ErdosKoRado(7, 3), tamanho);
    }
}
