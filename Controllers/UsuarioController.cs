using Microsoft.AspNetCore.Mvc;
using Biblioteca.Models;
using Microsoft.AspNetCore.Http;


namespace Biblioteca.Controllers
{
    public class UsuariosController : Controller
    {
        //inserção
         public IActionResult RegistrarUsuarios()
        {
            Autenticacao.CheckLogin(this);
            Autenticacao.verificaSeUsuarioEAdmin(this);
            return View();

        }

        [HttpPost]
        public IActionResult RegistrarUsuarios(Usuario novoUsuario)
        {
            novoUsuario.Senha = Criptografo.TextoCriptografado(novoUsuario.Senha);
            new UsuarioService().incluirUsuario(novoUsuario);
            return RedirectToAction("CadastroRealizado");
        }

        //listagem
        public IActionResult listaDeUsuarios()
        {
            Autenticacao.CheckLogin(this);
            Autenticacao.verificaSeUsuarioEAdmin(this);

            return View(new UsuarioService().Listar());
        }

        //edição
        public IActionResult EditarUsuario(int id)
        {
            Usuario u = new UsuarioService().Listar(id);
            return View(u);
        }

        [HttpPost]
        public IActionResult EditarUsuario(Usuario usuarioEditado)
        {
            // new UsuarioService().editarUsuario(userEditado);
            // UsuarioService us = new UsuarioService();
            new UsuarioService().editarUsuario(usuarioEditado);
            // us.editarUsuario(usuarioEditado);
            
            return RedirectToAction("ListaDeUsuarios");
        }

        //exclusão
        public IActionResult excluirUsuario(int id)
        {
            return View(new UsuarioService().Listar(id));
        }

        [HttpPost]
        public IActionResult excluirUsuario(string decisao, int id)
        {
            if (decisao == "Excluir")
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

        //funções extras
        public IActionResult Sair()
        {
            HttpContext.Session.Clear();
            return RedirectToAction ("Index", "Home");
        }

        public IActionResult RequiresAdmin()
        {
            Autenticacao.CheckLogin(this);
            return View();
        }

        public IActionResult CadastroRealizado()
        {
            Autenticacao.CheckLogin(this);
            Autenticacao.verificaSeUsuarioEAdmin(this);

            return View();
        }
    }
}