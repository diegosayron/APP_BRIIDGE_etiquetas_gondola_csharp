using APP_BRIIDGE_etiquetas.Api;
using APP_BRIIDGE_etiquetas.Models;
using APP_BRIIDGE_etiquetas.Printing;
using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace APP_BRIIDGE_etiquetas
{
    public partial class FrmEtiqueta : Form
    {
        ComboBox cboImpressora = new();
        ComboBox cboTipoEtiqueta = new();
        NumericUpDown numQuantidade = new();
        TextBox txtCodigo = new();
        Label lblDescricao = new();
        Label lblPreco = new();
        private Label label1;
        private Label label2;
        private Label label3;
        private NumericUpDown nudQuantidade;
        private TextBox txtPesquisa;
        private Label label4;
        private Label lblEAN;
        private PictureBox pictureBox1;
        private Label lblUnidade;
        private Button btnFechar;
        private Button btnPesquisar;
        Button btnImprimir = new();

        private readonly ApiClient api = new();
        private Produto? produtoAtual;

        public FrmEtiqueta()
        {
            InitializeComponent();

            Text = "Impressão de Etiquetas";
            StartPosition = FormStartPosition.CenterScreen;

            CriarControles();
        }

        private void CriarControles()
        {
            //...
        }

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmEtiqueta));
            cboImpressora = new ComboBox();
            label1 = new Label();
            label2 = new Label();
            cboTipoEtiqueta = new ComboBox();
            label3 = new Label();
            nudQuantidade = new NumericUpDown();
            txtPesquisa = new TextBox();
            label4 = new Label();
            lblDescricao = new Label();
            lblEAN = new Label();
            pictureBox1 = new PictureBox();
            lblUnidade = new Label();
            lblPreco = new Label();
            btnImprimir = new Button();
            btnFechar = new Button();
            btnPesquisar = new Button();
            ((System.ComponentModel.ISupportInitialize)nudQuantidade).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            SuspendLayout();
            // 
            // cboImpressora
            // 
            cboImpressora.Font = new Font("Segoe UI", 14F);
            cboImpressora.FormattingEnabled = true;
            cboImpressora.Location = new Point(215, 59);
            cboImpressora.Name = "cboImpressora";
            cboImpressora.Size = new Size(567, 39);
            cboImpressora.TabIndex = 0;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 14F);
            label1.Location = new Point(79, 66);
            label1.Name = "label1";
            label1.Size = new Size(130, 32);
            label1.TabIndex = 1;
            label1.Text = "Impressora";
            label1.TextAlign = ContentAlignment.MiddleRight;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI", 14F);
            label2.Location = new Point(20, 111);
            label2.Name = "label2";
            label2.Size = new Size(189, 32);
            label2.TabIndex = 3;
            label2.Text = "Tipo de Etiqueta";
            label2.TextAlign = ContentAlignment.MiddleRight;
            // 
            // cboTipoEtiqueta
            // 
            cboTipoEtiqueta.Font = new Font("Segoe UI", 14F);
            cboTipoEtiqueta.FormattingEnabled = true;
            cboTipoEtiqueta.Location = new Point(215, 104);
            cboTipoEtiqueta.Name = "cboTipoEtiqueta";
            cboTipoEtiqueta.Size = new Size(567, 39);
            cboTipoEtiqueta.TabIndex = 2;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Segoe UI", 14F);
            label3.Location = new Point(70, 177);
            label3.Name = "label3";
            label3.Size = new Size(139, 32);
            label3.TabIndex = 4;
            label3.Text = "Quantidade";
            label3.TextAlign = ContentAlignment.MiddleRight;
            // 
            // nudQuantidade
            // 
            nudQuantidade.Font = new Font("Segoe UI", 14F);
            nudQuantidade.Location = new Point(215, 170);
            nudQuantidade.Maximum = new decimal(new int[] { 20, 0, 0, 0 });
            nudQuantidade.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            nudQuantidade.Name = "nudQuantidade";
            nudQuantidade.Size = new Size(150, 39);
            nudQuantidade.TabIndex = 5;
            nudQuantidade.Value = new decimal(new int[] { 1, 0, 0, 0 });
            // 
            // txtPesquisa
            // 
            txtPesquisa.CharacterCasing = CharacterCasing.Upper;
            txtPesquisa.Font = new Font("Segoe UI", 14F);
            txtPesquisa.Location = new Point(215, 215);
            txtPesquisa.Name = "txtPesquisa";
            txtPesquisa.Size = new Size(567, 39);
            txtPesquisa.TabIndex = 6;
            txtPesquisa.TextChanged += txtPesquisa_TextChanged;
            txtPesquisa.KeyDown += txtPesquisa_KeyDown;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new Font("Segoe UI", 14F);
            label4.Location = new Point(118, 222);
            label4.Name = "label4";
            label4.Size = new Size(91, 32);
            label4.TabIndex = 7;
            label4.Text = "Código";
            label4.TextAlign = ContentAlignment.MiddleRight;
            // 
            // lblDescricao
            // 
            lblDescricao.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            lblDescricao.Location = new Point(217, 308);
            lblDescricao.Name = "lblDescricao";
            lblDescricao.Size = new Size(567, 32);
            lblDescricao.TabIndex = 8;
            lblDescricao.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblEAN
            // 
            lblEAN.AutoSize = true;
            lblEAN.Font = new Font("Segoe UI", 8F);
            lblEAN.Location = new Point(217, 340);
            lblEAN.Name = "lblEAN";
            lblEAN.Size = new Size(105, 19);
            lblEAN.TabIndex = 9;
            lblEAN.Text = "789000000000";
            lblEAN.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // pictureBox1
            // 
            pictureBox1.Location = new Point(217, 308);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(567, 163);
            pictureBox1.TabIndex = 10;
            pictureBox1.TabStop = false;
            // 
            // lblUnidade
            // 
            lblUnidade.AutoSize = true;
            lblUnidade.Font = new Font("Segoe UI", 8F);
            lblUnidade.Location = new Point(729, 340);
            lblUnidade.Name = "lblUnidade";
            lblUnidade.Size = new Size(0, 19);
            lblUnidade.TabIndex = 11;
            lblUnidade.TextAlign = ContentAlignment.MiddleRight;
            // 
            // lblPreco
            // 
            lblPreco.BorderStyle = BorderStyle.FixedSingle;
            lblPreco.Font = new Font("Arial Rounded MT Bold", 48F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblPreco.Location = new Point(273, 363);
            lblPreco.Name = "lblPreco";
            lblPreco.Size = new Size(448, 95);
            lblPreco.TabIndex = 12;
            lblPreco.Text = "19,99";
            lblPreco.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // btnImprimir
            // 
            btnImprimir.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            btnImprimir.Location = new Point(621, 549);
            btnImprimir.Name = "btnImprimir";
            btnImprimir.Size = new Size(179, 49);
            btnImprimir.TabIndex = 13;
            btnImprimir.Text = "Imprimir";
            btnImprimir.UseVisualStyleBackColor = true;
            btnImprimir.Click += btnImprimir_Click;
            // 
            // btnFechar
            // 
            btnFechar.Font = new Font("Segoe UI", 12F);
            btnFechar.Location = new Point(419, 549);
            btnFechar.Name = "btnFechar";
            btnFechar.Size = new Size(179, 49);
            btnFechar.TabIndex = 14;
            btnFechar.Text = "Fechar";
            btnFechar.UseVisualStyleBackColor = true;
            btnFechar.Click += btnFechar_Click;
            // 
            // btnPesquisar
            // 
            btnPesquisar.Font = new Font("Segoe UI", 8F);
            btnPesquisar.Location = new Point(215, 260);
            btnPesquisar.Name = "btnPesquisar";
            btnPesquisar.Size = new Size(150, 42);
            btnPesquisar.TabIndex = 15;
            btnPesquisar.Text = "Pesquisar";
            btnPesquisar.UseVisualStyleBackColor = true;
            btnPesquisar.Click += btnPesquisar_Click;
            // 
            // FrmEtiqueta
            // 
            ClientSize = new Size(854, 669);
            Controls.Add(btnPesquisar);
            Controls.Add(btnFechar);
            Controls.Add(btnImprimir);
            Controls.Add(lblPreco);
            Controls.Add(lblUnidade);
            Controls.Add(lblEAN);
            Controls.Add(lblDescricao);
            Controls.Add(label4);
            Controls.Add(txtPesquisa);
            Controls.Add(nudQuantidade);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(cboTipoEtiqueta);
            Controls.Add(label1);
            Controls.Add(cboImpressora);
            Controls.Add(pictureBox1);
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FrmEtiqueta";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Impressão de Etiquetas - Briidge Inovação e Tecnologias Avançadas";
            Load += FrmEtiqueta_Load;
            ((System.ComponentModel.ISupportInitialize)nudQuantidade).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ResumeLayout(false);
            PerformLayout();

        }

        private void ExibirProduto()
        {
            if (produtoAtual == null)
                return;

            lblDescricao.Text = produtoAtual.Nome;
            lblEAN.Text = produtoAtual.Codigo;
            lblUnidade.Text = produtoAtual.Unidade;
            lblPreco.Text = produtoAtual.PrecoVenda.ToString("C2");
        }

        private void LimparTela()
        {
            produtoAtual = null;

            txtPesquisa.Clear();

            lblDescricao.Text = "";
            lblEAN.Text = "";
            lblUnidade.Text = "";
            lblPreco.Text = "";

            btnImprimir.Enabled = false;

            txtPesquisa.Focus();
        }

        private void FrmEtiqueta_Load(object sender, EventArgs e)
        {
            CarregarImpressoras();
            CarregarTiposEtiqueta();

            btnImprimir.Enabled = false;

            nudQuantidade.Value = 1;

            txtPesquisa.Focus();
        }

        private void CarregarImpressoras()
        {
            cboImpressora.Items.Clear();

            foreach (string impressora in PrinterSettings.InstalledPrinters)
                cboImpressora.Items.Add(impressora);

            if (cboImpressora.Items.Count > 0)
                cboImpressora.SelectedIndex = 0;
        }

        private void CarregarTiposEtiqueta()
        {
            cboTipoEtiqueta.Items.Clear();

            cboTipoEtiqueta.Items.Add("100 x 37 mm");
            cboTipoEtiqueta.Items.Add("105 x 111 mm");
            cboTipoEtiqueta.Items.Add("105 x 30 mm");

            cboTipoEtiqueta.SelectedIndex = 2;
        }

        private async Task PesquisarProduto(bool imprimirAutomaticamente)
        {
            string pesquisa = txtPesquisa.Text.Trim();

            if (pesquisa.Length == 0)
                return;

            Cursor = Cursors.WaitCursor;

            try
            {
                produtoAtual = await api.BuscarProdutoAsync(pesquisa);

                if (produtoAtual == null)
                {
                    MessageBox.Show(
                        "Produto não encontrado.",
                        "Pesquisa",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);

                    return;
                }

                ExibirProduto();
                btnImprimir.Enabled = true;
                if (imprimirAutomaticamente)
                    btnImprimir.PerformClick();

            }
            finally
            {
                Cursor = Cursors.Default;
                txtPesquisa.Focus();
            }
        }

        private async void btnPesquisar_Click(object sender, EventArgs e)
        {
            await PesquisarProduto(false);
        }

        private async void txtPesquisa_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter)
                return;

            e.SuppressKeyPress = true;
            e.Handled = true;

            await PesquisarProduto(true);
        }

        private void txtPesquisa_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnImprimir_Click(object sender, EventArgs e)
        {
            if (produtoAtual == null)
                return;

            if (cboTipoEtiqueta.SelectedIndex == 0)
            {
                Etiqueta100x37Printer printer =
                    new Etiqueta100x37Printer(produtoAtual);

                printer.Imprimir(
                    cboImpressora.Text,
                    (int)nudQuantidade.Value);
            }
            else if (cboTipoEtiqueta.SelectedIndex == 1)
            {
                Etiqueta105x111Printer printer =
                    new Etiqueta105x111Printer(produtoAtual);

                printer.Imprimir(
                    cboImpressora.Text,
                    (int)nudQuantidade.Value);
            }
            else if (cboTipoEtiqueta.SelectedIndex == 2)
            { 
                    Etiqueta105x30Printer printer =
                        new Etiqueta105x30Printer(produtoAtual);

                    printer.Imprimir(
                        cboImpressora.Text,
                        (int)nudQuantidade.Value);
            }
            LimparTela();
        }

        private void btnFechar_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
