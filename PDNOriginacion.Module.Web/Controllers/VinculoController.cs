using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Layout;
using DevExpress.ExpressApp.Model.NodeGenerators;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.ExpressApp.Templates;
using DevExpress.ExpressApp.Utils;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using PDNOriginacion.Module.BusinessObjects;

namespace PDNOriginacion.Module.Web.Controllers
{
    public partial class VinculoController : ViewController
    {
        public VinculoController()
        {
            InitializeComponent();
        }

        protected override void OnActivated()
        {
            base.OnActivated();
            ObjectSpace.ObjectSaving += ObjectSpace_ObjectSaving;
        }
        private void ObjectSpace_ObjectSaving(object sender, ObjectManipulatingEventArgs e)
        {
            if (e.Object == View.CurrentObject)
            {
                PersonaVinculo personaActual = (PersonaVinculo)View.CurrentObject;

                int tieneConyugue = personaActual.Vinculo.Vinculos.Where<PersonaVinculo>(p => p.Parentezco.Codigo == "CONYUGE").Count();

                if (tieneConyugue == 1 && personaActual.Parentezco.Codigo == "CONYUGE")
                {
                    throw new UserFriendlyException("La persona ya cuenta con un Vinculo Conyugue!");
                }

                if (personaActual.Vinculo != null && personaActual.Parentezco.Codigo == "CONYUGE")
                {                    
                    PersonaVinculo personaVinculada = new PersonaVinculo(personaActual.Session)
                    {
                        Persona = personaActual.Vinculo,
                        Vinculo = personaActual.Persona,
                        Parentezco = personaActual.Parentezco
                    };

                    if (personaVinculada.Persona.EstadoCivil == null || personaVinculada.Persona.EstadoCivil.Codigo != "C" || personaVinculada.Persona.EstadoCivil.Codigo != "B")
                    {
                        EstadoCivil estadoCivil = personaActual.Session.FindObject<EstadoCivil>(CriteriaOperator.Parse("Codigo = 'C'"));
                        personaVinculada.Persona.EstadoCivil = estadoCivil;
                    }

                    personaVinculada.Save();
                }
            }
        }
        protected override void OnViewControlsCreated()
        {
            base.OnViewControlsCreated();
        }
        protected override void OnDeactivated()
        {
            ObjectSpace.ObjectSaving -= ObjectSpace_ObjectSaving;
            base.OnDeactivated();
        }
    }
}
