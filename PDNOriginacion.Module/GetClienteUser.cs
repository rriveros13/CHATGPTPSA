using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using PDNOriginacion.Module.BusinessObjects;
using System;

namespace PDNOriginacion.Module
{
    public class GetClienteUser : ICustomFunctionOperator
    {
        static GetClienteUser()
        {
            GetClienteUser instance = new GetClienteUser();
            if(CriteriaOperator.GetCustomFunction(instance.Name) == null)
            {
                CriteriaOperator.RegisterCustomFunction(instance);
            }
        }

        public object Evaluate(params object[] operands)
        {
            Usuario usr = (Usuario)SecuritySystem.CurrentUser;
            return usr.Cliente?.Nombre;
        }
        public static void Register()
        {
        }
        public Type ResultType(params Type[] operands) => typeof(string);

        public string Name => nameof(GetClienteUser);
    }
}
