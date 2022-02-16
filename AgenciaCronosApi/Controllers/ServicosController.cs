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
    public class ServicosController : ControllerBase
    {
        private readonly DataContext _context;

        public ServicosController(DataContext context)
        {
            _context = context;
        }

        //LISTAGEM COMPLETA DOS SERVIÇOS
        [HttpGet]
        [Route("getservicos")]
        public IEnumerable<Servicos> GetServicos()
        {
            var listservices = new List<Servicos>();
            listservices.Clear();

            var servlinq = (from a in _context.Servicos
                         where a.Excluido == false
                         select a).OrderBy(c => c.CodServico).ToList();
            foreach (var add in servlinq)
            {
                listservices.Add(add);
            }

            return listservices;
        }

        //CRIAÇÃO DE UM NOVO SERVIÇO
        [HttpPost("postservicos")]
        public async Task<ActionResult<Servicos>> PostServicos(Servicos model)
        {
            try
            {
                Servicos novoservico = new Servicos();
                novoservico.CodServico = model.CodServico;
                novoservico.DescServico = model.DescServico;
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

        //EDITAR SERVIÇO
        [HttpPost("posteditservicos")]
        public async Task<ActionResult<Servicos>> PostEditServicos(Servicos model)
        {
            try
            {
                var achaservico = (from a in _context.Servicos
                                   where a.Id == model.Id
                                   select a).FirstOrDefault();
                if (achaservico != null)
                {
                    achaservico.CodServico = model.CodServico;
                    achaservico.DescServico = model.DescServico;

                    await _context.SaveChangesAsync();
                }

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //DELETAR SERVIÇO
        [HttpPost("postdeleteservicos")]
        public async Task<ActionResult<Servicos>> PostDeleteServicos(Servicos model)
        {
            try
            {
                var achaservico = (from a in _context.Servicos
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
