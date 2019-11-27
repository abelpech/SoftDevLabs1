using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

/*
 Class Software Development in Propietary Platforms
 Instructor - Engineer Angel Maldonado
 Developer Abel Pech / Marc Albrand
   
 */

namespace AngelC4
{
    public partial class Main : Form
    {
        bool CincoSegundos = false;
        bool Open = false;
        object senderr;
        string path;

        EventArgs ee;
        private SqlConnection conexion = new SqlConnection("server=PECH ; database=TestDB ; integrated security = true");
        public Main()
        {
            InitializeComponent();
            tabControl1.SelectedTab = tabPage9;
            BtnSelect_Click(senderr, ee);
            EstablecerComboBox();
            this.WindowState = FormWindowState.Maximized;
        }

        private void BtnWindows_Click(object sender, EventArgs e)
        {

            if (CincoSegundos)
            {
                toolStripStatusLabel1.Text = "Se cerro la conexion";
                conexion.Close();
                Open = false;
                CincoSegundos = false;
            }
            else if(Open == false)
            {
                conexion.Open();
                Open = true;
                timer1.Start();
                toolStripStatusLabel1.Text = "Se abrio la conexion con el servidor SQL Server y se selecciono la base de datos";

            }

        }

        private void BtnSQL_Click(object sender, EventArgs e)
        {
            conexion.Open();

            MessageBox.Show("Se abrio la conexion con el servidor y se selecciono la base de datos");

            conexion.Close();

            MessageBox.Show("Se cerro la conexion.");
        }

        private void Timer1_Tick(object sender, EventArgs e)
        {
            CincoSegundos = true;
            timer1.Stop();
            BtnWindows_Click(sender, e);
        }

        private void BtnInsertar_Click(object sender, EventArgs e)
        {
            conexion.Open();

            string IdOficial = Convert.ToString(nudInsIdOficial.Value);
            string NombreCompleto = txtInsNombreCompleto.Text;
            int Puesto = Convert.ToInt32(cbPuesto.SelectedValue);
            DateTime FechaDeNacimiento = dtpInsFechaDeNacimiento.Value;

            //String con comando INSERT y los valores capturados.
            string cadena = "INSERT into [Empleados] (" +
                "[IdOficial], " +
                "[NombreCompleto], " +
                "[Puesto], " +
                "[FechaNacimiento]) " +
                "values (" +
                "'" + IdOficial + "'," +
                "'" + NombreCompleto + "'," +
                "'" + Puesto + "'," + 
                "'" + FechaDeNacimiento + "'" +
                ")";
            //MessageBox.Show(Convert.ToString(FechaDeNacimiento);
            SqlCommand comando = new SqlCommand(cadena, conexion);
            
            //el metodo ExecuteNonQuery se comunica con el servidor para que ejecute el comando de SQL
            comando.ExecuteNonQuery();

            MessageBox.Show("Los datos se guardaron correctamente");

            //txtInsIdOficial.Text = "";
            txtInsNombreCompleto.Text = "";

            conexion.Close();
        }

        private void BtnSelect_Click(object sender, EventArgs e)
        {
            txtSelect.Clear();
            conexion.Open();

            //string cadena = "select [IdOficial], [NombreCompleto], [FechaNacimiento] FROM [Empleados]";
            string cadena = "select [IdEmpleado], [IdOficial], [NombreCompleto], [Puesto], [FechaNacimiento] FROM [Empleados]";

            SqlCommand comando = new SqlCommand(cadena, conexion);

            //Utilizamos el metodo ExecuteReader del objeto Sqlcommand para recuperar los datos que genera el SQL server
            //El metodo regresa un objeto SqlDatareader que almacena en el resultado del comando.
            SqlDataReader registros = comando.ExecuteReader();

            //Utilizamos una secuencia repetitiva while para llamar en cada ciclo al metodo read
            //Para acceder a cada fila que nos regresa el comando de select SQL

            //MessageBox.Show("Llamando al metodo Read");
            while (registros.Read())
            {
                
                txtSelect.AppendText(registros["IdOficial"].ToString());
                txtSelect.AppendText(" - ");
                txtSelect.AppendText(registros["IdEmpleado"].ToString());
                txtSelect.AppendText(" - ");
                txtSelect.AppendText(registros["NombreCompleto"].ToString());
                txtSelect.AppendText(" - ");
                txtSelect.AppendText(registros["Puesto"].ToString());
                txtSelect.AppendText(" - ");
                txtSelect.AppendText(registros["FechaNacimiento"].ToString());
                txtSelect.AppendText(" - ");


                txtSelect.AppendText(Environment.NewLine);
            }

            // Cada registro lo insertamos en una linea en el textbox multiline
            
            conexion.Close();

        }

        private void BtnSearch1_Click(object sender, EventArgs e)
        {
            conexion.Open();

            string IdOficial = txtIdOficial.Text;

            string cadena = "select [IdOficial], [NombreCompleto] FROM [Empleados] WHERE [IdOficial] = " + "'" + IdOficial + "'";

            SqlCommand comando = new SqlCommand(cadena, conexion);

            SqlDataReader registro = comando.ExecuteReader();

            //Si en el del comando de consulta se recupero un registro de la tabla se hace la llamada al metodo read
            //Y se despliega el resultado
            if (registro.Read())
            {
                lbIdOficial.Text = registro["IdOficial"].ToString();
                lbNombreCompleto.Text = registro["NombreCompleto"].ToString();
            }
            else
                MessageBox.Show("No existe un articulo con el codigo ingresado");

            conexion.Close();

        }

        private void BtnSearch2_Click(object sender, EventArgs e)
        {
            conexion.Open();

            string cadena = "select [IdOficial], [NombreCompleto] from [Empleados] where [IdOficial] = @IdOficial";

            SqlCommand comando = new SqlCommand(cadena, conexion);

            //Llamamos al metedo Add de la propiedad Parameters del objeto SqlCommand en donde indicamos el parametro
            //y de que tipo de parametro

            SqlParameter param = new SqlParameter();

            param.ParameterName = "@IdOficial";
            param.Value = int.Parse(txtIdOficial.Text);

            comando.Parameters.Add(param);

            SqlDataReader reader = comando.ExecuteReader();

            if (reader.Read())
            {
                lbIdOficial.Text = reader["IdOficial"].ToString();
                lbNombreCompleto.Text = reader["NombreCompleto"].ToString();


            }
            else
                MessageBox.Show("No existe un empleado con el IdOficial ingresado");

            conexion.Close();
        }

        private void BtnSearchDelete_Click(object sender, EventArgs e)
        {
            conexion.Open();

            string IdOficial = txtIdOficialDelete.Text;

            string cadena = "select [IdOficial], [NombreCompleto] from [Empleados] where [IdOficial] = " + "'" + IdOficial + "'";

            SqlCommand comando = new SqlCommand(cadena, conexion);

            SqlDataReader registro = comando.ExecuteReader();

            if (registro.Read())
            {
                lbIdOficialDelete.Text = registro["IdOficial"].ToString();
                lbNombreCompletoDelete.Text = registro["NombreCompleto"].ToString();
                btnDeleteDelete.Enabled = true;

            }
            else
            {
                MessageBox.Show("No existe un empleado con ese IdOficial ingresado");
                btnDeleteDelete.Enabled = false;
            }
                

            conexion.Close();
        }

        private void BtnDeleteDelete_Click(object sender, EventArgs e)
        {
            conexion.Open();

            string IdOficial = txtIdOficialDelete.Text;

            string cadena = "delete from [Empleados] where [IdOficial] = " + IdOficial;

            SqlCommand comando = new SqlCommand(cadena, conexion);

            int cant;
            cant = comando.ExecuteNonQuery();

            if (cant == 1)
            {
                lbIdOficialDelete.Text = "";
                lbNombreCompletoDelete.Text = "";
                MessageBox.Show("No existe un empleado con el IdOficial ingresado");
            }

            conexion.Close();

            btnDeleteDelete.Enabled = false;
        }

        private void BtnSearchUpdate_Click(object sender, EventArgs e)
        {
            conexion.Open();
            string IdOficial = txtIdOficialUpdate.Text;
            string cadena = "SELECT [NombreCompleto], [Puesto] FROM [Empleados] WHERE [IdOficial] = " + "'" + IdOficial + "'";
            SqlCommand comando = new SqlCommand(cadena, conexion);
            SqlDataReader registro = comando.ExecuteReader();
            if (registro.Read())
            {
                txtNIdOficialUpdate.Text = IdOficial;
                txtNombreCompletoUpdate.Text = registro["NombreCompleto"].ToString();
                cbPuestoUpdate.SelectedIndex = (Convert.ToInt32(registro["Puesto"]) - 1);
                btnUpdateUpdate.Enabled = true;

            }
            else
                MessageBox.Show("No existe un empleado con el IdOficial ingresado");
            conexion.Close();
        }

        private void BtnUpdateUpdate_Click(object sender, EventArgs e)
        {
            conexion.Open();

            string IdOficial = txtIdOficialUpdate.Text;
            string NIdOficial = txtNIdOficialUpdate.Text;
            string NombreCompleto = txtNombreCompletoUpdate.Text;
            int Puesto = cbPuestoUpdate.SelectedIndex;
            DateTime dtpFechaNac = dtpFechaNacUpdate.Value;

            string cadena = "UPDATE [Empleados] SET [IdOficial] = " + NIdOficial + "," +
                " [NombreCompleto] = '" + NombreCompleto+ "', " + 
                " [Puesto] = " + (Puesto+1) + ", [FechaNacimiento] = '" + dtpFechaNac + "' " +  " WHERE IdOficial = " + IdOficial;

            SqlCommand comando = new SqlCommand(cadena, conexion);
            int cant;
            MessageBox.Show(cadena);
            cant = comando.ExecuteNonQuery();
            if (cant == 1)
            {
                MessageBox.Show("Se modificaron los datos del empleado");
                txtIdOficialUpdate.Text = "";
                txtNombreCompletoUpdate.Text = "";
                cbPuestoUpdate.SelectedIndex = 0;
                txtNIdOficialUpdate.Text = "";
                
            }
            else
                MessageBox.Show("No existe un empleado con el IdOficial ingresado");

            conexion.Close();
            btnUpdateUpdate.Enabled = false;


        }

        private void Button1_Click(object sender, EventArgs e)
        {
            conexion.Open();

            string cadena = "INSERT into [Empleados] (" +
                            "[IdOficial], " +
                            "[NombreCompleto], " +
                            "[Puesto], " +
                            "[FechaNacimiento]) " +
                            "values (" +
                            "@IdOficial," +
                            "@NombreCompleto," +
                            "@Puesto," +
                            "@FechaNac" +
                            ")";
            SqlCommand comando = new SqlCommand(cadena, conexion);

            //Llamamos al metodo add de la propiedad parameters del objeto SqlCommand en donde indicamos el parametro
            //y de que tipo de parametro
            comando.Parameters.Add("@IdOficial", SqlDbType.Int);
            comando.Parameters.Add("@NombreCompleto", SqlDbType.NVarChar);
            comando.Parameters.Add("@Puesto", SqlDbType.Int);
            comando.Parameters.Add("@FechaNac", SqlDbType.SmallDateTime);

            //Una vez los parametros creados inicializamos con los valores que estamos capturando y como subindice 
            //Podemos indicar el nombre del parametro
            comando.Parameters["@IdOficial"].Value = int.Parse(txtIdOficialParameters.Text);
            comando.Parameters["@NombreCompleto"].Value = txtNombreCompletoParameters.Text;
            comando.Parameters["@Puesto"].Value = (cbPuestoParameters.SelectedIndex + 1);
            comando.Parameters["@FechaNac"].Value = dtpFechaNacParameters.Value;

            comando.ExecuteNonQuery();

            MessageBox.Show("Los datos se guardaron correctamente");

            txtIdOficialParameters.Text = "";
            txtNombreCompletoParameters.Text = "";

            conexion.Close();
        }

        public void EstablecerComboBox()
        {
            cbPuesto.Items.Clear();
            cbPuestoParameters.Items.Clear();
            cbPuestoUpdate.Items.Clear();
            conexion.Open();

            //cbPuestoParameters.Items.Add("Hola");
            string query = "SELECT [Puesto] from [Puestos]";
            SqlCommand comando = new SqlCommand(query, conexion);

            SqlDataReader reader = comando.ExecuteReader();
            while (reader.Read())
            {
                cbPuesto.Items.Add(reader["Puesto"].ToString());
                cbPuestoParameters.Items.Add(reader["Puesto"].ToString());
                cbPuestoUpdate.Items.Add(reader["Puesto"].ToString());
                
            }

            conexion.Close();
        }

        private void BtnConsultaStoreDgv_Click(object sender, EventArgs e)
        {
            conexion.Open();
            {
                SqlCommand cmd = new SqlCommand("ConsultaEmpleados", conexion);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add("@Filtro",SqlDbType.VarChar);

                if (rbCE.Checked == true)
                {
                    cmd.Parameters["@Filtro"].Value = "nada";
                }
                else if (rbEMV.Checked == true)
                {
                    cmd.Parameters["@Filtro"].Value = "viejos";
                }

                //Llenado del DataTable
                cmd.ExecuteNonQuery();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                
                dgvStoreDgv.DataSource = dt;
                //dgvStoreDgv.Dock = DockStyle.Fill;
                
            }
            conexion.Close();
            dgvStoreDgv.ReadOnly = true;
            dgvStoreDgv.AllowUserToAddRows = false;
            lbRowsCount.Text = dgvStoreDgv.Rows.Count.ToString();
            
            
            
        }

        private void DgvStoreDgv_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int i = dgvStoreDgv.CurrentCell.RowIndex;
            dgvStoreDgv.Rows[i].Selected = true;
            
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            txtIdOficialStoreParamIn.MaxLength = 10;
            {
                SqlCommand cmd = new SqlCommand("EmpleadosWhere", conexion);

                //specify that it is a sored procedure and not a normal proc
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                //list the parameters required and what they should be
                cmd.Parameters.AddWithValue("@IdOficial", txtIdOficialStoreParamIn.Text);

                conexion.Open();
                cmd.ExecuteNonQuery();
                conexion.Close();

                using (SqlDataAdapter adap = new SqlDataAdapter(cmd))
                {
                    DataTable dt = new DataTable();
                    adap.Fill(dt);
                    dgvStoreParamIn.DataSource = dt;
                }

                dgvStoreParamIn.ReadOnly = true;
                dgvStoreParamIn.AllowUserToAddRows = false;
                lbRowsCountdgvStoredParamIn.Text = dgvStoreParamIn.Rows.Count.ToString();
                dgvStoreParamIn.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells;

            }
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            int SPResult = 0;
            string IdOficial = txtIN.Text;

            SqlCommand cmd = new SqlCommand("EmpleadosCount", conexion);
            
            //Specify it is SP
            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            //List parameters required and what they should be
            cmd.Parameters.AddWithValue("@IdOficial", IdOficial);

            //Set up parameters
            cmd.Parameters.Add("@Result", SqlDbType.Int);
            cmd.Parameters["@Result"].Direction = ParameterDirection.Output;

            // set parameters value
            // cmd.Parameters["@Result"].Value = SPResult;

            conexion.Open();
            cmd.ExecuteNonQuery();

            cmd.ExecuteNonQuery();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            //Desplegar solo columna en especifico

            


            dataGridView1.DataSource = dt;

            //this.dataGridView1.Columns["NombreCompleto"].Visible = false;

            dataGridView1.ReadOnly = true;
            dataGridView1.AllowUserToAddRows = false;

            conexion.Close();

            SPResult = Int16.Parse(cmd.Parameters["@Result"].Value.ToString());
            if(txtIN.Text == "")
            {
                MessageBox.Show("Favor de ingresar un valor a consultar");
            }
            else
            {
                if (SPResult == 0)
                {
                    MessageBox.Show("No existe un empleado con el IDOficial ingresado");
                    txtIN.Text = "";
                    pictureBox1.Image = Image.FromFile("C:\\Users\\AbelFH\\Pictures\\bad.jpg");

                }
                else
                {
                    MessageBox.Show("Si existe el empleado: " + IdOficial + System.Environment.NewLine+ "Total de Registro:" + SPResult);
                    txtIN.Text = "";
                    pictureBox1.Image = Image.FromFile("C:\\Users\\AbelFH\\Pictures\\good.jpg");

                }
            }
            

        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView1.SelectedCells.Count > 0)
            {
                
                int rowindex = dataGridView1.CurrentCell.RowIndex;
                int columnindex = dataGridView1.CurrentCell.ColumnIndex;

                Dato.Text = dataGridView1.Rows[rowindex].Cells[columnindex].Value.ToString();


            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            try
            {
                SqlCommand cmd = new SqlCommand("INSERT INTO [Productos](Imagen) VALUES (@Imagen)", conexion);
                String strFilePath = @"C:\good.jpg";

                //Read jpg file stream and from there into Byte Array
                FileStream fsFile = new FileStream(strFilePath, FileMode.Open, FileAccess.Read);
                Byte[] bytData = new byte[fsFile.Length];
                fsFile.Read(bytData, 0, bytData.Length);
                fsFile.Close();

                //Create Parameter for insert command and add to SQLCommand Object
                SqlParameter prm = new SqlParameter("@Imagen", SqlDbType.VarBinary, bytData.Length, ParameterDirection.Input,
                false, 0, 0, null, DataRowVersion.Current, bytData);
                cmd.Parameters.Add(prm);


                //Open connection, execute query and close connection.
                conexion.Open();
                cmd.ExecuteNonQuery();
                conexion.Close();
                MessageBox.Show("Imagen ha sido capturada en la Base de Datos", "Aviso", MessageBoxButtons.OK);


            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString(), "Advertencia", MessageBoxButtons.OK);
            }
        }

        private void button2_Click_2(object sender, EventArgs e)
        {
            try
            {
                conexion.Open();
                //ASC is default on SQL Database Select
                SqlCommand cmd = new SqlCommand("SELECT [IdProductos], [Imagen] FROM [Productos] ORDER BY [IdProductos] DESC", conexion);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                da.Fill(ds, "dt");
                int c = ds.Tables["dt"].Rows.Count;

                if (c > 0)
                {
                    Byte[] byteData = new byte[0];
                    byteData = (Byte[])(ds.Tables["dt"].Rows[c - 1]["Imagen"]);
                    MemoryStream stmData = new MemoryStream(byteData);
                    pictureBox2.Image = Image.FromStream(stmData);
                }
                conexion.Close();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString(), "Advertencia", MessageBoxButtons.OK);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                SqlCommand cmd = new SqlCommand("INSERT INTO [Productos](Imagen) VALUES(@Imagen)", conexion);

                MemoryStream ms = new MemoryStream();
                pictureBox2.Image.Save(ms, ImageFormat.Jpeg);

                Byte[] bytData = new Byte[ms.Length];
                ms.Position = 0;
                ms.Read(bytData, 0, Convert.ToInt32(ms.Length));

                SqlParameter prm = new SqlParameter("@Imagen", SqlDbType.VarBinary, bytData.Length,
                    ParameterDirection.Input, false, 0, 0, null, DataRowVersion.Current, bytData);

                cmd.Parameters.Add(prm);
                conexion.Open();
                cmd.ExecuteNonQuery();
                conexion.Close();
                MessageBox.Show("Imagen ha sido capturada en la Base de Datos", "Aviso", MessageBoxButtons.OK);

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString(), "Advertencia", MessageBoxButtons.OK);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            //Open File Dialog
            OpenFileDialog open = new OpenFileDialog();
            //Image Filters
            open.Filter = "Image Files(*.jpg;*.jpeg;*.gif;*.bmp)|*.jpg;*.jpeg;*.gif;*.bmp";
            if (open.ShowDialog() == DialogResult.OK)
            {

                pictureBox2.Image = new Bitmap(open.FileName);
                //Open File Name gives us the PATH
                textBox1.Text = open.FileName;
                path = open.FileName;

            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            //ESTA MAL - NO FUE CORRECTAMENTE IMPLEMENTADO
            try
            {
                string parte = txtNumParte.Text;
                string descrip = txtDescripcion.Text;
                string emple = txtEmpleado.Text;
                int canti = Convert.ToInt32(nudCantidad.Value);
                DateTime fecha = dateTimePicker1.Value;
                Image test;

                if (textBox1.Text != "")
                {
                    try
                    {
                        SqlCommand cmd2 = new SqlCommand("INSERT INTO [Productos](Imagen) VALUES (@Imagen)", conexion);
                        String strFilePath = path;

                        //Read jpg file stream and from there into Byte Array
                        FileStream fsFile = new FileStream(strFilePath, FileMode.Open, FileAccess.Read);
                        Byte[] bytData = new byte[fsFile.Length];
                        fsFile.Read(bytData, 0, bytData.Length);
                        fsFile.Close();

                        //Create Parameter for insert command and add to SQLCommand Object
                        SqlParameter prm = new SqlParameter("@Imagen", SqlDbType.VarBinary, bytData.Length, ParameterDirection.Input,
                        false, 0, 0, null, DataRowVersion.Current, bytData);
                        cmd2.Parameters.Add(prm);


                        //Open connection, execute query and close connection.
                        conexion.Open();
                        cmd2.ExecuteNonQuery();
                        conexion.Close();
                        MessageBox.Show("Imagen ha sido capturada en la Base de Datos", "Aviso", MessageBoxButtons.OK);


                    }
                    catch (Exception ex)
                    {

                        MessageBox.Show(ex.ToString(), "Advertencia", MessageBoxButtons.OK);
                    }
                }
                //string query = "InsertarProducto '" + parte + "', '" + descrip + "', '" + emple + "', '" + canti + "', '" + fecha + "', '" + test + "'";



                SqlCommand cmd = new SqlCommand();
                //cmd.CommandText = query;
                cmd.CommandTimeout = 0;
                cmd.Connection = conexion;
                //cmd.CommandType = CommandType.StoredProcedure;

                conexion.Open();

                cmd.ExecuteNonQuery();
                MessageBox.Show("Captura Realizada en la Base de Datos", "Aviso", MessageBoxButtons.OK);
                conexion.Close();

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString(), "Advertencia", MessageBoxButtons.OK);
            }

        }
    }
}
