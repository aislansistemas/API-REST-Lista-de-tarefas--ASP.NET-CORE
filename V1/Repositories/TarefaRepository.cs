using MinhasTarefasAPI.DataBase;
using MinhasTarefasAPI.V1.Models;
using MinhasTarefasAPI.V1.Repositories.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MinhasTarefasAPI.V1.Repositories
{
    public class TarefaRepository : ITarefaRepository
    {
        private readonly MinhasTarefasContext _banco;
        public TarefaRepository(MinhasTarefasContext banco)
        {
            _banco = banco;
        }

        //retorna uma lista de de registros 
        public List<Tarefa> Restauracao(ApplicationUser usuario, DateTime dateUltimaSincronizacao)
        {
            var query = _banco.Tarefas.Where(a=>a.UsuarioId==usuario.Id).AsQueryable();
            if (dateUltimaSincronizacao != null)
            {
                query.Where(a => a.Criado >= dateUltimaSincronizacao || a.Atualizado >= dateUltimaSincronizacao);
            }
            return query.ToList<Tarefa>();
        }

        //cadastra novos registro no banco
        public List<Tarefa> Sincronizacao(List<Tarefa> tarefas)
        {
            var tarefasNovas = tarefas.Where(a => a.IdTarefaApi == 0).ToList();
            var tarefasExcluidasAtualizadas = tarefas.Where(a => a.IdTarefaApi != 0).ToList();

            // Cadastrar novos registros
            if (tarefasNovas.Count() > 0)
            {
                foreach (var tarefa in tarefasNovas)
                {
                    _banco.Tarefas.Add(tarefa);
                }
            }


            // Atualização de registro (Excluido)
            if (tarefasExcluidasAtualizadas.Count() > 0)
            {
                foreach (var tarefa in tarefasExcluidasAtualizadas)
                {
                    _banco.Tarefas.Update(tarefa);
                }
            }

            _banco.SaveChanges();

            return tarefasNovas.ToList();
        }
    }
}
