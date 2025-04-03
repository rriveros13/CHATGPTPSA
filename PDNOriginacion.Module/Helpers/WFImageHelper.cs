using MetadataExtractor;
using MetadataExtractor.Formats.Exif;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDNOriginacion.Module.Helpers
{
    public static class WFImageHelper
    {
        public static GeoLocation GetGeoLocation(ref MemoryStream ms)
        {
            try
            {
                var gps = ImageMetadataReader.ReadMetadata(ms)
                            .OfType<GpsDirectory>()
                            .FirstOrDefault();

                GeoLocation location = gps.GetGeoLocation();
                return location;
            }
            catch
            {
                return null;
            }
            
           
        }

        public static GeoLocation GetGeoLocation(byte[] imagen)
        {
            try
            {
                MemoryStream ms = new MemoryStream(imagen);
                var gps = ImageMetadataReader.ReadMetadata(ms)
                            .OfType<GpsDirectory>()
                            .FirstOrDefault();

                GeoLocation location = gps.GetGeoLocation();
                return location;
            }
            catch
            {
                return null;
            }
        }

        public static GeoLocation GetGeoLocation(string path)
        {
            try
            {
                var gps = ImageMetadataReader.ReadMetadata(path)
                            .OfType<GpsDirectory>()
                            .FirstOrDefault();

                GeoLocation location = gps.GetGeoLocation();
                return location;
            }
            catch
            {
                return null;
            }
           
        }
    }
}
