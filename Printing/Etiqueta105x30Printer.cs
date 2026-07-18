using APP_BRIIDGE_etiquetas.Models;
using System.Drawing.Printing;
using ZXing;
using ZXing.Common;
using ZXing.Windows.Compatibility;


namespace APP_BRIIDGE_etiquetas.Printing;

public class Etiqueta105x30Printer : EtiquetaPrinterBase
{
    public Etiqueta105x30Printer(Produto produto)
        : base(produto)
    {
    }

    protected override void ConfigurarPagina(PrintDocument pd)
    {
        pd.DefaultPageSettings.PaperSize =
            new PaperSize(
                "Etiqueta105x30",
                Medidas.Mm(105),
                Medidas.Mm(30));

        pd.DefaultPageSettings.Margins =
            new Margins(0, 0, 0, 0);

        pd.DefaultPageSettings.Landscape = false;
    }

    protected override void PrintPage(
    object? sender,
    PrintPageEventArgs e)
    {
        Graphics g = e.Graphics;

        float mmX = g.DpiX / 25.4f;
        float mmY = g.DpiY / 25.4f;

        string descricao = Produto.Nome;
        string linha1 = descricao;
        string linha2 = "";

        if (descricao.Length > 28)
        {
            int pos = descricao.LastIndexOf(' ', 28);

            if (pos > 0)
            {
                linha1 = descricao[..pos];
                linha2 = descricao[(pos + 1)..];
            }
        }

        using Font fonteDescricao =
            new Font(
            "Arial Narrow",
            14,
            FontStyle.Bold);

        using Font fonteCodigo =
            new("Arial", 8);

        using Font fontePreco =
        new(
            "Arial Narrow",
            40,
            FontStyle.Bold);

        // Descrição
        g.DrawString(
            linha1,
            fonteDescricao,
            Brushes.Black,
            2 * mmX,
            1 * mmY);

        if (!string.IsNullOrWhiteSpace(linha2))
        {
            g.DrawString(
                linha2,
                fonteDescricao,
                Brushes.Black,
                2 * mmX,
                3 * mmY);
        }

        // Código interno
        g.DrawString(
            Produto.Codigo,
            fonteCodigo,
            Brushes.Black,
            2 * mmX,
            6 * mmY);

        // Unidade
        SizeF tamanhoUnidade =
            g.MeasureString(
                Produto.Unidade,
                fonteDescricao);

        SizeF tamLinha2 = g.MeasureString(
        linha2,
        fonteDescricao);

        g.DrawString(
            Produto.Unidade,
            fonteDescricao,
            Brushes.Black,
            4 * mmX + tamLinha2.Width,
            3 * mmY);

        // Preço
        string preco =
            Produto.PrecoVenda.ToString("C2");

        StringFormat sf = new()
        {
            Alignment = StringAlignment.Near
        };

        RectangleF areaPreco = new RectangleF(
        14 * mmX,
        4 * mmY,
        100 * mmX,
        18 * mmY);

        g.DrawString(
            preco,
            fontePreco,
            Brushes.Black,
            areaPreco,
            sf);

        //CODIGO BARRAS DO RODAPÉ DA ETIQUETA
        Bitmap codigoBarras = GerarCodigoBarras(Produto.Codigo);

        g.DrawImage(
            codigoBarras,
            new RectangleF(
                6 * mmX,
                17 * mmY,
                93 * mmX,
                10 * mmY));
    }


    private Bitmap GerarCodigoBarras(string codigo)
    {
        BarcodeFormat formato =
            codigo.Length == 13 && codigo.All(char.IsDigit)
                ? BarcodeFormat.EAN_13
                : BarcodeFormat.CODE_128;

        var writer = new BarcodeWriter
        {
            Format = formato,
            Options = new EncodingOptions
            {
                Width = 420,
                Height = 70,
                Margin = 0,
                PureBarcode = true
            }
        };

        return writer.Write(codigo);
    }
}