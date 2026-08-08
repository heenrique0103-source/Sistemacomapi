using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/", async () =>
{
    using HttpClient cliente = new HttpClient();

    string url =
        "https://brasilapi.com.br/api/fipe/marcas/v1/carros";

    string respostaJson =
        await cliente.GetStringAsync(url);

    var opcoes = new JsonSerializerOptions
    {
        PropertyNameCaseInsensitive = true
    };

    List<Marca>? marcas =
        JsonSerializer.Deserialize<List<Marca>>(
            respostaJson,
            opcoes
        );

    string linhasTabela = "";

    if (marcas != null)
    {
        foreach (Marca marca in marcas)
        {
            linhasTabela += $"""
                <tr>
                    <td>{marca.Valor}</td>
                    <td>{marca.Nome}</td>
                </tr>
            """;
        }
    }

    string pagina = $$"""
    <!DOCTYPE html>
    <html lang="pt-BR">
    <head>
        <meta charset="UTF-8">
        <title>Consulta FIPE</title>

        <style>
            body {
                font-family: Arial, sans-serif;
                background-color: #f4f4f4;
                margin: 40px;
            }

            .container {
                max-width: 800px;
                margin: auto;
                background-color: white;
                padding: 30px;
                border-radius: 10px;
            }

            h1 {
                color: #0066cc;
            }

            table {
                width: 100%;
                border-collapse: collapse;
            }

            th, td {
                padding: 12px;
                border: 1px solid #dddddd;
                text-align: left;
            }

            th {
                background-color: #0066cc;
                color: white;
            }

            tr:nth-child(even) {
                background-color: #f2f2f2;
            }
        </style>
    </head>

    <body>
        <div class="container">
            <h1>Marcas de carros</h1>

            <p>Dados obtidos por meio da API FIPE da BrasilAPI.</p>

            <table>
                <thead>
                    <tr>
                        <th>Código</th>
                        <th>Marca</th>
                    </tr>
                </thead>

                <tbody>
                    {{linhasTabela}}
                </tbody>
            </table>
        </div>
    </body>
    </html>
""";

    return Results.Content(
        pagina,
        "text/html; charset=utf-8"
    );
});

app.Run();

public class Marca
{
    public string Nome { get; set; } = "";
    public string Valor { get; set; } = "";
}





