using APP_BRIIDGE_etiquetas.Models;
using System.Drawing.Printing;

namespace APP_BRIIDGE_etiquetas.Printing;

public abstract class EtiquetaPrinterBase
{
    protected Produto Produto { get; }

    protected EtiquetaPrinterBase(Produto produto)
    {
        Produto = produto;
    }

    public void Imprimir(string impressora, int quantidade)
    {
        PrintDocument pd = new();

        pd.PrinterSettings.PrinterName = impressora;
        pd.PrinterSettings.Copies = (short)quantidade;

        pd.PrintPage += PrintPage;

        ConfigurarPagina(pd);

        pd.Print();
    }

    protected virtual void ConfigurarPagina(PrintDocument pd)
    {
    }

    protected abstract void PrintPage(
        object? sender,
        PrintPageEventArgs e);
}