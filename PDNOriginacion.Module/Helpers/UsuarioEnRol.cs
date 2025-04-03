using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.Xpo;
using DevExpress.Xpo.DB;
using PDNOriginacion.Module.BusinessObjects;
using System;
using System.Linq;
using System.Reflection;

namespace PDNOriginacion.Module.Helpers
{
    internal class UsuarioEnRol : ICustomFunctionOperatorFormattable, ICustomFunctionOperatorQueryable
    {
        private string rol;
        private Usuario u;

        static UsuarioEnRol()
        {
            UsuarioEnRol instance = new UsuarioEnRol();
            if(CriteriaOperator.GetCustomFunction(instance.Name) == null)
            {
                CriteriaOperator.RegisterCustomFunction(instance);
            }
        }

        MethodInfo ICustomFunctionOperatorQueryable.GetMethodInfo() => GetType().GetMethod(nameof(UsuarioEnRol));
        private string[] GetRoleNames(Usuario usu) => usu.RolesUsuario.Select(r => r.Name).ToArray();

        public object Evaluate(params object[] operands)
        {
            if(string.IsNullOrEmpty((string)operands[0]))
            {
                return false;
            }

            rol = (string)operands[0];
            u = (Usuario)SecuritySystem.CurrentUser;
            //return u.IsUserInRole(rol);

            return u.RolesUsuario.Any(r => r.Name == rol);
        }
        public string Format(Type providerType, params string[] operands)
        {
            Usuario newu = (Usuario)u.Clone();
            string[] roleNames = GetRoleNames(newu);
            if(roleNames.Length == 0)
            {
                return "FALSE";
            }

            if(operands.Length == 0)
            {
                return "FALSE";
            }

            string roleName = operands[0];
            if(typeof(MSSqlConnectionProvider).IsAssignableFrom(providerType))
            {
                return $"({roleName}) in ({string.Join(",", roleNames.Select(s => string.Concat("'", s, "'")))})";
            }
            throw new NotSupportedException(providerType.FullName);
        }
        public static void Register()
        {
        }
        public Type ResultType(params Type[] operands) => typeof(bool);

        public string Name => nameof(UsuarioEnRol);
    }
}
