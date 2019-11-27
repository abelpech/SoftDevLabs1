using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;

using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Drawing.Imaging;

namespace AngelC4
{
    public partial class Form1 : Form
    {
        bool CincoSegundos = false;
        bool Open = false;
        object senderr;
        EventArgs ee;
        private SqlConnection conexion = new SqlConnection("server=DESKTOP-IH0ESEJ\\SQLEXPRESS ; database=TestDB ; integrated security = true");
        public Form1()
        {
            InitializeComponent();
            this.WindowState = FormWindowState.Maximized;
            tabControl1.SelectedTab = tabPage9;
            BtnSelect_Click(senderr, ee);
           
            EstablecerComboBox();
            EstablecerComboBox2();
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
            int Puesto = Convert.ToInt32(cbPuesto.SelectedIndex );
          //  Puesto =+ 1;
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
            string query = "SELECT [Nombre] from [Puesto]";
            SqlCommand comando = new SqlCommand(query, conexion);

            SqlDataReader reader = comando.ExecuteReader();
            while (reader.Read())
            {
                cbPuesto.Items.Add(reader["Nombre"].ToString());
                cbPuestoParameters.Items.Add(reader["Nombre"].ToString());
                cbPuestoUpdate.Items.Add(reader["Nombre"].ToString());
                
            }

            conexion.Close();
        }



        public void EstablecerComboBox2()
        {
            cbPuesto.Items.Clear();
            
            conexion.Open();

            //cbPuestoParameters.Items.Add("Hola");
            string query = "SELECT [NombreCompleto] from [Empleados]";
            SqlCommand comando = new SqlCommand(query, conexion);

            SqlDataReader reader = comando.ExecuteReader();
            while (reader.Read())
            {
                cb1.Items.Add(reader["NombreCompleto"].ToString());
                

            }

            conexion.Close();
        }

        private void BtnConsulta_Click(object sender, EventArgs e)
        {
            conexion.Open();
            {
                this.dgv.EditMode = DataGridViewEditMode.EditProgrammatically;
                this.dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;


                if (rbnormal.Checked == true)
                {
                    SqlCommand cmd = new SqlCommand("ConsultaEmpleado", conexion);

                    cmd.CommandType = CommandType.StoredProcedure;

                    //llenado de data table

                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();

                    da.Fill(dt);

                    dgv.DataSource = dt;
                    //dgv.Dock = DockStyle.Fill;

                }

                else if(rbfecha.Checked == true)
                {

                    SqlCommand cmd = new SqlCommand("ConsultaEmpleadoFechas", conexion);

                    cmd.CommandType = CommandType.StoredProcedure;

                    //llenado de data table

                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();

                    da.Fill(dt);

                    dgv.DataSource = dt;
                    //dgv.Dock = DockStyle.Fill;

                }








            }
            conexion.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            txyidoficial.MaxLength = 10;

            SqlCommand cmd = new SqlCommand("EmpleadosWhere", conexion);

            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@IdOficial", txyidoficial.Text);

            conexion.Open();
            cmd.ExecuteNonQuery();
            conexion.Close();

            using( SqlDataAdapter adap = new SqlDataAdapter(cmd))
            {
                DataTable dt = new DataTable();
                adap.Fill(dt);
                dgvstore.DataSource = dt;
            }
            label26.Text = dgvstore.RowCount.ToString();


        }

        private void button3_Click(object sender, EventArgs e)
        {
            pbcorrecto.Visible = false;
            pbincorrecto.Visible = false;

            int SPResult = 0;
            string IdOficial = txtparam.Text;

            SqlCommand cmd = new SqlCommand("EmpleadosCount", conexion);

            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@IdOficial", IdOficial);
            cmd.Parameters.Add("@Result", SqlDbType.Int);
            cmd.Parameters["@Result"].Direction = ParameterDirection.Output;
            conexion.Open();
            cmd.ExecuteNonQuery();
            conexion.Close();

            SPResult = Int16.Parse(cmd.Parameters["@Result"].Value.ToString());

            if(SPResult == 0)
            {
                MessageBox.Show("No existe un empleado con el IdOficial ingresado");
                pbincorrecto.Visible = true;
            }
            else
            {
                MessageBox.Show(" Si existe un empleado con el IdOficial: " + IdOficial + " Numero de Usuarios: " + SPResult);
                pbcorrecto.Visible = true;

                using (SqlDataAdapter adap = new SqlDataAdapter(cmd))
                {
                    DataTable dt = new DataTable();
                    adap.Fill(dt);
                    dgvparam.DataSource = dt;

                    this.dgvparam.Columns["FechaNacimiento"].Visible = false;
                    this.dgvparam.Columns["Nombre"].Visible = false;
                    this.dgvparam.Columns["IdOficial"].Visible = false;
                }
            }


        }

        private void label27_Click(object sender, EventArgs e)
        {

        }

        private void dgvparam_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            label29.Text = dgvparam.CurrentRow.Cells[0].Value.ToString();

            //  this.dgvparam.Columns("NombreColumna").Visible = False;

            

            txtparamnombre.Text = dgvparam.CurrentRow.Cells[0].Value.ToString();
            txtparamif.Text = dgvparam.CurrentRow.Cells[1].Value.ToString();
            txtparampuesto.Text = dgvparam.CurrentRow.Cells[2].Value.ToString();
            txtparamfecha.Text = dgvparam.CurrentRow.Cells[3].Value.ToString();



        }

        private void btnfile2db_Click(object sender, EventArgs e)
        {


            try
            {

                SqlCommand cmd = new SqlCommand("INSERT INTO [Productos] (Imagen) VALUES (@Imagen) ", conexion);

                String strFilePath = @"C:\3.jpg";// Modify this path as needed

                FileStream fsFile = new FileStream(strFilePath, FileMode.Open, FileAccess.Read);
                Byte[] byData = new byte[fsFile.Length];
                fsFile.Read(byData, 0, byData.Length);
                fsFile.Close();


                SqlParameter prm = new SqlParameter("@Imagen", SqlDbType.VarBinary, byData.Length, ParameterDirection.Input, false, 0, 0, null, DataRowVersion.Current, byData);
                cmd.Parameters.Add(prm);

                conexion.Open();
                cmd.ExecuteNonQuery();
                conexion.Close();


            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }



        }

        private void button5_Click(object sender, EventArgs e)
        {

            try
            {

                conexion.Open();
                SqlCommand cmd = new SqlCommand("SELECT [IdImagen],[Imagen] FROM [Productos] ORDER BY [IdImagen] DESC", conexion);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                da.Fill(ds, "dt");
                int c = ds.Tables["dt"].Rows.Count;

                if (c > 0)
                {
                    Byte[] byData = new byte[0];
                    byData = (Byte[])(ds.Tables["dt"].Rows[c - 1]["Imagen"]);
                    MemoryStream stmData = new MemoryStream(byData);
                    pictureBox1.Image =Image.FromStream(stmData);
                }
                conexion.Close();


            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);


            }




        }

        private void button4_Click(object sender, EventArgs e)
        {
            try
            {

                conexion.Open();
                SqlCommand cmd = new SqlCommand("SELECT [IdImagen],[Imagen] FROM [Productos] ORDER BY [IdImagen] ASC", conexion);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                da.Fill(ds, "dt");
                int c = ds.Tables["dt"].Rows.Count;

                if (c > 0)
                {
                    Byte[] byData = new byte[0];
                    byData = (Byte[])(ds.Tables["dt"].Rows[c - 1]["Imagen"]);
                    MemoryStream stmData = new MemoryStream(byData);
                    pictureBox1.Image = Image.FromStream(stmData);
                }
                conexion.Close();


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);


            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            try
            {
                SqlCommand cmd = new SqlCommand("INSERT INTO [Productos] (Imagen)  VALUES (@Imagen)", conexion);



                MemoryStream ms = new MemoryStream();
                pictureBox1.Image.Save(ms, ImageFormat.Jpeg);
                Byte[] byData = new byte[ms.Length];
                ms.Position = 0;
                ms.Read(byData, 0, Convert.ToInt32(ms.Length));


                SqlParameter prm = new SqlParameter("@Imagen", SqlDbType.VarBinary, byData.Length,
                ParameterDirection.Input, false,
                0, 0, null, DataRowVersion.Current, byData);
                cmd.Parameters.Add(prm);
                conexion.Open();
                cmd.ExecuteNonQuery();
                conexion.Close();



            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            //Abir el File Dialog
            OpenFileDialog open = new OpenFileDialog();
            //Filtro para las imagenes
            open.Filter = "Image Files(*.jpg; *.jpeg;*.gif; *.bmp|*.jpg; *.jpeg;*.gif; *.bmp; ";

            if(open.ShowDialog() == DialogResult.OK)
            {   //se agrega la imagen a el Picture Box
                pictureBox1.Image = new Bitmap(open.FileName);
                //Se agrega la ruta de la foto
                textBox1.Text = open.FileName;
            }

        }

        private void tabPage6_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button8_Click(object sender, EventArgs e)
        {
            OpenFileDialog open = new OpenFileDialog();
            SqlCommand cmd = new SqlCommand("AgregarDatos", conexion);


            MemoryStream ms = new MemoryStream();
            pictureBox1.Image.Save(ms, ImageFormat.Jpeg);
            Byte[] byData = new byte[ms.Length];
            ms.Position = 0;
            ms.Read(byData, 0, Convert.ToInt32(ms.Length));


            SqlParameter prm = new SqlParameter("@Imagen", SqlDbType.VarBinary, byData.Length,
            ParameterDirection.Input, false,
            0, 0, null, DataRowVersion.Current, byData);



            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.Add(prm);
            
            cmd.Parameters.AddWithValue("@IdParte", txtparte.Text);
            cmd.Parameters.AddWithValue("@Empleado", (cb1.SelectedIndex + 1));
            cmd.Parameters.AddWithValue("@cantidad", txtcantidad.Text);
            cmd.Parameters.AddWithValue("@descripcion", txtdesc.Text);



            




            conexion.Open();
            cmd.ExecuteNonQuery();
            conexion.Close();

            MessageBox.Show("SI SE PUDO");
        }

        private void cbPuestoUpdate_SelectedIndexChanged(object sender, EventArgs e)
        {

            


            /*
             OpenFileDialog open = new OpenFileDialog();
             conexion.Open();

             string cadena = "INSERT into [Productos] (" +
                             "[Imagen], " +
                             "[IdParte], " +
                             "[Empleado], " +
                             "[cantidad], " +
                             "[descripcion]) " +
                             "values (" +
                             "@Imagen," +
                             "@IdParte," +
                             "@Empleado," +
                             "@cantidad" +
                             "@descripcion" +
                             ")";
             SqlCommand comando = new SqlCommand(cadena, conexion);

             //Llamamos al metodo add de la propiedad parameters del objeto SqlCommand en donde indicamos el parametro
             //y de que tipo de parametro
             comando.Parameters.Add("@Imagen", SqlDbType.Image);
             comando.Parameters.Add("@IdParte", SqlDbType.NVarChar);
             comando.Parameters.Add("@Empleado", SqlDbType.Int);
             comando.Parameters.Add("@cantidad", SqlDbType.Int);
             comando.Parameters.Add("@descripcion", SqlDbType.NVarChar);



             //Una vez los parametros creados inicializamos con los valores que estamos capturando y como subindice 
             //Podemos indicar el nombre del parametro
             comando.Parameters["@Imagen"].Value = pictureBox1.Image;
             comando.Parameters["@IdParte"].Value = txtparte.Text;
             comando.Parameters["@Empleado"].Value = (cbPuestoParameters.SelectedIndex + 1);
             comando.Parameters["@cantidad"].Value = txtcantidad.Text;
             comando.Parameters["@descripcion"].Value = txtdesc.Text;

             comando.ExecuteNonQuery();

             MessageBox.Show("Los datos se guardaron correctamente");

             txtIdOficialParameters.Text = "";
             txtNombreCompletoParameters.Text = "";

             conexion.Close();

     */


        }
    }
}
