using MinhasTarefasAPI.DataBase;
using MinhasTarefasAPI.V1.Models;
using MinhasTarefasAPI.V1.Repositories.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MinhasTarefasAPI.V1.Repositories
{
    public class TokenRepository : ITokenRepository
    {
        private readonly MinhasTarefasContext _banco;
        public TokenRepository(MinhasTarefasContext contexnto)
        {
            _banco = contexnto; 
        }
        public void Atualizar(Token token)
        {
            _banco.Token.Update(token);
            _banco.SaveChanges();
        }

        public void Cadastrar(Token token)
        {
            _banco.Token.Add(token);
            _banco.SaveChanges();
        }

        public Token Obter(string refreshtoken)
        {
            return _banco.Token.FirstOrDefault(a => a.RefreshToken == refreshtoken && a.Utilizado==false);
        }
    }
}
