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
    public class IntegrantesController : ControllerBase
    {
        private readonly DataContext _context;

        public IntegrantesController(DataContext context)
        {
            _context = context;
        }

        //LISTAGEM COMPLETA DOS INTEGRANTES
        [HttpGet]
        [Route("getintegrantes")]
        public IEnumerable<Integrantes> GetIntegrantes()
        {
            var listservices = new List<Integrantes>();
            listservices.Clear();

            var servlinq = (from a in _context.Integrantes
                            where a.Excluido == false
                            select a).OrderBy(c => c.Nome).ToList();
            foreach (var add in servlinq)
            {
                listservices.Add(add);
            }

            return listservices;
        }

        //CRIAÇÃO DE UM NOVO INTEGRANTE
        [HttpPost("postintegrantes")]
        public async Task<ActionResult<Integrantes>> PostIntegrantes(Integrantes model)
        {
            try
            {
                Integrantes novoservico = new Integrantes();
                novoservico.Celular = model.Celular;
                novoservico.Cpf = model.Cpf;
                novoservico.Email = model.Email;
                novoservico.Nome = model.Nome;
                novoservico.DataCriacao = System.DateTime.Now;

                await _context.AddAsync(novoservico);
                await _context.SaveChangesAsync();

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //EDITAR INTEGRANTE
        [HttpPost("posteditintegrantes")]
        public async Task<ActionResult<Integrantes>> PostEditServicos(Integrantes model)
        {
            try
            {
                var achaservico = (from a in _context.Integrantes
                                   where a.Id == model.Id
                                   select a).FirstOrDefault();
                if (achaservico != null)
                {
                    achaservico.Celular = model.Celular;
                    achaservico.Cpf = model.Cpf;
                    achaservico.Email = model.Email;
                    achaservico.Nome = model.Nome;

                    await _context.SaveChangesAsync();
                }

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //DELETAR INTEGRANTE
        [HttpPost("postdeleteintegrantes")]
        public async Task<ActionResult<Integrantes>> PostDeleteIntegrantes(Servicos model)
        {
            try
            {
                var achaservico = (from a in _context.Integrantes
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
