namespace APP_BRIIDGE_etiquetas;

public static class TokenProvider
{
    public static string? Token { get; private set; }

    public static bool Definir(string token)
    {
        if (!string.IsNullOrWhiteSpace(Token))
            return false;

        Token = token;
        return true;
    }
}