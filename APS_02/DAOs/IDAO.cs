using APS_02.Models;
using System.Collections.Generic;

namespace APS_02.DAOs
{
    public interface IDAO<T> where T : Model
    {
        void Inserir(T obj);

        void Remover(T obj);

        void Atualizar(T obj);

        T RetornarPorId(int id);

        List<T> RetornarTodos();
    }
}