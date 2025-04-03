using DevExpress.Data.Filtering;
using PDNOriginacion.Module.BusinessObjects;
using System;
using System.Linq;

namespace PDNOriginacion.Module.Helpers
{
    internal class EvalCriterioMostrar : ICustomFunctionOperator
    {
        static EvalCriterioMostrar()
        {
            EvalCriterioMostrar instance = new EvalCriterioMostrar();
            if(CriteriaOperator.GetCustomFunction(instance.Name) == null)
            {
                CriteriaOperator.RegisterCustomFunction(instance);
            }
        }

        public object Evaluate(params object[] operands)
        {
            string nombre = (string)operands[0];
            Solicitud sol = (Solicitud)operands[1];

            CampoProducto cp = sol?.Producto?.Campos.FirstOrDefault(c => c.Campo.Nombre == nombre);

            if(cp == null)
            {
                return false;
            }

            if(!string.IsNullOrEmpty(cp.CriterioVis))
            {
                return (bool)sol.Evaluate(cp.CriterioVis);
            }
            return true;
        }
        public static void Register()
        {
        }
        public Type ResultType(params Type[] operands) => typeof(bool);

        public string Name => nameof(EvalCriterioMostrar);
    }
}
