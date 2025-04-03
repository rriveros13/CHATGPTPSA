using DevExpress.Data.Filtering;
using PDNOriginacion.Module.BusinessObjects;
using System;

namespace PDNOriginacion.Module.Helpers
{
    class EvalCriterioValidacion : ICustomFunctionOperator
    {
        static EvalCriterioValidacion()
        {
            EvalCriterioValidacion instance = new EvalCriterioValidacion();
            if(CriteriaOperator.GetCustomFunction(instance.Name) == null)
            {
                CriteriaOperator.RegisterCustomFunction(instance);
            }
        }

        public object Evaluate(params object[] operands)
        {
            string nombre = (string)operands[0];
            Solicitud sol = (Solicitud)operands[1];

            if(sol?.Producto == null)
            {
                return false;
            }

            foreach(CampoProducto campo in sol.Producto.Campos)
            {
                if(campo.Campo.Nombre != nombre)
                {
                    continue;
                }

                if(string.IsNullOrEmpty(campo.CriterioVis))
                {
                    return true;
                }

                return (bool)sol.Evaluate(campo.CriterioVal);
            }

            return false;
        }
        public static void Register()
        {
        }
        public Type ResultType(params Type[] operands) => typeof(bool);

        public string Name => nameof(EvalCriterioValidacion);
    }
}
