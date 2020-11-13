using APS_02.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace APS_02.DAOs
{
    public abstract class BaseDAO<T> : IDAO<T> where T : Model
    {
        public virtual void Atualizar(T obj)
        {
            ExecutarComando(GetComandoCompleto(GetSqlUpdate(), obj));
        }

        public virtual void Inserir(T obj)
        {
            if (obj.Id == null)
                // Tem que tratar esse erro um dia...
                return;

            ExecutarComando(GetComandoCompleto(GetSqlInsert(), obj));
        }

        public virtual void Remover(T obj)
        {
            ExecutarComando(GetComandoId(GetSqlDelete(), Convert.ToInt32(obj.Id)));
        }
        
        public virtual void RemoverPorId(int id)
        {
            ExecutarComando(GetComandoId(GetSqlDelete(), id)); 
        }

        public virtual T RetornarPorId(int id)
        {
            var cmd = new SqlCommand(GetSqlSelectId());
            var tabela = GetDataTable(cmd);

            if (tabela.Rows.Count > 0)
                return GetObjeto(tabela.Rows[0]);

            return null;
        }

        public virtual List<T> RetornarTodos()
        {
            var lista = new List<T>();

            var cmd = new SqlCommand(GetSqlSelect());
            var tabela = GetDataTable(cmd);

            foreach (DataRow reg in tabela.Rows)
                lista.Add(GetObjeto(reg));

            return lista;
        }

        protected abstract T GetObjeto(DataRow reg);
        protected abstract string GetSqlUpdate();
        protected abstract string GetSqlInsert();
        protected abstract string GetSqlDelete();
        protected abstract string GetSqlSelect();
        protected abstract string GetSqlSelectId();
        protected abstract void AdicionarParametros(SqlCommand cmd, T obj);

        protected void ExecutarComando(SqlCommand cmd)
        {
            using (var conexao = new SqlConnection(GetStringConexao()))
            {
                conexao.Open();

                cmd.Connection = conexao;

                cmd.ExecuteNonQuery();

                conexao.Close();
            }
        }

        protected void ExecutarComandos(IEnumerable<SqlCommand> comandos)
        {
            using (var conexao = new SqlConnection(GetStringConexao()))
            {
                conexao.Open();

                var transacao = conexao.BeginTransaction();

                try
                {
                    foreach (var cmd in comandos)
                    {
                        cmd.Transaction = transacao;
                        cmd.Connection = conexao;
                        cmd.ExecuteNonQuery();
                    }

                    transacao.Commit();
                }
                catch
                {
                    transacao.Rollback();
                    conexao.Close();
                    throw;
                }

                conexao.Close();
            }
        }

        protected SqlCommand GetComandoId(string sql, int codigo)
        {
            var cmd = new SqlCommand(sql);

            cmd.Parameters.AddWithValue("@CODIGO", codigo);

            return cmd;
        }

        private DataTable GetDataTable(SqlCommand cmd)
        {
            using (var conexao = new SqlConnection(GetStringConexao()))
            {
                conexao.Open();

                cmd.Connection = conexao;

                var dt = new DataTable();
                var da = new SqlDataAdapter(cmd);

                da.Fill(dt);

                conexao.Close();

                return dt;
            }
        }

        private SqlCommand GetComandoCompleto(string sql, T obj)
        {
            var cmd = GetComandoId(sql, Convert.ToInt32(obj.Id));

            AdicionarParametros(cmd, obj);

            return cmd;
        }

        private static string GetStringConexao()
        {
            return @"Integrated Security=SSPI;Persist Security Info=False;" +
                   @"Initial Catalog=Aps02;Data Source=localhost\SQLEXPRESS";
        }
    }
}