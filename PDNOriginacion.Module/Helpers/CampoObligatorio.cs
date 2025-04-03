using DevExpress.Data.Filtering;
using PDNOriginacion.Module.BusinessObjects;
using System;
using System.Drawing;
using System.Linq;

namespace PDNOriginacion.Module.Helpers
{
    class CampoObligatorio : ICustomFunctionOperator
    {
        static CampoObligatorio()
        {
            CampoObligatorio instance = new CampoObligatorio();
            if(CriteriaOperator.GetCustomFunction(instance.Name) == null)
            {
                CriteriaOperator.RegisterCustomFunction(instance);
            }
        }

        private object VerificarCampo(string campo, Producto prod) => prod != null &&
            prod.Campos.Any(c => c.Campo.Nombre == campo && c.Obligatorio);

        private object VerificarCampo(string campo, Persona per) 
        {
            CampoPersona cp = per.Session.FindObject<CampoPersona>(CriteriaOperator.Parse("Campo.Nombre = '" + campo + "' and Obligatorio = True"));
            return cp != null;
        }

        public object Evaluate(params object[] operands)
        {
            string campo = (string)operands[0];

            Type tipo = operands[1].GetType();

            object retorno = false;

            if (tipo == typeof(Producto))
                retorno = VerificarCampo(campo, (Producto)operands[1]);
            else
                retorno = VerificarCampo(campo, (Persona)operands[1]);

            return retorno == null ? false : retorno;
        }
        public static void Register()
        {
        }
        public Type ResultType(params Type[] operands) => typeof(bool);

        public string Name => nameof(CampoObligatorio);
    }
}
