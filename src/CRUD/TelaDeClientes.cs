using System.Data;
using System.Data.SqlClient;

namespace CRUD
{
    public partial class FormularioDeClientes : Form
    {
        private string strcon = @"Integrated Security=SSPI;Persist Security Info=False;Initial Catalog=master;Data Source=SAMUELARAAGDCAD";
        private SqlConnection sqlCon;

        private int ?IdClienteSelecionado = null;

        public FormularioDeClientes()
        {
            InitializeComponent();

            lstContatos.View = View.Details;
            lstContatos.LabelEdit = true;
            lstContatos.AllowColumnReorder = true;
            lstContatos.FullRowSelect = true;
            lstContatos.GridLines = true;
            tsbDeletar.Enabled = false;
            tsbEditar.Enabled = false;
            tsbSalvar.Enabled = false;
            tsbLimparDados.Enabled = false;


            lstContatos.Columns.Add("Id", 30, HorizontalAlignment.Left);
            lstContatos.Columns.Add("Nome", 150, HorizontalAlignment.Left);
            lstContatos.Columns.Add("CPF", 100, HorizontalAlignment.Left);
            lstContatos.Columns.Add("Telefone", 150, HorizontalAlignment.Left);
            lstContatos.Columns.Add("Data", 100, HorizontalAlignment.Left);
            lstContatos.Columns.Add("Email", 100, HorizontalAlignment.Left);
            lstContatos.Columns.Add("Endereco", 100, HorizontalAlignment.Left);
            lstContatos.Columns.Add("Bairro", 100, HorizontalAlignment.Left);
            lstContatos.Columns.Add("Cidade", 100, HorizontalAlignment.Left);
            lstContatos.Columns.Add("Estado", 100, HorizontalAlignment.Left);
            lstContatos.Columns.Add("Complemento", 100, HorizontalAlignment.Left);

            CarregarClientes();
        }

        private string strSql = String.Empty;
        public SqlConnection SqlCon { get => sqlCon; set => sqlCon = value; }

        private void AoClicarEmSalvar(object sender, EventArgs e)
        {
            if (txtNome.Text == string.Empty)
            {
                MessageBox.Show("Insira o nome do cliente");
                return;
            }

            try
            {
                SqlCommand cmd = new SqlCommand(strSql, SqlCon);
                

                if (IdClienteSelecionado == null)
                {
                    SqlCon.Open();
                    cmd.Connection = sqlCon;
                    cmd.CommandText = "insert into ClientesDaLoja (Nome, CPF, Telefone, Data, Email, Endereco, Bairro, Cidade, Estado, Complemento) values (@Nome, @CPF, @Telefone, @Data, @Email, @Endereco, @Bairro, @Cidade, @Estado, @Complemento)";
                    cmd.Prepare();

                    cmd.Parameters.AddWithValue("@Nome", txtNome.Text);
                    cmd.Parameters.AddWithValue("@CPF", mskCPF.Text);
                    cmd.Parameters.AddWithValue("@Telefone", mskTel.Text);
                    cmd.Parameters.AddWithValue("@Data", mskData.Text);
                    cmd.Parameters.AddWithValue("@Email", txtEmail.Text);
                    cmd.Parameters.AddWithValue("@Endereco", txtEndereco.Text);
                    cmd.Parameters.AddWithValue("@Bairro", txtBairro.Text);
                    cmd.Parameters.AddWithValue("@Cidade", txtCidade.Text);
                    cmd.Parameters.AddWithValue("@Estado", txtEstado.Text);
                    cmd.Parameters.AddWithValue("@Complemento", txtComplemento.Text);

                    cmd.ExecuteNonQuery();

                    MessageBox.Show("Cadastro Realizado com sucesso!");
                    limparCampos();
                }
                else
                {
                    MessageBox.Show("Cliente ja cadastrado!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                SqlCon.Close();
                CarregarClientes();
            }

        }

        private void AoClicarEmEditar(object sender, EventArgs e)
        {
            
            strSql = "update ClientesDaLoja set Nome=@Nome, CPF=@CPF, Telefone=@Telefone, Data=@Data, Email=@Email, Endereco=@Endereco, Bairro=@Bairro, Cidade=@Cidade, Estado=@Estado, Complemento=@Complemento where Id=@Id";
            sqlCon = new SqlConnection(strcon);
            SqlCommand cmd = new SqlCommand(strSql, SqlCon);

            cmd.Parameters.Add("Id", SqlDbType.VarChar).Value = IdClienteSelecionado;
            cmd.Parameters.Add("Nome", SqlDbType.VarChar).Value = txtNome.Text;
            cmd.Parameters.Add("CPF", SqlDbType.VarChar).Value = mskCPF.Text;
            cmd.Parameters.Add("Telefone", SqlDbType.VarChar).Value = mskTel.Text;
            cmd.Parameters.Add("Data", SqlDbType.VarChar).Value = mskData.Text;
            cmd.Parameters.Add("Email", SqlDbType.VarChar).Value = txtEmail.Text;
            cmd.Parameters.Add("Endereco", SqlDbType.VarChar).Value = txtEndereco.Text;
            cmd.Parameters.Add("Bairro", SqlDbType.VarChar).Value = txtBairro.Text;
            cmd.Parameters.Add("Cidade", SqlDbType.VarChar).Value = txtCidade.Text;
            cmd.Parameters.Add("Estado", SqlDbType.VarChar).Value = txtEstado.Text;
            cmd.Parameters.Add("Complemento", SqlDbType.VarChar).Value = txtComplemento.Text;

            try
            {
                SqlCon.Open();
                cmd.ExecuteNonQuery();
                MessageBox.Show("Cadastro atualizado");

            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                SqlCon.Close();
                CarregarClientes();
            }
        }

        private void AoClicarEmDeletar(object sender, EventArgs e)
        {
            if (MessageBox.Show("Deseja excluir o cliente?", "Cuidado", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.No)
            {
                MessageBox.Show("Operação cancelada");
            }
            else
            {
                strSql = "delete from ClientesDaLoja where Id=@Id";
                SqlCon = new SqlConnection(strcon);
                SqlCommand cmd = new SqlCommand(strSql, SqlCon);
                cmd.Parameters.Add("@Id", SqlDbType.Int).Value = IdClienteSelecionado;
                try
                {
                    SqlCon.Open();
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Cliente deletado.");
                    CarregarClientes();
                    limparCampos();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                finally
                {
                    SqlCon.Close();
                }
            }
        }

        private void AoClicarEmNovoCliente(object sender, EventArgs e)
        {
            limparCampos();

            tsbNovo.Enabled = false;
            tsbSalvar.Enabled = true;
            tsbEditar.Enabled = false;
            tsbDeletar.Enabled = false;
            tsbLimparDados.Enabled = true;

            txtNome.Enabled = true;
            mskCPF.Enabled = true;
            mskTel.Enabled = true;
            mskData.Enabled = true;
            txtEmail.Enabled = true;
            txtEndereco.Enabled = true;
            txtBairro.Enabled = true;
            txtCidade.Enabled = true;
            txtEstado.Enabled = true;
            txtComplemento.Enabled = true;
        }

        private void AoClicarEmLimparDados(object sender, EventArgs e)
        {
            limparCampos();

        }

        private void AoClicarEmNoCliente(object sender, EventArgs e)
        {
            tsbDeletar.Enabled = true;
            tsbEditar.Enabled = true;
            tsbLimparDados.Enabled = true;

            ListView.SelectedListViewItemCollection itensSelecionados = lstContatos.SelectedItems;

            foreach (ListViewItem item in itensSelecionados)
            {
                IdClienteSelecionado = Convert.ToInt32(item.SubItems[0].Text);
                txtNome.Text = item.SubItems[1].Text;
                mskCPF.Text = item.SubItems[2].Text;
                mskTel.Text = item.SubItems[3].Text;
                mskData.Text = item.SubItems[4].Text;
                txtEmail.Text = item.SubItems[5].Text;
                txtEndereco.Text = item.SubItems[6].Text;
                txtBairro.Text = item.SubItems[7].Text;
                txtCidade.Text = item.SubItems[8].Text;
                txtEstado.Text = item.SubItems[9].Text;
                txtComplemento.Text = item.SubItems[10].Text;
            }
        }

        private void AoClicarEmBuscar(object sender, EventArgs e)
        {

            try
            {
                SqlCon.Open();
                SqlCommand cmd = new SqlCommand(strSql, SqlCon);
                cmd.Connection = sqlCon;
                cmd.CommandText = "select * from ClientesDaLoja where Nome like @q or Email like @q or CPF like @q";
                cmd.Prepare();

                cmd.Parameters.AddWithValue("@q", "%" + tstNomeBuscar.Text + "%");

                lstContatos.Items.Clear();
                SqlDataReader dr = cmd.ExecuteReader();


                if (tstNomeBuscar.Text == String.Empty)
                {
                    strSql = "select * from ClientesDaLoja";
                }

                if (dr.HasRows == false)
                {
                    throw new Exception("Cliente não encontrado");
                }

                while (dr.Read())
                {
                    string[] row =
                    {
                        Convert.ToString(dr.GetInt32(0)),
                        dr.GetString(1),
                        dr.GetString(2),
                        dr.GetString(3),
                        dr.GetString(4),
                        dr.GetString(5),
                        dr.GetString(6),
                        dr.GetString(7),
                        dr.GetString(8),
                        dr.GetString(9),
                        dr.GetString(10),
                    };
                    //listView1
                    lstContatos.Items.Add(new ListViewItem(row));
                };
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                SqlCon.Close();
            }
        }

        private void CarregarClientes()
        {
            try
            {

                strSql = "select * from ClientesDaLoja order by Id desc ";

                SqlCon = new SqlConnection(strcon);
                SqlCommand cmd = new SqlCommand(strSql, SqlCon);

                lstContatos.Items.Clear();
                sqlCon.Open();
                SqlDataReader dr = cmd.ExecuteReader();


                while (dr.Read())
                {
                    string[] row =
                    {
                        Convert.ToString(dr.GetInt32(0)),
                        dr.GetString(1),
                        dr.GetString(2),
                        dr.GetString(3),
                        dr.GetString(4),
                        dr.GetString(5),
                        dr.GetString(6),
                        dr.GetString(7),
                        dr.GetString(8),
                        dr.GetString(9),
                        dr.GetString(10),

                    };

                    lstContatos.Items.Add(new ListViewItem(row));
                };
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                SqlCon.Close();
            }
        }
        public void limparCampos()
        {

            tsbNovo.Enabled = true;
            tsbSalvar.Enabled = false;
            tsbEditar.Enabled = false;
            tsbDeletar.Enabled = false;
            tsbLimparDados.Enabled = false;
            txtNome.Enabled = true;
            mskCPF.Enabled = true;
            mskTel.Enabled = true;
            mskData.Enabled = true;
            txtEmail.Enabled = true;
            txtEndereco.Enabled = true;
            txtBairro.Enabled = true;
            txtCidade.Enabled = true;
            txtEstado.Enabled = true;
            txtComplemento.Enabled = true;

            txtNome.Text = "";
            mskCPF.Text = "";
            mskTel.Text = "";
            mskData.Text = "";
            txtEmail.Text = "";
            txtEndereco.Text = "";
            txtBairro.Text = "";
            txtCidade.Text = "";
            txtEstado.Text = "";
            txtComplemento.Text = "";

            IdClienteSelecionado = null;
        }

        private void txtBuscar_TextChanged(object sender, EventArgs e)
        {

        }
        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void maskedTextBox1_MaskInputRejected(object sender, MaskInputRejectedEventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void mskCPF_MaskInputRejected(object sender, MaskInputRejectedEventArgs e)
        {

        }

        private void label9_Click(object sender, EventArgs e)
        {

        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {

        }


        private void label6_Click_1(object sender, EventArgs e)
        {

        }

        private void txtNome_TextChanged(object sender, EventArgs e)
        {

        }

    }
}