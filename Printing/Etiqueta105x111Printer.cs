using APP_BRIIDGE_etiquetas.Models;
using System.Drawing;
using System.Drawing.Printing;

namespace APP_BRIIDGE_etiquetas.Printing;

public class Etiqueta105x111Printer : EtiquetaPrinterBase
{
    public Etiqueta105x111Printer(Produto produto)
        : base(produto)
    {
    }

    protected override void ConfigurarPagina(PrintDocument pd)
    {
        pd.DefaultPageSettings.Margins =
            new Margins(5, 5, 5, 5);

        pd.DefaultPageSettings.Landscape = false;
    }

    protected override void PrintPage(
        object? sender,
        PrintPageEventArgs e)
    {

    }
}