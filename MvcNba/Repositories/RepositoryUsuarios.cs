using System.Linq;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using MvcNba.Helpers;
using MvcNba.Models;
using MvcNba.Data;

namespace MvcNba.Repositories
{
    public class RepositoryUsuarios
    {
        private NbaContext context;

        public RepositoryUsuarios(NbaContext context)
        {
            this.context = context;
        }

        public async Task<bool> LogInUserAsync(string nombre, string password)
        {
            var usuario = await context.Usuarios.FirstOrDefaultAsync(u => u.Nombre == nombre);

            if (usuario != null)
            {
                string salt = usuario.Salt;
                byte[] temp = HelperCryptography.EncryptPassword(password, salt);
                byte[] passUser = usuario.Password;
                bool response = HelperTools.CompareArrays(temp, passUser);

                return response;
            }
            else
            {
                return false;
            }
        }
        public Usuario GetUser(string username)
        {
            var usuario = (from u in context.Usuarios
                           where u.Nombre == username
                           select u).FirstOrDefault();

            return usuario;
        }
        public List<Usuario> GetUsuarios()
        {
            var consulta = from datos in context.Usuarios
                           select datos;
            return consulta.ToList();
        }
        public bool UsuarioExists(string username)
        {
            var consulta = from u in context.Usuarios
                           where u.Nombre == username
                           select u;

            return consulta.Any();
        }
        public bool EmailExists(string email)
        {
            var consulta = from u in context.Usuarios
                           where u.Email == email
                           select u;

            return consulta.Any();
        }
        private async Task<int> GetMaxIdUsuarioAsync()
        {
            if (this.context.Usuarios.Count() == 0)
            {
                return 1;
            }
            else
            {
                return await this.context.Usuarios.MaxAsync(z => z.IdUsuario) + 1;
            }
        }
        public async Task<Usuario> RegisterUserAsync(string nombre, string password, string email)
        {
            Usuario user = new Usuario();
            user.IdUsuario = await this.GetMaxIdUsuarioAsync();
            user.Nombre = nombre;
            user.Email = email;
            user.Comentario = "";
            user.FotoPerfil = "";
            user.Rol = 2;
            //CADA USUARIO TENDRA UN SALT DISTINTO
            user.Salt = HelperTools.GenerateSalt();
            //GUARDAMOS EL PASSWORD EN BYTE[]
            user.Password = HelperCryptography.EncryptPassword(password, user.Salt);

            this.context.Usuarios.Add(user);
            await this.context.SaveChangesAsync();
            return user;
        }
    }
}