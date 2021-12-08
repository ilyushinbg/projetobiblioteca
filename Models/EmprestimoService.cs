using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System;


namespace Biblioteca.Models
{
    public class EmprestimoService 
    {
        public void Inserir(Emprestimo e)
        {
            using(BibliotecaContext bc = new BibliotecaContext())
            {
                bc.Emprestimos.Add(e);
                bc.SaveChanges();
            }
        }

        public void Atualizar(Emprestimo e)
        {
            using(BibliotecaContext bc = new BibliotecaContext())
            {
                Emprestimo emprestimo = bc.Emprestimos.Find(e.Id);
                emprestimo.NomeUsuario = e.NomeUsuario;
                emprestimo.Telefone = e.Telefone;
                emprestimo.LivroId = e.LivroId;
                emprestimo.DataEmprestimo = e.DataEmprestimo;
                emprestimo.DataDevolucao = e.DataDevolucao;

                bc.SaveChanges();
            }
        }

        public ICollection<Emprestimo> ListarTodos(FiltrosEmprestimos filtro = null)
        {
            using(BibliotecaContext bc = new BibliotecaContext())
            {
                IQueryable<Emprestimo> query;

                if(filtro != null)
                {
                    switch(filtro.TipoFiltro)
                    {
                        case "Usuario":
                            query = bc.Emprestimos.Where(e => e.NomeUsuario.Contains(filtro.Filtro));
                        break;

                        case "Livro":
                            List<Livro> LivrosFiltro = bc.Livros.Where(l => l.Titulo.Contains(filtro.Filtro)).ToList();
                            
                            List<int> LivrosId = new List<int>();
                            for (int n = 0; n < LivrosFiltro.Count; n++) {
                                LivrosId.Add(LivrosFiltro[n].Id);
                            }
                            query = bc.Emprestimos.Where(e => LivrosId.Contains(e.LivroId));
                            var debug = query.ToList();
                        break;

                        default:
                            query = bc.Emprestimos;
                        break;
                    }
                }
                else
                {
                    query = bc.Emprestimos;
                }
                List<Emprestimo> ListaConsulta = query.OrderByDescending(e => e.DataEmprestimo).ToList();

                for (int n = 0; n < ListaConsulta.Count; n++) {
                    ListaConsulta[n].Livro = bc.Livros.Find(ListaConsulta[n].LivroId);
                }

                return ListaConsulta;
            }
        }

        public Emprestimo ObterPorId(int id)
        {
            using(BibliotecaContext bc = new BibliotecaContext())
            {
                return bc.Emprestimos.Find(id);
            }
        }
    }
}