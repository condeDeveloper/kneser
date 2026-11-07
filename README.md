# kneser

**Pinte os subconjuntos de tamanho *k* de um conjunto de *n* elementos de modo
que dois subconjuntos disjuntos nunca tenham a mesma cor. Quantas cores são
precisas?**

Kneser perguntou isso em 1955 e respondeu com um palpite: *n − 2k + 2*. A
construção que gasta essas cores é fácil e cabe em três linhas. Provar que
nenhuma a menos resolve levou **vinte e três anos** e, quando saiu, a prova de
Lovász não era de combinatória: era topologia, o teorema de Borsuk-Ulam.

Aqui o teorema não é provado. Ele é **conferido**, um grafo por vez, em
dezessete grafos de Kneser — e a conferência é a parte cara, porque dizer "não
cabe em *c* cores" exige esgotar todas as maneiras de tentar.

## O grafo

K(*n*, *k*) tem um vértice para cada subconjunto de tamanho *k*, e uma aresta
entre dois vértices quando os conjuntos são disjuntos. K(5,2) é o grafo de
Petersen. K(*n*,1) é o grafo completo.

| K(n,k) | vértices | arestas | grau | regular |
|:------|---------:|--------:|-----:|:-------:|
| K(4,2) | 6 | 3 | 1 | sim |
| K(5,2) | 10 | 15 | 3 | sim |
| K(6,2) | 15 | 45 | 6 | sim |
| K(7,2) | 21 | 105 | 10 | sim |
| K(8,2) | 28 | 210 | 15 | sim |
| K(9,2) | 36 | 378 | 21 | sim |
| K(10,2) | 45 | 630 | 28 | sim |
| K(6,3) | 20 | 10 | 1 | sim |
| K(7,3) | 35 | 70 | 4 | sim |
| K(8,3) | 56 | 280 | 10 | sim |
| K(9,3) | 84 | 840 | 20 | sim |
| K(10,3) | 120 | 2.100 | 35 | sim |
| K(8,4) | 70 | 35 | 1 | sim |
| K(9,4) | 126 | 315 | 5 | sim |
| K(10,4) | 210 | 1.575 | 15 | sim |
| K(10,5) | 252 | 126 | 1 | sim |
| K(11,5) | 462 | 1.386 | 6 | sim |

## O número cromático, medido

A busca sobe de uma cor em uma cor até caber. O que ela gasta vem partido em
dois: os nós para **provar** que a cor anterior não basta, e os nós para
**achar** a coloração que cabe.

| K(n,k) | χ medido | n−2k+2 | nós da prova | nós da achada |
|:------|---------:|-------:|-------------:|--------------:|
| K(4,2) | 2 | 2 | 1 | 6 |
| K(5,2) | 3 | 3 | 5 | 10 |
| K(6,2) | 4 | 4 | 12 | 15 |
| K(7,2) | 5 | 5 | 41 | 21 |
| K(8,2) | 6 | 6 | 225 | 28 |
| K(9,2) | 7 | 7 | 1.088 | 36 |
| K(10,2) | 8 | 8 | 36.901 | 45 |
| K(6,3) | 2 | 2 | 1 | 20 |
| K(7,3) | 3 | 3 | 12 | 35 |
| K(8,3) | 4 | 4 | 72 | 56 |
| K(9,3) | 5 | 5 | 2.972 | 84 |
| K(10,3) | 6 | 6 | 225.716 | 156 |
| K(8,4) | 2 | 2 | 1 | 70 |
| K(9,4) | 3 | 3 | 41 | 126 |
| K(10,4) | 4 | 4 | 2.642 | 210 |
| K(10,5) | 2 | 2 | 1 | 252 |
| K(11,5) | 3 | 3 | 150 | 462 |

Dezessete grafos, dezessete acertos. E a assimetria do teorema aparece na
medida: em K(10,3), **achar** a coloração custa 156 nós e **provar** que cinco
cores não bastam custa 225.716 — 1.446 vezes mais.

K(11,3) e K(11,4) também terminam, mas passam de dois minutos cada e ficaram
fora da tabela que roda no CI. K(12,4) não terminou.

## A ordem da busca vale mais que tudo

A primeira versão escolhia a ordem dos vértices uma vez só, do mais ligado para
o menos. A segunda escolhe a cada passo o vértice mais **saturado**: o que já
tem mais cores diferentes em volta. As duas provam a mesma coisa.

| K(n,k) | cores | nós por saturação | nós em ordem fixa | razão |
|:------|------:|------------------:|------------------:|------:|
| K(5,2) | 2 | 4 | 41 | 10x |
| K(6,2) | 3 | 9 | 538 | 60x |
| K(7,2) | 4 | 33 | 10.878 | 330x |
| K(8,2) | 5 | 209 | 347.863 | 1.664x |
| K(7,3) | 2 | 11 | 12.433 | 1.130x |
| K(8,3) | 3 | 67 | 105.415.176 | 1.573.360x |

Com a ordem fixa, a tabela inteira do número cromático levava **oito minutos** e
não passava de K(8,3). Com a saturação ela leva **1,3 segundo** e vai até
K(11,5).

## O maior conjunto independente

Um conjunto independente em K(*n*,*k*) é uma família de conjuntos que **se
cruzam** dois a dois. O teorema de Erdős, Ko e Rado (1961) diz que o maior tem
C(*n*−1, *k*−1) membros, o tamanho da "estrela": todos os conjuntos que contêm
um elemento fixo.

| K(n,k) | maior, por busca | C(n−1,k−1) | a estrela | nós |
|:------|-----------------:|-----------:|----------:|----:|
| K(4,2) | 3 | 3 | 3 | 34 |
| K(5,2) | 4 | 4 | 4 | 155 |
| K(6,2) | 5 | 5 | 5 | 553 |
| K(7,2) | 6 | 6 | 6 | 1.732 |
| K(8,2) | 7 | 7 | 7 | 4.977 |
| K(9,2) | 8 | 8 | 8 | 13.476 |
| K(10,2) | 9 | 9 | 9 | 34.974 |
| K(6,3) | 10 | 10 | 10 | 38.066 |
| K(7,3) | 15 | 15 | 15 | 1.240.080 |
| K(8,3) | 21 | 21 | 21 | 34.802.823 |

A busca de K(9,3) passa de dois bilhões de nós e seis minutos, e foi cortada.

## A cota que a contagem dá, e o quanto ela erra

Como nenhuma cor cobre mais que C(*n*−1,*k*−1) vértices, são precisas pelo menos
C(*n*,*k*)/C(*n*−1,*k*−1) = *n*/*k* cores. É uma cota honesta e **nunca passa**
do valor certo — mas fica cada vez mais longe dele.

| K(n,k) | χ | cota por contagem | atrás por |
|:------|--:|------------------:|----------:|
| K(6,2) | 4 | 3 | 1 |
| K(8,2) | 6 | 4 | 2 |
| K(10,2) | 8 | 5 | 3 |
| K(8,3) | 4 | 3 | 1 |
| K(9,3) | 5 | 3 | 2 |
| K(10,3) | 6 | 4 | 2 |
| K(10,4) | 4 | 3 | 1 |

É por isso que o problema esperou vinte e três anos: a conta fácil dá o valor
certo só nos casos pequenos, e depois desiste.

## O que as medidas me corrigiram

- **Escrevi três razões de cabeça, e as três estavam erradas por um.**
  225.716/156 é 1.446,9 e eu escrevi 1.447; 105.415.176/67 é 1.573.360,8 e eu
  escrevi 1.573.361. É divisão inteira, e arredondar para cima é inventar nó que
  não foi visitado. Pior: os números foram digitados dentro do `MedidasTestes`,
  que é o arquivo que existe justamente para impedir isso. O teste pegou os três.
- **Chutei o grau de K(11,5) olhando a linha de cima.** Escrevi 12 e 2.772; o
  certo é 6 e 1.386, porque o grau é C(*n*−*k*, *k*) = C(6,5) = 6. A tabela tem
  que vir do medidor, não da vizinha.
- **Achei que a ordem fosse detalhe de implementação.** Trocar a ordem fixa pela
  saturação não mudou uma linha da resposta e cortou a prova de K(8,3) de
  105.415.176 nós para 67. A busca é a mesma; o que mudou foi por onde ela
  começa.
- **O guloso acerta sempre, e isso não é sorte.** Eu esperava que ele estourasse
  o número cromático em algum caso. Ele acerta nos dezessete, e o motivo é que a
  ordem crescente da máscara visita os conjuntos pelo menor elemento — refazendo,
  sem saber, a própria coloração natural.

## Quatro achados

1. O número cromático medido por busca exaustiva bate com *n* − 2*k* + 2 nos
   **dezessete** grafos de Kneser da tabela, de K(4,2) a K(11,5).
2. Trocar a ordem fixa pela ordem por saturação faz a mesma prova de K(8,3) com
   **67 nós no lugar de 105.415.176** — 1.573.360 vezes menos, sem mudar a
   resposta.
3. Em K(10,3), **achar** a coloração de seis cores custa 156 nós e **provar** que
   cinco não bastam custa 225.716: a parte difícil do teorema é 1.446 vezes mais
   cara que a parte fácil.
4. A cota que sai da contagem de conjuntos independentes nunca passa do valor
   certo e fica até **3 cores atrás** dele, em K(10,2).

## Rodar

```bash
dotnet build src/Kneser/Kneser.csproj -c Release
dotnet run --project ferramentas/Medidor -c Release -- forma
dotnet run --project ferramentas/Medidor -c Release -- cromatico
dotnet run --project ferramentas/Medidor -c Release -- ordem
dotnet run --project ferramentas/Medidor -c Release -- independente
dotnet run --project ferramentas/Medidor -c Release -- construcoes
dotnet run --project ferramentas/Medidor -c Release -- cota
```

São 33 testes.

```bash
dotnet test testes/Kneser.Testes/Kneser.Testes.csproj -c Release
```

## Licença

MIT.
