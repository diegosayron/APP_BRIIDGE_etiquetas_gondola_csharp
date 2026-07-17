using APP_BRIIDGE_etiquetas.Models;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Text;


namespace APP_BRIIDGE_etiquetas.Api;

public class ApiClient
{
    private readonly HttpClient _http;

    public ApiClient()
    {
        _http = new HttpClient();

        _http.BaseAddress =
            new Uri("https://novaapi-lb1.vendaerp.com.br/");
    }

    public void AtualizarToken(string token)
    {
        _http.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", token);
    }

    private async Task<string> BuscarProduto(string pesquisa)
    {
        var body = new
        {
            pagina = new
            {
                number = 0,
                lenght = 15
            },
            order = new
            {
                ascending = false,
                fieldName = "Codigo"
            },
            filtro = new
            {
                ehPesquisaSimples = true,
                invalidGenre = Array.Empty<string>(),
                types = Array.Empty<string>(),
                possuiComposicao = false,
                somenteComEstoque = false,
                cadastroInativo = false,
                fornecedor = "",
                codigoEAN = "",
                marca = "",
                prateleira = "",
                categoria = "",
                atributo = "",
                genero = "",
                tipo = "",
                pesquisaSimples = pesquisa
            }
        };

        string json = JsonSerializer.Serialize(body);

        HttpResponseMessage response = await _http.PostAsync(
            "https://novaapi-lb1.vendaerp.com.br/v3/produtos/produtos",
            new StringContent(json, Encoding.UTF8, "application/json"));

        response.EnsureSuccessStatusCode();

        return await response.Content.ReadAsStringAsync();
    }

    public async Task<Produto?> BuscarProdutoAsync(string pesquisa)
    {
        AtualizarToken(TokenProvider.Token!);

        string json = await BuscarProduto(pesquisa);

        using JsonDocument doc = JsonDocument.Parse(json);

        if (!doc.RootElement.TryGetProperty("Data", out JsonElement data))
            return null;

        if (!data.TryGetProperty("Itens", out JsonElement itens))
            return null;

        if (itens.GetArrayLength() == 0)
            return null;

        JsonElement p = itens[0];
        //File.WriteAllText("produto.json", p.ToString());
        //MessageBox.Show(p.ToString());

        //return new Produto
        //{
        //Id = p.GetProperty("Id").GetInt32(),
        //Codigo = p.GetProperty("Codigo").GetString() ?? "",
        //CodigoNFe = p.GetProperty("CodigoNFe").GetString() ?? "",
        //Nome = p.GetProperty("Nome").GetString() ?? "",
        //Unidade = p.GetProperty("Unidade").GetString() ?? "",
        //PrecoVenda = p.GetProperty("PrecoVenda").GetDecimal()
        //};

        Produto produto = new();

        produto.Id = p.GetProperty("Id").ToString();

        // Código Sistema (7908414459028)
        produto.CodigoSistema = p.GetProperty("Codigo").ToString();

        // Código do produto (19725)
        produto.Codigo = p.GetProperty("CodigoNFe").ToString();

        // Mantemos por compatibilidade
        produto.CodigoNFe = produto.Codigo;

        produto.Nome = p.GetProperty("Nome").ToString();

        produto.Unidade = p.GetProperty("UnidadeComercial_NFe").ToString();

        produto.PrecoVenda = decimal.Parse(
            p.GetProperty("PrecoVenda").ToString(),
            System.Globalization.CultureInfo.InvariantCulture);

        return produto;
    }
}