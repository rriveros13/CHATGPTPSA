using DevExpress.Xpo;
using IntegracionITGF.ITGFDataModel;
using Shared;
using System;

namespace IntegracionITGF.DataAccess
{
    public class ITGFAccess : IIntegracion
    {
        private Session SessionIT;

        public OpeRespuesta CreatePersona(Object PerSolicitudOri)
        {
            OpeRespuesta resp = new OpeRespuesta();
            try
            {
                if (this.SessionIT == null || !this.SessionIT.IsConnected)
                {
                    ConnectionHelper.Connect(DevExpress.Xpo.DB.AutoCreateOption.None);
                    SessionIT = new Session();
                }

                SessionIT.BeginTransaction();
                Personas.CreatePersona((PDNOriginacion.Module.BusinessObjects.SolicitudPersona)PerSolicitudOri, SessionIT);
                SessionIT.CommitTransaction();
            }
            catch (Exception e)
            {
                SessionIT.RollbackTransaction();
                resp.Error = true;
                resp.Mensaje = e.Message;
                resp.Excepcion = e;
            }
            return resp;
        }

        public OpeRespuesta CreateSolicitud(Object SolicitudOri)
        {
            OpeRespuesta resp = new OpeRespuesta();
            try
            {
                if (this.SessionIT == null || this.SessionIT.IsConnected || !this.SessionIT.IsConnected)
                {
                    ConnectionHelper.Connect(DevExpress.Xpo.DB.AutoCreateOption.None);
                    SessionIT = new Session();
                }

                //SessionIT.BeginTransaction();
                Solicitudes.CreateSolicitud((PDNOriginacion.Module.BusinessObjects.Solicitud)SolicitudOri, SessionIT);
                //SessionIT.CommitTransaction();
            }
            catch (Exception e)
            {
                SessionIT.ExplicitRollbackTransaction();
                resp.Error = true;
                resp.Mensaje = e.Message;
                resp.Excepcion = e;
            }
            return resp;
        } 

        ~ITGFAccess()
        {
            //this.SessionIT.Connection.Close();
        }

    }
}
