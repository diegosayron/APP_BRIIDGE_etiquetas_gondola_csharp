namespace APP_BRIIDGE_etiquetas.Models;

public class Produto
{
    public string Id { get; set; } = "";

    public string Codigo { get; set; } = "";
    
    public string CodigoSistema { get; set; } = "";

    public string CodigoNFe { get; set; } = "";

    public string Nome { get; set; } = "";

    public string Unidade { get; set; } = "";

    public decimal PrecoVenda { get; set; }
}