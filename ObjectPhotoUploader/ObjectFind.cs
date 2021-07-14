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

        public override string ToString()
        {
            return string.Format("{0}.{1}.{2}.{3}.{4} - {5}",
                this.utm_hemisphere,
                this.utm_zone,
                this.area_utm_easting_meters,
                this.area_utm_northing_meters,
                this.context_number,
                this.find_number);
        }
    }

    public class ObjectFindData
    {
        public string utm_hemisphere { get; set; }
        public int utm_zone { get; set; }
        public int area_utm_easting_meters { get; set; }
        public int area_utm_northing_meters { get; set; }
        public int context_number { get; set; }
        public string material { get; set; }
        public string category { get; set; }
        public object director_notes { get; set; }

        public ObjectFindData(
            string utm_hemisphere,
            int utm_zone,
            int area_utm_easting_meters,
            int area_utm_northing_meters,
            int context_number,
            string material,
            string category,
            string director_notes
            )
        {
            this.utm_hemisphere = utm_hemisphere;
            this.utm_zone = utm_zone;
            this.area_utm_easting_meters = area_utm_easting_meters;
            this.area_utm_northing_meters = area_utm_northing_meters;
            this.context_number = context_number;
            this.material = material;
            this.category = category;
            this.director_notes = director_notes;

        }

    }

    public class ObjectFindRoot
    {
        public int count { get; set; }
        public string next { get; set; }
        public object previous { get; set; }
        public List<ObjectFind> results { get; set; }
    }



}
