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

    private async Task<string> BuscarProduto(string pesquisa, bool pesquisarPorEAN)
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
                ehPesquisaSimples = !pesquisarPorEAN,
                invalidGenre = Array.Empty<string>(),
                types = Array.Empty<string>(),
                possuiComposicao = false,
                somenteComEstoque = false,
                cadastroInativo = false,
                fornecedor = "",
                codigoEAN = pesquisarPorEAN ? pesquisa : "",
                marca = "",
                prateleira = "",
                categoria = "",
                atributo = "",
                genero = "",
                tipo = "",
                pesquisaSimples = pesquisarPorEAN ? "" : pesquisa
            }
        };

        string json = JsonSerializer.Serialize(body);

        HttpResponseMessage response = await _http.PostAsync(
            "https://novaapi-lb1.vendaerp.com.br/v3/produtos/produtos",
            new StringContent(json, Encoding.UTF8, "application/json"));

        response.EnsureSuccessStatusCode();

        return await response.Content.ReadAsStringAsync();
    }



    public async Task<Produto?> BuscarProdutoAsync(string pesquisa, bool pesquisarPorEAN)
    {
        AtualizarToken(TokenProvider.Token!);

        string json = await BuscarProduto(pesquisa, pesquisarPorEAN);

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

        // Código interno
        produto.CodigoSistema = p.GetProperty("Codigo").ToString();

        // Código NFe retornado pela API
        produto.CodigoNFe = p.GetProperty("CodigoNFe").ToString();

        // Código que será impresso
        if (pesquisarPorEAN)
        {
            // A API não devolve o EAN.
            // Então usamos o mesmo código pesquisado.
            produto.Codigo = pesquisa;
        }
        else
        {
            // Pesquisa pelo código interno.
            // Se houver um CódigoNFe diferente, ele será impresso.
            produto.Codigo = string.IsNullOrWhiteSpace(produto.CodigoNFe)
                ? produto.CodigoSistema
                : produto.CodigoNFe;
        }

        produto.Nome = p.GetProperty("Nome").ToString();

        produto.Unidade = p.GetProperty("UnidadeComercial_NFe").ToString();

        produto.PrecoVenda = decimal.Parse(
            p.GetProperty("PrecoVenda").ToString(),
            System.Globalization.CultureInfo.InvariantCulture);

        return produto;
    }
}