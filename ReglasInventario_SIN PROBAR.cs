using System;

namespace Prototipo
{
    public class ReglasInventario
    {
        public int ProductoID { get; set; }
        public int CantidadMinima { get; set; }
        public int ProveedorID { get; set; }

        public bool VerificarInventario(int stockDisponible)
        {
            return stockDisponible <= CantidadMinima;
        }
    }
}
