using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MvcNba.Models
{
    [Table("RESERVAENTRADAS")]
    public class ReservaEntrada
    {
        [Key]
        [Column("RESERVAID")]
        public int ReservaId { get; set; }
        [Column("PARTIDOID")]
        public int PartidoId { get; set; }
        [Column("CANTIDADID")]
        public int CantidadId { get; set; }
        [Column("PRECIOTOTAL")]
        public decimal PrecioTotal { get; set; }
       
    }
}
