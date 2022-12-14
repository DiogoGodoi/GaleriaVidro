using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using EstoqueModel;
using Conexao;

namespace EstoqueControle
{
    public class ctrlEstoque
    {
        private int qtd; 
        private string item { get; set; }
        public bool CadastrarInsumo(mdlEstoque _mdlEstoque)
        {
            Conexao.ConexaoDB.conectar();
            var abrirCONN = ConexaoDB.conectar();

            try
            {
                abrirCONN.Open();

                string query = "INSERT INTO Estoque (codigo, nome, unidade, quantidade, estoqueMin) VALUES (@codigo, @nome, @unidade, @quantidade, @estoqueMin)";
                SqlCommand comando = new SqlCommand(query, abrirCONN);

                var pmtCodigo = comando.CreateParameter();
                pmtCodigo.ParameterName = "@codigo";
                pmtCodigo.DbType = DbType.Int32;
                pmtCodigo.Value = _mdlEstoque.codigo;
                comando.Parameters.Add(pmtCodigo);

                var pmtNome = comando.CreateParameter();
                pmtNome.ParameterName = "@nome";
                pmtNome.DbType = DbType.String;
                pmtNome.Value = _mdlEstoque.nome;
                comando.Parameters.Add(pmtNome);
                
                var pmtUnidade = comando.CreateParameter();
                pmtUnidade.ParameterName = "@unidade";
                pmtUnidade.DbType = DbType.String;
                pmtUnidade.Value = _mdlEstoque.unidade;
                comando.Parameters.Add(pmtUnidade);

                var pmtQuantidade = comando.CreateParameter();
                pmtQuantidade.ParameterName = "@quantidade";
                pmtQuantidade.DbType = DbType.Int32;
                pmtQuantidade.Value = _mdlEstoque.quantidade;
                comando.Parameters.Add(pmtQuantidade);

                var pmtQuantidadeMin = comando.CreateParameter();
                pmtQuantidadeMin.ParameterName = "@estoqueMin";
                pmtQuantidadeMin.DbType = DbType.Int32;
                pmtQuantidadeMin.Value = _mdlEstoque.qtdMinima;
                comando.Parameters.Add(pmtQuantidadeMin);

                var leitura = comando.ExecuteReader();

                if(leitura.Read() == false)
                {
                    abrirCONN.Close();
                    return true;
                }else
                {
                    abrirCONN.Close();
                    return false;
                }
            }
            catch(Exception ex)
            {
                abrirCONN.Close();
                return false;
                throw new Exception("Erro no cadastro: " + ex.Message);
            }
            finally
            {
                abrirCONN.Close();
            }

        }
        public DataTable ExibirZerado()
        {

            Conexao.ConexaoDB.conectar();
            var abrirConn = Conexao.ConexaoDB.conectar();

            try
            {
                abrirConn.Open();
                string query = "SELECT codigo, nome, unidade, quantidade, estoqueMin FROM Estoque WHERE quantidade=0 ORDER BY nome ASC";
                SqlCommand comando = new SqlCommand(query, abrirConn);

                comando.CommandType = CommandType.Text;
                SqlDataAdapter adaptador = new SqlDataAdapter(comando);
                DataTable tabela = new DataTable();
                adaptador.Fill(tabela);

                return tabela;

            }
            catch (Exception ex)
            {
                abrirConn.Close();
                return null;
                throw new Exception("Erro interno ao acesso a base " + ex.Message);
            }
            finally
            {
                abrirConn.Close();
            }

        }
        public DataTable Pesquisar(string nome)
        {

            Conexao.ConexaoDB.conectar();
            var abrirConn = Conexao.ConexaoDB.conectar();
            try
            {
                abrirConn.Open();
                string query = "SELECT nome, unidade, quantidade, codigo, estoqueMin FROM Estoque WHERE nome LIKE @nome";
                SqlCommand comando = new SqlCommand(query, abrirConn);

                comando.Parameters.AddWithValue("@nome", nome + "%");

                comando.CommandType = CommandType.Text;
                SqlDataAdapter adaptador = new SqlDataAdapter(comando);
                DataTable tabela = new DataTable();
                adaptador.Fill(tabela);

                var leitura = comando.ExecuteReader();

                if(leitura.Read() == true)
                {
                    qtd = Convert.ToInt32(leitura["quantidade"]);
                    return tabela;
                }else
                {
                    return null;
                }
                
                
              
            }
            catch (Exception ex)
            {
                abrirConn.Close();
                return null;
                throw new Exception("Erro ao acesso a base " + ex.Message);
            }
            finally
            {
                abrirConn.Close();
            }
        }
        public DataTable PesquisarPorCodigo(int codigo)
        {

            Conexao.ConexaoDB.conectar();
            var abrirConn = Conexao.ConexaoDB.conectar();
            try
            {
                abrirConn.Open();
                string query = "SELECT nome, unidade, quantidade, codigo, estoqueMin FROM Estoque WHERE codigo LIKE @codigo";
                SqlCommand comando = new SqlCommand(query, abrirConn);

                comando.Parameters.AddWithValue("@codigo", codigo + "%");

                comando.CommandType = CommandType.Text;
                SqlDataAdapter adaptador = new SqlDataAdapter(comando);
                DataTable tabela = new DataTable();
                adaptador.Fill(tabela);

                var leitura = comando.ExecuteReader();

                if (leitura.Read() == true)
                {
                    item = leitura["nome"].ToString();
                    return tabela;
                }
                else
                {
                    return null;
                }

            }
            catch (Exception ex)
            {
                abrirConn.Close();
                return null;
                throw new Exception("Erro ao acesso a base " + ex.Message);
            }
            finally
            {
                abrirConn.Close();
            }
        }
        public DataTable Entrada(string codigo, string quantidade)
        {
            Conexao.ConexaoDB.conectar();
            var abrirConn = Conexao.ConexaoDB.conectar();
            try
            {
                abrirConn.Open();
                string query =  "UPDATE estoque SET quantidade = quantidade + @quantidade WHERE nome=@codigo";
                SqlCommand comando = new SqlCommand(query, abrirConn);

                comando.Parameters.AddWithValue("@codigo", codigo);
                comando.Parameters.AddWithValue("@quantidade", quantidade);

                comando.CommandType = CommandType.Text;
                SqlDataAdapter adaptador = new SqlDataAdapter(comando);
                DataTable tabela = new DataTable();
                adaptador.Fill(tabela);

                return tabela;
                
            }
            catch (Exception ex)
            {
                abrirConn.Close();
                return null;
                throw new Exception("Erro ao acesso a base " + ex.Message);
            }
            finally
            {
                abrirConn.Close();
            }
        }
        public DataTable Saida(string nome, string quantidade)
        {
            Conexao.ConexaoDB.conectar();
            var abrirConn = Conexao.ConexaoDB.conectar();

            try
            {
                abrirConn.Open();
                string query = "UPDATE estoque SET quantidade = quantidade-@quantidade WHERE nome=@nome";
                SqlCommand comando = new SqlCommand(query, abrirConn);

                comando.Parameters.AddWithValue("@nome", nome);
                comando.Parameters.AddWithValue("@quantidade", quantidade);

                comando.CommandType = CommandType.Text;
                SqlDataAdapter adaptador = new SqlDataAdapter(comando);
                DataTable tabela = new DataTable();
                adaptador.Fill(tabela);

                return tabela;


            }
            catch (Exception ex)
            {
                abrirConn.Close();
                return null;
                throw new Exception("Erro ao acesso a base " + ex.Message);
            }
            finally
            {
                abrirConn.Close();
            }
        }
        public DataTable Exibir()
        {

            Conexao.ConexaoDB.conectar();
            var abrirConn = Conexao.ConexaoDB.conectar();

            try
            {
                abrirConn.Open();
                string query = "SELECT * FROM Estoque ORDER BY nome ASC";
                SqlCommand comando = new SqlCommand(query, abrirConn);

                comando.CommandType = CommandType.Text;
                SqlDataAdapter adaptador = new SqlDataAdapter(comando);
                DataTable tabela = new DataTable();
                adaptador.Fill(tabela);

                return tabela;

            }
            catch (Exception ex)
            {
                abrirConn.Close();
                return null;
                throw new Exception("Erro interno ao acesso a base " + ex.Message);
            }
            finally
            {
                abrirConn.Close();
            }

        }
        public string getItem()
        {
            return item;
        }
        public int getQtd()
        {
            return qtd;
        }
    }
}
