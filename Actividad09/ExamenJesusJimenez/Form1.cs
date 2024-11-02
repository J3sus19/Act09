using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace Actividad09
{

    public partial class Form1 : Form
    {
        
        public SerialPort ArduinoPort { get; }
        private string connectionString = "server=localhost;database=Formulario;uid:root;pwd=1993";

        public Form1()
        {

            InitializeComponent();

            ArduinoPort = new SerialPort("COM4", 9600);
            ArduinoPort.DataReceived += new SerialDataReceivedEventHandler(DataReceivedHandler);
            ArduinoPort.Open();

        }


        private void btnGuardar_Click(object sender, EventArgs e)
        {
            string nombre = txtNombre.Text;
            string fecha = txtFecha.Text;

            // validar los formatos
            if (string.IsNullOrWhiteSpace(nombre) || string.IsNullOrWhiteSpace(fecha))
            {
                MessageBox.Show("Ingresar todos los datos", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                // Mensaje de datos guardados
                MessageBox.Show("Datos guardados :)", "Bien hecho", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnBorrar_Click(object sender, EventArgs e)
        {
            txtNombre.Clear();
            txtFecha.Clear();

        }


        // Manejador de eventos para la validación del formato 
        // utilizando Leave
        private void txtNombre_Leave(object sender, EventArgs e)
        {
            // Validación del textbox "nombre"
            if (string.IsNullOrWhiteSpace(txtNombre.Text))
            {
                MessageBox.Show("El campo de nombre no puede estar vacío.", "Alto ahí", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtNombre.Focus();  
            }

        }

       
        private void txtFecha_Leave(object sender, EventArgs e)
        {
           
            DateTime fecha;
            if (!DateTime.TryParseExact(txtFecha.Text, "dd/MM/yyyy", null, System.Globalization.DateTimeStyles.None, out fecha))
            {
                MessageBox.Show("Ingrese la fecha en formato dd/mm/yyyy.", "Formato incorrecto :c", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtFecha.Focus();
            }
        }
        private void DataReceivedHandler(object sender, SerialDataReceivedEventArgs e)
        {
            string data = ArduinoPort.ReadLine().Trim();

            
            string[] temperatures = data.Split(',');

            if (temperatures.Length == 2)
            {
               
                lblTemperatureCelsius.Invoke((MethodInvoker)(() =>
                {
                    lblTemperatureCelsius.Text = "Temperatura (°C): " + temperatures[0];
                }));

        
                lblTemperatureFahrenheit.Invoke((MethodInvoker)(() =>
                {
                    lblTemperatureFahrenheit.Text = "Temperatura (°F): " + temperatures[1];
                }));
            }
        }

        private void txtNombre_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtNombre.Text))
            {
              
                MessageBox.Show("El campo Nombre no puede estar vacío.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }


        }

        private void servON_Click(object sender, EventArgs e)
        {
            if (ArduinoPort.IsOpen)
            {
                ArduinoPort.Write("e"); 
            }
            else
            {
                MessageBox.Show("El puerto serial no está abierto.");
            }


        }

        private void servOFF_Click(object sender, EventArgs e)
        {

        }

        private void txtApellidos_TextChanged(object sender, EventArgs e)
        {
            // Validación del campo
            if (string.IsNullOrWhiteSpace(txtApellidos.Text))
            {
                
                MessageBox.Show("El campo Apellidos no puede estar vacío.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

        }

        private void txtEstatura_TextChanged(object sender, EventArgs e)
        {
           
            if (string.IsNullOrWhiteSpace(txtEstatura.Text))
            {
                MessageBox.Show("El campo Estatura no puede estar vacío.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                if (!decimal.TryParse(txtEstatura.Text, out _))
                {
                    MessageBox.Show("Por favor, ingrese un valor numérico válido para la estatura.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

        private void txtTelefono_TextChanged(object sender, EventArgs e)
        {
            
            if (string.IsNullOrWhiteSpace(txtTelefono.Text))
            {
                MessageBox.Show("El campo Teléfono no puede estar vacío.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
       
            else if (txtTelefono.Text.Length > 13)
            {
                MessageBox.Show("El número de teléfono no puede exceder los 13 dígitos.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

        }
    }
}
