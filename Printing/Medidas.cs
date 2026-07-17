namespace APP_BRIIDGE_etiquetas.Printing;

public static class Medidas
{
    /// <summary>
    /// Converte milímetros para centésimos de polegada
    /// (unidade usada pelo PrintDocument).
    /// </summary>
    public static int Mm(double mm)
    {
        return (int)Math.Round(mm * 100.0 / 25.4);
    }
}