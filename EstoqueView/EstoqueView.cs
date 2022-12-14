using EstoqueControle;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using HistoricoControle;
using HistoricoModel;
using System.Data.SqlClient;
using Conexao;
using HistoricoView;
using System.Threading;
using ColaboradoresControle;
using colaboradoreModel;
using UsuariosModel;
using EstoqueView;

namespace Almoxarifado
{
    public partial class frmEstoque : Form
    {
        ctrlEstoque _ctrlEstoque = new ctrlEstoque();
        ctrlHistorico _ctrlHistorico = new ctrlHistorico();
        mdlHistorico _mdlHistorico = new mdlHistorico();
        ctrlColaboradores _ctrlColaboradores = new ctrlColaboradores();
        Thread _thread;
        
        public frmEstoque()
        {
            InitializeComponent();
        }
        private void btnProcurar_Click_1(object sender, EventArgs e)
        {
            try
            {
            ctrlEstoque _ctrlEstoque = new ctrlEstoque();
            string nome = txtProcurar.Text;
            DataTable retorno = _ctrlEstoque.Pesquisar(nome);

            if (retorno == null)
            {
                MessageBox.Show("Item não localizado", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtProcurar.Text = String.Empty;
                grdEstoque.DataSource = _ctrlEstoque.Exibir();
            }
                else if (retorno != null && nome != String.Empty)
            {
                grdEstoque.DataSource = _ctrlEstoque.Pesquisar(nome);
            }   else
                {
                    MessageBox.Show("Digite o nome do item", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }catch(Exception)
            {
                MessageBox.Show("Erro na pesquisa", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void btnEntrada_Click(object sender, EventArgs e)
        {

            string nome = txtProcurar.Text;
            string nomeRequisitante = txtNomeRequisitante.Text;
            string quantidade = txtQuantidade.Text;
            DataTable tabelaEstoque = _ctrlEstoque.Entrada(nome, quantidade);
            string linhaSelecionada = grdEstoque.SelectedCells[0].Value.ToString();

            try
            {
                if (tabelaEstoque != null && nome != String.Empty && quantidade != String.Empty && nome == linhaSelecionada && nomeRequisitante != String.Empty)
                {
                    grdEstoque.DataSource = _ctrlEstoque.Pesquisar(nome);
                    MessageBox.Show("Entrada efetuada com sucesso", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

                    _mdlHistorico.natureza = "ENTRADA";
                    _mdlHistorico.operador =  mdlUsuarios.getNome();
                    _mdlHistorico.quantidade = Convert.ToInt32(quantidade);
                    _mdlHistorico.item = nome.ToUpper();
                    _mdlHistorico.requisitante = nomeRequisitante.ToUpper();
                    
                    _ctrlHistorico.CadastrarHistorico(_mdlHistorico);

                    txtProcurar.Text = String.Empty;
                    txtQuantidade.Text = String.Empty;
                }else
                {
                    MessageBox.Show("Verifique se: \n 1) Os campos nome, quantidade e colaborador estão corretamente preenchidos \n 2) Se o nome do item foi selecionado", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }catch (Exception erro)
            {
                MessageBox.Show("Erro na entrada: "+ erro, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void btnSaida_Click(object sender, EventArgs e)
        {
            /*
            string nome = txtProcurar.Text;
            string nomeRequisitante = txtNomeRequisitante.Text;
            string quantidade = txtQuantidade.Text;
            var tabelaEstoque = _ctrlEstoque.Saida(nome, quantidade);
            string linhaSelecionada = grdEstoque.SelectedCells[0].Value.ToString();

            try
            {
                if (tabelaEstoque != null && nome != String.Empty && quantidade != String.Empty && nome == linhaSelecionada && nomeRequisitante != String.Empty)
                {
                    grdEstoque.DataSource = _ctrlEstoque.Pesquisar(nome);
                    MessageBox.Show("Saída efetuada com sucesso", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

                    _mdlHistorico.natureza = "SAÍDA";
                    _mdlHistorico.operador = mdlUsuarios.getNome();
                    _mdlHistorico.quantidade = Convert.ToInt32(quantidade);
                    _mdlHistorico.item = nome.ToUpper();
                    _mdlHistorico.requisitante = nomeRequisitante.ToUpper();

                    _ctrlHistorico.CadastrarHistorico(_mdlHistorico);

                    txtProcurar.Text = String.Empty;
                    txtQuantidade.Text = String.Empty;
                }
                else
                {
                    MessageBox.Show("Verifique se: \n 1) Os campos nome e quantidade estão corretamente preenchidos \n 2) Se o nome do item foi selecionado", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }
            catch (Exception erro)
            {
                MessageBox.Show("Erro na entrada: " + erro, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            */
            _thread = new Thread(TelaSaida);
            _thread.SetApartmentState(ApartmentState.STA);
            _thread.Start();
        }
        private void btnDevolucao_Click(object sender, EventArgs e)
        {
            string nome = txtProcurar.Text;
            string nomeRequisitante = txtNomeRequisitante.Text;
            string quantidade = txtQuantidade.Text;
            DataTable tabelaEstoque = _ctrlEstoque.Entrada(nome, quantidade);
            string linhaSelecionada = grdEstoque.SelectedCells[0].Value.ToString();

            try
            {
                if (tabelaEstoque != null && nome != String.Empty && quantidade != String.Empty && nome == linhaSelecionada && nomeRequisitante != String.Empty)
                {
                    grdEstoque.DataSource = _ctrlEstoque.Pesquisar(nome);
                    MessageBox.Show("Devolução efetuada com sucesso", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

                    _mdlHistorico.natureza = "DEVOLUÇÃO";
                    _mdlHistorico.operador = mdlUsuarios.getNome();
                    _mdlHistorico.quantidade = Convert.ToInt32(quantidade);
                    _mdlHistorico.item = nome.ToUpper();
                    _mdlHistorico.requisitante = nomeRequisitante.ToUpper();

                    _ctrlHistorico.CadastrarHistorico(_mdlHistorico);

                    txtProcurar.Text = String.Empty;
                    txtQuantidade.Text = String.Empty;
                }
                else
                {
                    MessageBox.Show("Verifique se: \n 1) Os campos nome e quantidade estão corretamente preenchidos \n 2) Se o nome do item foi selecionado", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }
            catch (Exception erro)
            {
                MessageBox.Show("Erro na entrada: " + erro, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void btnOrdenar_Click_1(object sender, EventArgs e)
        {
            ctrlEstoque _ctrlEstoque = new ctrlEstoque();

            grdEstoque.DataSource = _ctrlEstoque.Exibir();
        }
        private void btnEstoqueZerado_Click_1(object sender, EventArgs e)
        {
            ctrlEstoque _ctrlEstoque = new ctrlEstoque();

            grdEstoque.DataSource = _ctrlEstoque.ExibirZerado();
        }
        private void btnBuscarColaborador_Click(object sender, EventArgs e)
        {
            
            try
            {
            int cracha = Convert.ToInt32(txtCracha.Text);
            bool retorno = _ctrlColaboradores.pesquisarColaborador(cracha);

                if (retorno == false)
                {
                    MessageBox.Show("Colaborador não localizado", "Não localizado", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }else
                {
                txtNomeRequisitante.Text = _ctrlColaboradores.getNome();
                }
            }catch (Exception)
            {
                MessageBox.Show("Por favor digite o número do crachá", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }
        private void frmEstoque_Load(object sender, EventArgs e)
        {
            try
            {
            // TODO: esta linha de código carrega dados na tabela 'dbGaleria.Estoque'. Você pode movê-la ou removê-la conforme necessário.
            this.estoqueTableAdapter.Fill(this.dbGaleria.Estoque);
            }catch(Exception)
            {

            }
            finally
            {
            ctrlEstoque _ctrlEstoque = new ctrlEstoque();
            DataTable tabela = _ctrlEstoque.Exibir();
            grdEstoque.DataSource = tabela;
            txtNomeRequisitante.Enabled = false;
            }
        }
        private void grdEstoque_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            txtProcurar.Text = grdEstoque.SelectedCells[0].Value.ToString();
        }
        public void telaHistorico()
        {
            Application.Run(new TelaHistorico());
        }
        public void TelaSaida()
        {
            Application.Run(new SaidaView());
        }
    }
}
