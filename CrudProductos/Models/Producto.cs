using System.ComponentModel.DataAnnotations;

namespace CrudProductos.Models
{
    public class Producto
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre del producto es obligatorio")]
        [StringLength(100)]
        public string Nombre { get; set; } = string.Empty;

        [Required(ErrorMessage = "La descripción es obligatoria")]
        [StringLength(250)]
        public string Descripcion { get; set; } = string.Empty;

        [Range(0.01, 9999.99, ErrorMessage = "El precio debe ser mayor a cero")]
        public decimal Precio { get; set; }

        [Range(0, 10000, ErrorMessage = "El stock no puede ser negativo")]
        public int Stock { get; set; }

        public DateTime FechaRegistro { get; set; } = DateTime.UtcNow;
    }
}