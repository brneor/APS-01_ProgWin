using APS_02.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace APS_02.DAOs
{
    public class ProdutoDAO : BaseDAO<Produto>, IDAO<Produto>
    {
        protected override string GetSqlDelete() =>
            "DELETE FROM PRODUTOS WHERE CODIGO=@CODIGO";

        protected override string GetSqlInsert() =>
            "INSERT INTO PRODUTOS (CODIGO, DESCRICAO, ESTOQUE) VALUES (@CODIDO, @DESCRICAO, @ESTOQUE)";

        protected override string GetSqlSelect() =>
            "SELECT * FROM PRODUTOS ORDER BY CODIGO";

        protected override string GetSqlSelectId() =>
            "SELECT * FROM PRODUTOS WHERE CODIGO=@CODIGO";

        protected override string GetSqlUpdate() =>
            "UPDATE PRODUTOS SET DESCRICAO=@DESCRICAO, ESTOQUE=@ESTOQUE WHERE CODIGO=@CODIGO";

        protected override void AdicionarParametros(SqlCommand cmd, Produto obj)
        {
            cmd.Parameters.AddWithValue("@DESCRICAO", obj.Descricao);
        }

        protected override Produto GetObjeto(DataRow reg)
        {
            var obj = new Produto();

            obj.Id = Convert.ToInt32(reg["ID"]);
            obj.Descricao = reg["DESCRICAO"].ToString();
            obj.Preco = Convert.ToDouble(reg["Preco"]);

            return obj;
        }
    }
}