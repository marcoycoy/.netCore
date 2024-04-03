using DI_Service_Lifetime.Models;
using DI_Service_Lifetime.Services;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Text;

namespace DI_Service_Lifetime.Controllers
{
    public class HomeController : Controller
    {
        private readonly IScopedService _scope1;
        private readonly IScopedService _scope2;

        private readonly ITransientService _transient1;
        private readonly ITransientService _transient2;

        private readonly ISingletonService _singleton1;
        private readonly ISingletonService _singleton2;

        public HomeController(ISingletonService singleton1, ISingletonService singleton2,
                              ITransientService transient1, ITransientService transient2,
                              IScopedService scope1, IScopedService scope2)
        {
            _scope1 = scope1;
            _scope2 = scope2;

            _transient1 = transient1;
            _transient2 = transient2;

            _singleton1 = singleton1;
            _singleton2 = singleton2;


        }

        public IActionResult Index()
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append($"Singleton Service1: {_singleton1.GetGuid() }\n");
            stringBuilder.Append($"Singleton Service2: {_singleton2.GetGuid() }\n\n\n");

            stringBuilder.Append($"Scope Service1: {_scope1.GetGuid() }\n");
            stringBuilder.Append($"Scope Service2: {_scope1.GetGuid() }\n\n\n");

            stringBuilder.Append($"Transient Service1: {_transient1.GetGuid() }\n");
            stringBuilder.Append($"Transient Service2: {_transient2.GetGuid() }\n\n\n");


            return Ok(stringBuilder.ToString());
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
