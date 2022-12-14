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

namespace HistoricoView
{
    public partial class TelaHistorico : Form
    {
        public TelaHistorico()
        {
            InitializeComponent();
        }
        private void btnExibir_Click_1(object sender, EventArgs e)
        {
            ctrlHistorico _ctrlHistorico = new ctrlHistorico();
            var tabela = _ctrlHistorico.ExibirHistorico();
            grdHistorico.DataSource = tabela;
        } 
        private void btnEntrada_Click_1(object sender, EventArgs e)
        {

            ctrlHistorico _ctrlHistorico = new ctrlHistorico();
            var tabela = _ctrlHistorico.ExibirEntrada();
            grdHistorico.DataSource = tabela;

        }
        private void btnSaida_Click_1(object sender, EventArgs e)
        {
            string Arquivo = "tabela";
            ctrlHistorico _ctrlHistorico = new ctrlHistorico();
            var tabela = _ctrlHistorico.ExibirSaida();
            grdHistorico.DataSource = tabela;


            if (grdHistorico.Rows.Count > 0)
            {
                SaveFileDialog pdf = new SaveFileDialog();
                pdf.Filter = "PDF (.pdf) | * .pdf";
                pdf.FileName = Arquivo;

            }
        }
        private void btnPesquisarEntrada_Click(object sender, EventArgs e)
        {
            try
            {
            ctrlHistorico _ctrlHistorico = new ctrlHistorico();
            var data = Convert.ToDateTime(txtData.Text);
            var tabela = _ctrlHistorico.PesquisarEntrada(data);
            grdHistorico.DataSource = tabela;
            }
            catch (Exception)
            {
                MessageBox.Show("Digite a data por favor, no formato dia/mês/ano", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void btnPesquisarSaida_Click(object sender, EventArgs e)
        {
            try
            {
                ctrlHistorico _ctrlHistorico = new ctrlHistorico();
                var data = Convert.ToDateTime(txtData.Text);
                var tabela = _ctrlHistorico.PesquisarSaida(data);
                grdHistorico.DataSource = tabela;
            }catch (Exception formato)
            {
                MessageBox.Show("Digite a data por favor, no formato dia/mês/ano", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void btnExibirDevolucoes_Click(object sender, EventArgs e)
        {
            ctrlHistorico _ctrlHistorico = new ctrlHistorico();
            var tabela = _ctrlHistorico.ExibirDevolucao();
            grdHistorico.DataSource = tabela;
        }
        private void btnDevolucao_Click(object sender, EventArgs e)
        {
            try
            {
                ctrlHistorico _ctrlHistorico = new ctrlHistorico();
                var data = Convert.ToDateTime(txtData.Text);
                var tabela = _ctrlHistorico.PesquisarDevolucao(data);
                grdHistorico.DataSource = tabela;
            }
            catch (Exception formato)
            {
                MessageBox.Show("Digite a data por favor, no formato dia/mês/ano", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void TelaHistorico_Load(object sender, EventArgs e)
        {
            try
            {
            this.historicoTableAdapter.Fill(this.dbHistorico.historico);
            }catch(Exception)
            {

            }
            finally
            {
                ctrlHistorico _ctrlHistorico = new ctrlHistorico();
                DataTable tabela = _ctrlHistorico.ExibirHistorico();
                grdHistorico.DataSource = tabela;
            }
        }
    }
}
