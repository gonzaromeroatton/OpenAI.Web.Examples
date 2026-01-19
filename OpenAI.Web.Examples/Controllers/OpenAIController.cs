using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using OpenAI.Web.Examples.Models;

namespace OpenAI.Web.Examples.Controllers;

/// <summary>
/// 
/// </summary>
public sealed class OpenAIController : Controller
{
    /// <summary>
    /// 
    /// </summary>
    public OpenAIController() { }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public IActionResult Index()
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
