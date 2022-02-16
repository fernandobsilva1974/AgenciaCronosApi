using AgenciaCronosApi.Context;
using AgenciaCronosApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AgenciaCronosApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostsController : ControllerBase
    {
        private readonly DataContext _context;

        public PostsController(DataContext context)
        {
            _context = context;
        }

        //LISTAGEM COMPLETA DOS POSTS
        [HttpGet]
        [Route("getposts")]
        public IEnumerable<Posts> GetPosts()
        {
            var listservices = new List<Posts>();
            listservices.Clear();

            var servlinq = (from a in _context.Posts
                            where a.Excluido == false
                            select a).OrderBy(c => c.Titulo).ToList();
            foreach (var add in servlinq)
            {
                listservices.Add(add);
            }

            return listservices;
        }

        //CRIAÇÃO DE UM NOVO POST
        [HttpPost("createpost")]
        public async Task<ActionResult<Posts>> CreatePost(Posts model)
        {
            try
            {
                Posts novoservico = new Posts();
                novoservico.Descritivo = model.Descritivo;
                novoservico.Titulo = model.Titulo;
                novoservico.DataCriacao = System.DateTime.Now;

                int numero = 1;
                var achaultimo = (from a in _context.Posts
                                  where a.Excluido == false
                                  select a.Numero).Max();
                if (achaultimo != null)
                {
                    numero = Convert.ToInt32(achaultimo) + 1;
                }
                novoservico.Numero = numero;
                await _context.AddAsync(novoservico);
                await _context.SaveChangesAsync();

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //EDITAR POST
        [HttpPost("posteditposts")]
        public async Task<ActionResult<Posts>> PostEditPosts(Posts model)
        {
            try
            {
                var achaservico = (from a in _context.Posts
                                   where a.Id == model.Id
                                   select a).FirstOrDefault();
                if (achaservico != null)
                {
                    achaservico.Descritivo = model.Descritivo;
                    achaservico.Titulo = model.Titulo;

                    await _context.SaveChangesAsync();
                }

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //DELETAR POST
        [HttpPost("postdeleteposts")]
        public async Task<ActionResult<Posts>> PostDeletePosts(Posts model)
        {
            try
            {
                var achaservico = (from a in _context.Posts
                                   where a.Id == model.Id
                                   select a).FirstOrDefault();
                if (achaservico != null)
                {
                    // DELETAR A PARTIR DO CAMPO EXCLUIDO PARA NÃO EXCLUIR DEFINITIVAMENTE DO BANCO DE DADOS
                    achaservico.Excluido = true;

                    await _context.SaveChangesAsync();
                }

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
