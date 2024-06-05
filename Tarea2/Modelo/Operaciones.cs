using SQLite;
using System;

namespace Tarea2.Modelo
{
    public class Operaciones
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public DateTime Fecha { get; set; }

        public string OperacionTipo { get; set; }

        public double Numero1 { get; set; }

        public double Numero2 { get; set; }

        public double Resultado { get; set; }

        public double CalcularResultado()
        {
            switch (OperacionTipo)
            {
                case "Suma":
                    return Numero1 + Numero2;
                case "Resta":
                    return Numero1 - Numero2;
                case "Multiplicación":
                    return Numero1 * Numero2;
                case "División":
                    if (Numero2 == 0)
                        throw new DivideByZeroException("No se puede dividir por cero.");
                    return Numero1 / Numero2;
                default:
                    throw new ArgumentException("Operación no válida.");
            }
        }
    }
}
