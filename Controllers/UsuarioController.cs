using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Biblioteca.Models;
using System.Linq;
using System.Collections.Generic;


namespace Biblioteca.Controllers
{
    public class UsuarioController : Controller
    {
        // Listagem

        public IActionResult ListaDeUsuarios()
        {
            Autenticacao.CheckLogin(this);
            Autenticacao.VerificaSeUsuarioEAdmin(this);

            return View(new UsuarioService().Listar());

        }

        // Inserção

        public IActionResult RegistrarUsuario()
        {
            Autenticacao.CheckLogin(this);
            Autenticacao.VerificaSeUsuarioEAdmin(this);

            return View();

        }

        [HttpPost]
        public IActionResult RegistrarUsuario(Usuario novoUsuario)
        {

            novoUsuario.Senha = Criptografo.TextoCriptografado(novoUsuario.Senha);
            
            UsuarioService us = new UsuarioService();
            us.incluirUsuario(novoUsuario);

            return RedirectToAction("CadastroRealizado");
            
            // novoUsuario.Senha = Criptografo.TextoCriptografado(novoUsuario.Senha);
            // new UsuarioService().incluirUsuario(novoUsuario);
            // return RedirectToAction("CadastroRealizado");
        }

        public IActionResult CadastroRealizado()
        {
            return View();
        }


        //Edição

        public IActionResult EditarUsuario(int id)
        {
            Usuario u = new UsuarioService().Listar(id);
            return View(u);
        }

        [HttpPost]
        public IActionResult EditarUsuario(Usuario usuarioEditado)
        {
            new UsuarioService().editarUsuario(usuarioEditado);
            return RedirectToAction("ListaDeUsuarios");
        }


        // Exclusão

        public IActionResult ExcluirUsuario(int id)
        {
            return View(new UsuarioService().Listar(id));
        }

        [HttpPost]
        public IActionResult ExcluirUsuario(string decisao, int id)
        {
            if(decisao == "Excluir")
            {
                ViewData["Mensagem"] = "Exclusão do usuário " + new UsuarioService().Listar(id).Nome + " realizada com sucesso";
                new UsuarioService().excluirUsuario(id);
                return View("ListaDeUsuarios", new UsuarioService().Listar());
            }
            else
            {
                ViewData["Mensagem"] = "Exclusão Cancelada";
                return View("ListadeUsuarios", new UsuarioService().Listar());

            }
        }

        // Funções Extras
        public IActionResult Sair()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }

        public IActionResult RequiresAdmin()
        {
            Autenticacao.CheckLogin(this);
            return View();
        }

    }
}