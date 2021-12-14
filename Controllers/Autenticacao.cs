using System.Collections.Generic;
using System.Linq;
using Biblioteca.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;


namespace Biblioteca.Controllers
{
    public class Autenticacao
    {
        public static void CheckLogin(Controller controller)
        {   
            if(string.IsNullOrEmpty(controller.HttpContext.Session.GetString("Login")))
            {
                controller.Request.HttpContext.Response.Redirect("/Home/Login");
            }
        }

        public static bool verificaLoginSenha(string Login, string SenhaUsuario, Controller controller)
        {
            using(BibliotecaContext bc = new BibliotecaContext())
            {
                VerificaSeUsuarioAdminExiste(bc);
                SenhaUsuario = Criptografo.TextoCriptografado(SenhaUsuario);

                IQueryable<Usuario> UsuarioEncontrado = bc.Usuarios.Where(u => u.Login==Login && u.Senha==SenhaUsuario);
                List<Usuario>ListaUsuarioEncontrado = UsuarioEncontrado.ToList();

                if(ListaUsuarioEncontrado.Count == 0) {
                    return false;
                } else {
                    controller.HttpContext.Session.SetString("Login", ListaUsuarioEncontrado[0].Login);
                    controller.HttpContext.Session.SetString("Nome", ListaUsuarioEncontrado[0].Nome);
                    controller.HttpContext.Session.SetInt32("Tipo", ListaUsuarioEncontrado[0].Tipo);
                    return true;
                }
            }
        }
        public static void VerificaSeUsuarioAdminExiste(BibliotecaContext bc) {
            IQueryable<Usuario> usuarioEncontrado = bc.Usuarios.Where(u => u.Login=="admin");


            if (usuarioEncontrado.ToList().Count == 0) {
                Usuario admin = new Usuario();
                admin.Login = "admin";
                admin.Senha = Criptografo.TextoCriptografado("123");
                admin.Tipo = Usuario.admin;
                admin.Nome = "Administrador";

                bc.Usuarios.Add(admin);
                bc.SaveChanges();
            }
        }

        public static void VerificaSeUsuarioEAdmin(Controller controller)
        {
           if(!(controller.HttpContext.Session.GetInt32("Tipo")==Usuario.admin))
           {
               controller.Request.HttpContext.Response.Redirect("/Usuario/RequiresAdmin");
           } 
        }
    }
}
