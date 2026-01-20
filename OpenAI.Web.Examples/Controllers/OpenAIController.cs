using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.AI;
using OpenAI.Web.Examples.Models;
using System.Diagnostics;
using System.Net.NetworkInformation;

namespace OpenAI.Web.Examples.Controllers;

/// <summary>
/// 
/// </summary>
public sealed class OpenAIController : Controller
{
    /// <summary>
    /// 
    /// </summary>
    private readonly IChatClient _chatClient;


    private static List<ChatMessage> _historial = new();

    //Datos estáticos
    public static List<Transaccion> transacciones =
    [
        new Transaccion { Id = 1, Fecha = DateTime.Parse("12/01/2024"), ClienteId = 101, ProductoId = 1, Cantidad = 2, MontoTotal = 100, MetodoPago = "Tarjeta de crédito", Estado = "Completado" },
        new Transaccion { Id = 2, Fecha = DateTime.Parse("23/02/2024"), ClienteId = 102, ProductoId = 1, Cantidad = 3, MontoTotal = 150, MetodoPago = "PayPal", Estado = "Pendiente" },
        new Transaccion { Id = 3, Fecha = DateTime.Parse("11/09/2024"), ClienteId = 103, ProductoId = 1, Cantidad = 4, MontoTotal = 200, MetodoPago = "Efectivo", Estado = "Completado" },
        new Transaccion { Id = 4, Fecha = DateTime.Parse("10/12/2024"), ClienteId = 104, ProductoId = 2, Cantidad = 5, MontoTotal = 250, MetodoPago = "Transferencia bancaria", Estado = "Reembolsado" },
        new Transaccion { Id = 5, Fecha = DateTime.Parse("09/11/2024"), ClienteId = 105, ProductoId = 2, Cantidad = 6, MontoTotal = 300, MetodoPago = "Tarjeta de débito", Estado = "Completado" },
        new Transaccion { Id = 6, Fecha = DateTime.Parse("18/01/2024"), ClienteId = 106, ProductoId = 3, Cantidad = 6, MontoTotal = 300, MetodoPago = "PayPal", Estado = "Pendiente" },
        new Transaccion { Id = 7, Fecha = DateTime.Parse("05/03/2024"), ClienteId = 107, ProductoId = 3, Cantidad = 2, MontoTotal = 90, MetodoPago = "Criptomonedas", Estado = "Completado" },
        new Transaccion { Id = 8, Fecha = DateTime.Parse("22/06/2024"), ClienteId = 108, ProductoId = 4, Cantidad = 7, MontoTotal = 400, MetodoPago = "Efectivo", Estado = "Pendiente" },
        new Transaccion { Id = 9, Fecha = DateTime.Parse("30/07/2024"), ClienteId = 109, ProductoId = 5, Cantidad = 8, MontoTotal = 500, MetodoPago = "Tarjeta de crédito", Estado = "Completado" },
        new Transaccion { Id = 10, Fecha = DateTime.Parse("15/08/2024"), ClienteId = 110, ProductoId = 6, Cantidad = 1, MontoTotal = 50, MetodoPago = "Transferencia bancaria", Estado = "Reembolsado" }
    ];

    /// <summary>
    /// 
    /// </summary>
    public OpenAIController(IChatClient chatClient)
    {
        this._chatClient = chatClient;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public IActionResult Index()
    {
        return View();
    }

    /// <summary>
    /// Returns the default view for generating text.
    /// </summary>
    /// <returns>An <see cref="IActionResult"/> that renders the view associated with the text generation page.</returns>
    public IActionResult GenerarTexto()
    {
        return View();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="consulta"></param>
    /// <returns></returns>
    public async Task<IActionResult> ConsultarDatos(string consulta)
    {
        // Construimos un mensaje
        var prompt = $"el usuario preguntó: {consulta} \n\n aquí estan los datos de ventas: \n";

        foreach (var item in transacciones)
        {
            prompt += $"Id: {item.Id}, Fecha: {item.Fecha.ToShortDateString()}, ClienteId: {item.ClienteId}, " +
                $"ProductoId: {item.ProductoId}, Cantidad: {item.Cantidad}, MontoTotal: {item.MontoTotal}, MetodoPago: {item.MetodoPago}, Estado: {item.Estado} \n";
        }

        // Solicita a la IA un resultado en html y js
        prompt += "\n Responde en formato HTML con una tabla y un gráfico en JS para mostrar los resultados de la consulta.";

        var respuesta = await this._chatClient.GetResponseAsync(prompt);

        return View(nameof(GenerarTexto), respuesta);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="prompt"></param>
    /// <returns></returns>
    public async Task<IActionResult> GenerarChat()
    {
        return View();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    [HttpPost]
    public async Task<IActionResult> EnviarMensaje(string mensaje)
    {
        if (mensaje == null)
            return View("GenerarChat");

        _historial.Add(new ChatMessage(ChatRole.User, mensaje));

        // Corrección: Usar GetResponseAsync en lugar de CompleteAsync
        var respuestaIA = await _chatClient.GetResponseAsync(_historial);

        var respuestaAsistente = respuestaIA?.ToString() ?? "No se recibió respuesta de la IA";

        _historial.Add(new ChatMessage(ChatRole.Assistant, respuestaAsistente));

        ViewBag.Historial = _historial ?? new List<ChatMessage>();

        // Puedes procesar la respuesta y pasarla a la vista si es necesario
        return View("GenerarChat");
    }

    public IActionResult DeteccionObjectos()
    {
        return View();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
