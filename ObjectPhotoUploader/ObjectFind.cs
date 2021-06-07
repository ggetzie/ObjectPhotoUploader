using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ObjectPhotoUploader
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse); 
    public class Findphoto
    {
        public string photo_name { get; set; }
        public string photo_url { get; set; }
    }

    public class ObjectFind
    {
        public string id { get; set; }
        public string utm_hemisphere { get; set; }
        public int utm_zone { get; set; }
        public int area_utm_easting_meters { get; set; }
        public int area_utm_northing_meters { get; set; }
        public int context_number { get; set; }
        public int find_number { get; set; }
        public string material { get; set; }
        public string category { get; set; }
        public object director_notes { get; set; }
        public List<Findphoto> findphoto_set { get; set; }
    }

    public class ObjectFindRoot
    {
        public int count { get; set; }
        public string next { get; set; }
        public object previous { get; set; }
        public List<ObjectFind> results { get; set; }
    }



}
