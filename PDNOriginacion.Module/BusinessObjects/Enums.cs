namespace PDNOriginacion.Module.BusinessObjects
{
    public enum MonedaEnum : int
    {
        GS, USD, EUR
    }

;

    public enum PropietarioInquilino : int
    {
        PROPIETARIO, INQUILINO
    }

;

    public enum OrigenGeolocalizacion
    {
        GPS = 0,
        MANUAL = 1,
        FOTO = 2
    }

    public enum FormaDeSeguiminetoPreferida
    {
        TELPERSONALLLAMADA = 0,
        TELPERSONALWHATSAPP = 1,
        TELLABORALLLAMADA = 2,
        TELLABORALWHATSAPP = 3,
        CORREOPARTICULAR = 4,
        CORREOLABORAL = 5,
        OTRO = 6
    }
}

