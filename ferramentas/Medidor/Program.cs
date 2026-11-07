using Conde.Kneser;

// Uma tabela por argumento, para o CI rodar uma de cada vez.
var qual = args.Length > 0 ? args[0] : "tudo";

if (qual is "forma" or "tudo")
{
    Console.WriteLine("K(n,k) | vertices | arestas | grau | regular");
    foreach (var (n, k, vertices, arestas, grau, regular) in Medidas.Formas())
        Console.WriteLine($"K({n},{k})  | {vertices,8} | {arestas,7} | {grau,4} | {regular,7}");
    Console.WriteLine();
}

if (qual is "cromatico" or "tudo")
{
    Console.WriteLine("K(n,k) | cromatico medido | n-2k+2 | nos da prova | nos da achada");
    foreach (var (n, k) in Medidas.Pares)
    {
        var l = Medidas.LinhaCromatico(n, k);
        Console.WriteLine($"K({n},{k})  | {l.Cromatico,16} | {l.Lovasz,6} | {l.NosDaProva,12} | {l.NosDaAchada,13}");
    }
    Console.WriteLine();
}

if (qual is "independente" or "tudo")
{
    Console.WriteLine("K(n,k) | maior medido | C(n-1,k-1) | estrela | nos");
    foreach (var (n, k) in Medidas.ParesDoIndependente)
    {
        var l = Medidas.LinhaIndependente(n, k);
        Console.WriteLine($"K({n},{k})  | {l.Maior,12} | {l.Ekr,10} | {l.Estrela,7} | {l.Nos,12}");
    }
    Console.WriteLine();
}

if (qual is "construcoes" or "tudo")
{
    Console.WriteLine("K(n,k) | coloracao natural | guloso na ordem | n-2k+2");
    foreach (var (n, k) in Medidas.Pares)
    {
        var l = Medidas.LinhaConstrucoes(n, k);
        Console.WriteLine($"K({n},{k})  | {l.Natural,17} | {l.Guloso,15} | {l.Lovasz,6}");
    }
    Console.WriteLine();
}

if (qual is "ordem" or "tudo")
{
    Console.WriteLine("K(n,k) | cores | nos por saturacao | nos em ordem fixa | razao");
    foreach (var (n, k) in Medidas.ParesDaOrdem)
    {
        var l = Medidas.LinhaOrdem(n, k);
        Console.WriteLine($"K({n},{k})  | {l.Cores,5} | {l.Saturacao,17} | {l.OrdemFixa,17} | {l.Razao,12:F0}x");
    }
    Console.WriteLine();
}

if (qual is "cota" or "tudo")
{
    Console.WriteLine("K(n,k) | cromatico | cota por independente | diferenca");
    foreach (var (n, k) in Medidas.Pares)
    {
        var l = Medidas.LinhaCota(n, k);
        Console.WriteLine($"K({n},{k})  | {l.Lovasz,9} | {l.Cota,21} | {l.Diferenca,9}");
    }
    Console.WriteLine();
}
