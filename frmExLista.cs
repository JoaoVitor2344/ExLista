using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace ExLista
{
    public partial class frmExLista : Form
    {
        SqlConnection con = new SqlConnection("");
        List<Pessoa> pLista = null;

        public frmExLista()
        {
            InitializeComponent();
        }

        //Adicionar objetos a miha lista
        private void CarregaLista()
        {
            pLista = new List<Pessoa>();
            pLista.Add(new Pessoa(1, "João", 12, 'M'));
            pLista.Add(new Pessoa(2, "Maria", 12, 'F'));
            pLista.Add(new Pessoa(3, "Roberto", 89, 'M'));
            pLista.Add(new Pessoa(4, "Mari", 20, 'F'));
            pLista.Add(new Pessoa(5, "Valdir", 20, 'M'));
            pLista.Add(new Pessoa(6, "Rodrigo", 47, 'M'));
        }

        void Imprimir(List<Pessoa> pLista, string info)
        {
            lbResultado.Items.Clear();
            lbResultado.Items.Add(info);
            lbResultado.Items.Add("");
            lbResultado.Items.Add("ID\tNome\tIdade\tSexo");
            pLista.ForEach(delegate (Pessoa p)
            {
                lbResultado.Items.Add(p.ID + "\t" + p.Nome + "\t" + p.Idade + "\t" + p.Sexo);
            });
        }

        private void btnMostrar_Click(object sender, EventArgs e)
        {
            CarregaLista();
            Imprimir(pLista, "Mostrando a lista");
        }

        private void btnFiltro_Click(object sender, EventArgs e)
        {
            List<Pessoa> filtarIdade = pLista.FindAll(delegate (Pessoa p)
            {
                return p.Idade > 30;
            });

            Imprimir(filtarIdade, "Pessoas +30 anos");
        }

        private void btnRemover_Click(object sender, EventArgs e)
        {
            List<Pessoa> RemoveM = pLista;
            RemoveM.RemoveAll(delegate (Pessoa p)
            {
                return p.Sexo == 'M';
            });

            Imprimir(RemoveM, "Removendo todas as pessoa do sexo masculino");
        }

        private void btnLocalizar_Click(object sender, EventArgs e)
        {
            List<Pessoa> pessoas = pLista.FindAll(delegate (Pessoa p)
            {
                return p.Idade == 12;
            });
            
            Imprimir(pessoas, "Pessoas com idade = 12");
        }

        private void btnLocalizar2_Click(object sender, EventArgs e)
        {
            Int32 Lid = Convert.ToInt32(txtId.Text);
            Pessoa pessoas = pLista.Find(delegate (Pessoa p1)
            {
                return p1.ID == Lid;
            });
            lbResultado.Items.Add("Pessoa Localizada!");
            lbResultado.Items.Add(pessoas.ID + "\t" + pessoas.Nome + "\t" + pessoas.Idade + "\t" + pessoas.Sexo);
            txtNome.Text = pessoas.Nome;
            txtIdade.Text = Convert.ToString(pessoas.Idade);
            txtSexo.Text = Convert.ToString(pessoas.Sexo);
        }

        private void btnInserir_Click(object sender, EventArgs e)
        {
            List<Pessoa> npessoa = pLista;
            npessoa.Add(new Pessoa(){ ID = Convert.ToInt32(txtId.Text.Trim()), Nome = txtNome.Text.Trim(), Idade = Convert.ToSByte(txtIdade.Text.Trim()), Sexo = Convert.ToChar(txtSexo.Text.Trim()) });
            Imprimir(npessoa, "Pessoa adicionada!");
            txtId.Text = "";
            txtNome.Text = "";
            txtIdade.Text = "";
            txtSexo.Text = "";
        }

        private void btnExcluir_Click(object sender, EventArgs e)
        {
            Int32 Lid = Convert.ToInt32(txtId.Text);
            List<Pessoa> RemoveLista = pLista;
            RemoveLista.RemoveAll(delegate (Pessoa p) { return p.ID == Lid; });
            Imprimir(RemoveLista, "Pessoa removida!");
        }

        private void btnGravaBanco_Click(object sender, EventArgs e)
        {
            List<Pessoa> ex = pLista;
            for (int i = 0; i < ex.Count; i++)
            {
                SqlCommand cmd = new SqlCommand("InserirPessoa", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@id", SqlDbType.Int).Value = ex[i].ID;
                cmd.Parameters.AddWithValue("@nome", SqlDbType.NChar).Value = ex[i].Nome;
                cmd.Parameters.AddWithValue("@idade", SqlDbType.SmallInt).Value = ex[i].Idade;
                cmd.Parameters.AddWithValue("@sexo", SqlDbType.Char).Value = ex[i].Sexo;
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
            }
            MessageBox.Show("Lista gravada com sucesso", "Gravação", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            Int32 Lid = Convert.ToInt32(txtId.Text);
            List<Pessoa> RemoveLista = pLista;
            RemoveLista.RemoveAll(delegate (Pessoa p1) { return p1.ID == Lid; });
            Pessoa pessoa = new Pessoa();
            pessoa.Nome = txtNome.Text.Trim();
            pessoa.Idade = Convert.ToSByte(txtIdade.Text.Trim());
            pessoa.Sexo = Convert.ToChar(txtSexo.Text.Trim());
            Imprimir(RemoveLista, "Pessoa atualizada!");
            lbResultado.Items.Insert(Lid+2, Lid + "\t" + pessoa.Nome + "\t" + pessoa.Idade + "\t" + pessoa.Sexo);
        }
    }
}
