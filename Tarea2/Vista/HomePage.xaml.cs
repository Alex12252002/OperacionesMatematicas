using System;
using Xamarin.Forms;
using Tarea2.Modelo;
using System.Collections.Generic;

namespace Tarea2.Vista
{
    public partial class HomePage : ContentPage
    {
        public HomePage()
        {
            InitializeComponent();
        }

        private async void BtnCalcular_Clicked(object sender, EventArgs e)
        {
            try
            {
                // Obtener los datos ingresados por el usuario
                double numero1 = Convert.ToDouble(EntryNumero1.Text);
                double numero2 = Convert.ToDouble(EntryNumero2.Text);
                string operacion = PickerOperacion.SelectedItem.ToString();

                // Calcular el resultado
                double resultado = RealizarOperacion(numero1, numero2, operacion);

                // Guardar la operación en la base de datos
                Operaciones nuevaOperacion = new Operaciones
                {
                    Fecha = DateTime.Now,
                    OperacionTipo = operacion,
                    Numero1 = numero1,
                    Numero2 = numero2,
                    Resultado = resultado
                };
                await App.DatabaseConexion.InsertarOperacion(nuevaOperacion);

                // Mostrar el resultado al usuario
                await DisplayAlert("Resultado", $"El resultado de la operación {operacion} es: {resultado}", "OK");

                // Recargar la lista de operaciones
                CargarOperaciones();
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Ocurrió un error al realizar la operación: {ex.Message}", "OK");
            }
        }

        private double RealizarOperacion(double numero1, double numero2, string operacion)
        {
            switch (operacion)
            {
                case "Suma":
                    return numero1 + numero2;
                case "Resta":
                    return numero1 - numero2;
                case "Multiplicación":
                    return numero1 * numero2;
                case "División":
                    if (numero2 == 0)
                        throw new DivideByZeroException("No se puede dividir por cero.");
                    return numero1 / numero2;
                default:
                    throw new ArgumentException("Operación no válida.");
            }
        }

        private async void CargarOperaciones()
        {
            try
            {
                List<Operaciones> operaciones = await App.DatabaseConexion.ObtenerOperaciones();

                // Limpiar la lista actual
                listaOperaciones.Children.Clear();

                // Agregar cada operación a la interfaz de usuario
                foreach (Operaciones operacion in operaciones)
                {
                    StackLayout stackLayout = new StackLayout();

                    Label lblIdFecha = new Label ();
                    lblIdFecha.Text = $"ID: {operacion.Id}, Fecha: {operacion.Fecha.ToString("g")}";

                    Label lblOperacion = new Label();
                    lblOperacion.Text = $"Operación: {operacion.OperacionTipo}, Número 1: {operacion.Numero1}, Número 2: {operacion.Numero2}, Resultado: {operacion.Resultado}";

                    Button btnEliminar = new Button();
                    btnEliminar.Text = "Eliminar";
                    btnEliminar.CommandParameter = operacion.Id; // Pasar el ID de la operación como parámetro del comando
                    btnEliminar.Clicked += BtnEliminar_Clicked; // Asignar el controlador de eventos

                    stackLayout.Children.Add(lblIdFecha);
                    stackLayout.Children.Add(lblOperacion);
                    stackLayout.Children.Add(btnEliminar);

                    listaOperaciones.Children.Add(stackLayout);
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Error al cargar operaciones: {ex.Message}", "OK");
            }
        }

        private async void BtnEliminar_Clicked(object sender, EventArgs e)
        {
            try
            {
                // Obtener el botón que desencadenó el evento
                Button btnEliminar = (Button)sender;

                // Obtener el StackLayout padre que contiene los detalles de la operación y el botón "Eliminar"
                StackLayout stackLayout = (StackLayout)btnEliminar.Parent;

                // Obtener el ID de la operación almacenado en el CommandParameter del botón
                int idOperacion = (int)btnEliminar.CommandParameter;

                // Eliminar la operación de la base de datos
                await App.DatabaseConexion.EliminarOperacion(idOperacion);

                // Remover el StackLayout que contiene los detalles de la operación y el botón "Eliminar" de la lista
                listaOperaciones.Children.Remove(stackLayout);
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Error al eliminar operación: {ex.Message}", "OK");
            }
        }
    }
}
