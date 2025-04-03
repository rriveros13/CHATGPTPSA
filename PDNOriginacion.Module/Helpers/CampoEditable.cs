using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using PDNOriginacion.Module.BusinessObjects;
using System;
using System.Linq;

namespace PDNOriginacion.Module.Helpers
{
    class CampoEditable : ICustomFunctionOperator
    {
        static CampoEditable()
        {
            CampoEditable instance = new CampoEditable();
            if(CriteriaOperator.GetCustomFunction(instance.Name) == null)
            {
                CriteriaOperator.RegisterCustomFunction(instance);
            }
        }

        private static object VerificarCampo(string campo, Solicitud sol)
        {
            if(sol.Estado != null)
            {
                if(sol.Estado.NoPermitirCambios)
                {
                    return false;
                }
            }

            Usuario currentUser = sol.Session.GetObjectByKey<Usuario>(SecuritySystem.CurrentUserId);
            Tarea ta = sol.Tareas.FirstOrDefault(t => t?.ReservadaPor == currentUser);
            if(sol.Oid != -1)
            {
                if(ta == null)
                {
                    return false;
                }
            }

            CampoProducto cp = sol.Producto.Campos
                .FirstOrDefault(c => c.Campo.Nombre == campo && !string.IsNullOrEmpty(c.CriterioEditable));
            return cp == null ? true : sol.Evaluate(cp.CriterioEditable);
        }

        public object Evaluate(params object[] operands)
        {
            string campo = (string)operands[0];
            Solicitud solicitud = (Solicitud)operands[1];
            return solicitud.Producto == null ? false : VerificarCampo(campo, solicitud);   
        }
        public static void Register()
        {
        }
        public Type ResultType(params Type[] operands) => typeof(bool);

        public string Name => nameof(CampoEditable);
    }
}
