using DevExpress.Data.Filtering;
using PDNOriginacion.Module.BusinessObjects;
using System;
using System.Linq;

namespace PDNOriginacion.Module.Helpers
{
    class CampoEnProducto : ICustomFunctionOperator
    {
        static CampoEnProducto()
        {
            CampoEnProducto instance = new CampoEnProducto();
            if(CriteriaOperator.GetCustomFunction(instance.Name) == null)
            {
                CriteriaOperator.RegisterCustomFunction(instance);
            }
        }

        public object Evaluate(params object[] operands)
        {
            string campo = (string)operands[0];
            Producto prod = (Producto)operands[1];

            return prod != null && prod.Campos.Any(c => c.Campo.Nombre == campo);
        }
        public static void Register()
        {
        }
        public Type ResultType(params Type[] operands) => typeof(bool);

        public string Name => nameof(CampoEnProducto);
    }
}
