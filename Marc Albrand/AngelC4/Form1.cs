using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace AngelC4
{
    public partial class Form1 : Form
    {
        bool CincoSegundos = false;
        bool Open = false;
        object senderr;
        EventArgs ee;
        private SqlConnection conexion = new SqlConnection("server=MARC ; database=TestDB ; integrated security = true");
        public Form1()
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

        private void BtnConsultaStoreParamOut_Click(object sender, EventArgs e)
        {
            int SPResult = 0;
            string IdOficial = txtIdOficialStoreParamOut.Text;

            SqlCommand cmd = new SqlCommand("EmpleadosCount", conexion);

            //Specify that it is a storedprocedure and not a normal procedure
            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            //list the parameters required and what they should be
            cmd.Parameters.AddWithValue("@NombreCompleto", IdOficial);

            //Set up the parameters
            cmd.Parameters.Add("@Result", SqlDbType.Int);
            cmd.Parameters["@Result"].Direction = ParameterDirection.Output;

            // set parameter values
            //cmd.Parameters["@Result]".Value = SPResult;
            

            conexion.Open();
            cmd.ExecuteNonQuery();
            conexion.Close();

            SPResult = Int16.Parse(cmd.Parameters["@Result"].Value.ToString());

            using (SqlDataAdapter adap = new SqlDataAdapter(cmd))
            {
                DataTable dt = new DataTable();
                adap.Fill(dt);
                dt.Columns[0].ColumnMapping = MappingType.Hidden;
                dt.Columns[1].ColumnMapping = MappingType.Hidden;
                dt.Columns[3].ColumnMapping = MappingType.Hidden;
                dt.Columns[4].ColumnMapping = MappingType.Hidden;
                dgvStoreParamOut.DataSource = dt;

                foreach (DataRow row in dt.Rows)
                {
                    //string empleado = dt[0][""];
                }
            }

            

            dgvStoreParamOut.ReadOnly = true;
            dgvStoreParamOut.AllowUserToAddRows = false;
            lbRowsCountdgvStoredParamOut.Text = dgvStoreParamOut.Rows.Count.ToString();
            dgvStoreParamOut.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells;

            if (SPResult == 0)
            {
                MessageBox.Show("No existe un empleado con el IdOficial ingresado");
                pbStoreParamOut.Image = AngelC4.Properties.Resources.CaritaTriste;
            }
            else
            {
                //MessageBox.Show("Si existe el empleado: " + IdOficial + ", Total de registros: " + SPResult);
                pbStoreParamOut.Image = AngelC4.Properties.Resources.CaritaFeliz;
            }
            pbStoreParamOut.SizeMode = PictureBoxSizeMode.StretchImage;
            
            


        }

        private void DgvStoreParamOut_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int row = dgvStoreParamOut.CurrentCell.RowIndex;
            int column = dgvStoreParamOut.CurrentCell.ColumnIndex;

            string value = dgvStoreParamOut.Rows[row].Cells[column].Value.ToString();

            //IdEmpleado
            string IdEmpleado = dgvStoreParamOut.Rows[row].Cells[0].Value.ToString();
            //IdOficial
            string IdOficial = dgvStoreParamOut.Rows[row].Cells[1].Value.ToString();
            //NombreCompleto
            string NombreCompleto = dgvStoreParamOut.Rows[row].Cells[2].Value.ToString();
            //Puesto
            string Puesto = dgvStoreParamOut.Rows[row].Cells[3].Value.ToString();
            //FechaNacimiento
            string FechaNacimiento = dgvStoreParamOut.Rows[row].Cells[4].Value.ToString();

            txtIdEmpleadoStoreParamOut.Text = IdEmpleado;
            txtIdOficial1StoreParamOut.Text = IdOficial;
            txtNombreCompletoStoreParamOut.Text = NombreCompleto;
            txtPuestoStoreParamOut.Text = Puesto;
            txtFechaDeNacimientoStoreParamOut.Text = FechaNacimiento;
            lbInfoDeCeldaStoreParamOut.Text = value;

        }

        private void btnFileToDBImagen_Click(object sender, EventArgs e)
        {
            try
            {
                SqlCommand cmd = new SqlCommand("INSERT INTO [Productos] (Imagen) VALUES (@Imagen)", conexion);
                string strFilePath = @"C:\happy.jpg"; //Modify this path as needed.

                //Read jpg into file stream, and from there into Byte array.
                FileStream fsFile = new FileStream(strFilePath, FileMode.Open, FileAccess.Read);
                Byte[] bytData = new Byte[fsFile.Length];
                fsFile.Read(bytData, 0, bytData.Length);
                fsFile.Close();

                //Create parameter for insert command and add to SqlCommand object
                SqlParameter prm = new SqlParameter("@Imagen", SqlDbType.VarBinary, bytData.Length,
                    ParameterDirection.Input, false, 0, 0, null, DataRowVersion.Current, bytData);
                cmd.Parameters.Add(prm);

                //Open connection, execute query, and close connection.
                conexion.Open();
                cmd.ExecuteNonQuery();
                conexion.Close();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnDbToPictureBoxImagen_Click(object sender, EventArgs e)
        {
            try
            {
                conexion.Open();

                //Retrieve from database into DataSet.
                SqlCommand cmd = new SqlCommand("SELECT [idProductos], [Imagen] FROM [Productos] ORDER BY [idProductos]", conexion);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                da.Fill(ds, "dt");
                int c = ds.Tables["dt"].Rows.Count;
                

                if (c > 0)
                {
                    //Read into Byte array, then used to construct MemoryStream,
                    //then passed to PictureBox.
                    Byte[] byteData = new byte[0];
                    byteData = (Byte[])(ds.Tables["dt"].Rows[c - 1]["Imagen"]);
                    MemoryStream stmData = new MemoryStream(byteData);
                    pbImagen.Image = Image.FromStream(stmData);
                }
                conexion.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
    }
}
