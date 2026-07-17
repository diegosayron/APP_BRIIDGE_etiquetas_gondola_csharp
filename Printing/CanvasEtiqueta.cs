using System.Drawing;
using System.Drawing.Drawing2D;

namespace APP_BRIIDGE_etiquetas.Printing;

public class CanvasEtiqueta
{
    private readonly Graphics g;

    public CanvasEtiqueta(Graphics graphics)
    {
        g = graphics;

        g.SmoothingMode = SmoothingMode.HighQuality;
        g.InterpolationMode = InterpolationMode.HighQualityBicubic;
        g.TextRenderingHint =
            System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
    }

    public float X(float mm)
    {
        return mm * g.DpiX / 25.4f;
    }

    public float Y(float mm)
    {
        return mm * g.DpiY / 25.4f;
    }

    public void Texto(
        string texto,
        Font fonte,
        float x,
        float y,
        StringAlignment alinhamento = StringAlignment.Near)
    {
        using StringFormat sf = new();

        sf.Alignment = alinhamento;
        sf.LineAlignment = StringAlignment.Near;

        g.DrawString(
            texto,
            fonte,
            Brushes.Black,
            new RectangleF(
                X(x),
                Y(y),
                X(100),
                Y(10)),
            sf);
    }

    public void Linha(
        float x1,
        float y1,
        float x2,
        float y2)
    {
        g.DrawLine(
            Pens.Black,
            X(x1),
            Y(y1),
            X(x2),
            Y(y2));
    }

    public void Retangulo(
        float x,
        float y,
        float largura,
        float altura)
    {
        g.DrawRectangle(
            Pens.Black,
            X(x),
            Y(y),
            X(largura),
            Y(altura));
    }
}