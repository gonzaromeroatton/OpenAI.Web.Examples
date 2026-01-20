namespace OpenAI.Web.Examples.Models;

public sealed class Transaccion
{
    public Transaccion() { }

    public int Id { get; set; }
    public DateTime Fecha { get; set; }
    public int ClienteId { get; set; }
    public int ProductoId { get; set; }
    public int Cantidad { get; set; }
    public int MontoTotal { get; set; }
    public string MetodoPago { get; set; } = string.Empty;
    public string Estado { get; set; } = string.Empty;
}
