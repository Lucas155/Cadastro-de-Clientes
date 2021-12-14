using CadastroDeClientes.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace CadastroDeClientes.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        
        private static List<Clientes> _listaClientes = new List<Clientes>()
        {
            new Clientes() { Id=1, Nome="admin", CPF= "123456", Email="admin@gmail.com", DataNascimento = "1994-11-15", End = "Rua rust", Login = "admin", Senha = "admin"},
        };

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult CadastroClientes(Clientes clientes)
        {
            if (ModelState.IsValid)
            {
                return RedirectToAction("SalvaCliente", "Home", clientes);
            }

            return View();

        }

        public IActionResult Login()
        {
            return View();
        }

        public IActionResult LoginValida(Clientes clientes)
        {
            var vLogin = _listaClientes.Where(p => p.Login.Equals(clientes.Login)).FirstOrDefault();

            if (vLogin != null && clientes.Senha == vLogin.Senha)
            {
                return RedirectToAction("Clientes", "Home");
            }
            else
            {
                ModelState.AddModelError("Aviso", "Login ou senha invalidos");
                return View("Login");
            }

        }

        public IActionResult Clientes()
        {
            return View(_listaClientes);
        }

        public IActionResult SalvaCliente(Clientes clientes)
        {
            var resultado = "OK";
            var idSalvo = string.Empty;
            if (!ModelState.IsValid)
            {
                resultado = "AVISO";
            }
            else
            {
                try
                {
                    var registroBD = _listaClientes.Find(x => x.Id == clientes.Id);
                    if (registroBD == null)
                    {
                        registroBD = clientes;
                        registroBD.Id = _listaClientes.Max(x => x.Id) + 1;
                        _listaClientes.Add(registroBD);
                    }
                    else
                    {
                        registroBD.Nome = clientes.Nome;
                        registroBD.CPF = clientes.CPF;
                        registroBD.Email = clientes.Email;
                        registroBD.DataNascimento = clientes.DataNascimento;
                        registroBD.End = clientes.End;
                        registroBD.Login = clientes.Login;
                        registroBD.Senha = clientes.Senha;
                    }

                    idSalvo = registroBD.Id.ToString();
                }
                catch (Exception ex)
                {
                    resultado = "ERRO";

                }
            }

            return RedirectToAction("Clientes", "Home");
        }
        public ActionResult RecuperarProduto(int id)
        {
            return Json(_listaClientes.Find(x => x.Id == id));
        }
        public ActionResult ExcluirCliente(int id)
        {
            var ret = false;
            var registroBD = _listaClientes.Find(x => x.Id == id);
            if (registroBD != null)
            {
                _listaClientes.Remove(registroBD);
                ret = true;
            }
            return Json(ret);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
