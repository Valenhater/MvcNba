﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MvcNba.Models
{
    [Table("Usuarios")]
    public class Usuario
    {
        [Key]
        [Column("idusuario")]
        public int IdUsuario { get; set; }
        [Column("nombre")]
        public string Nombre { get; set; }
        [Column("passwd")]
        public byte[] Password { get; set; }
        [Column("salt")]
        public string Salt { get; set; }
        [Column("email")]
        public string Email { get; set; }
        [Column("comentario")]
        public string Comentario { get; set; }
        [Column("foto_perfil")]
        public string FotoPerfil { get; set; }
        [ForeignKey("Rol")]
        public int Rol { get; set; }        
    }
}