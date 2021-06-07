using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ObjectPhotoUploader
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse); 
    public class ContextphotoSet
    {
        public string thumbnail_url { get; set; }
        public string photo_url { get; set; }
    }

    public class BagphotoSet
    {
        public string thumbnail_url { get; set; }
        public string photo_url { get; set; }
    }

    public class SpatialContext
    {
        public string id { get; set; }
        public string utm_hemisphere { get; set; }
        public int utm_zone { get; set; }
        public int area_utm_easting_meters { get; set; }
        public int area_utm_northing_meters { get; set; }
        public int context_number { get; set; }
        public string type { get; set; }
        public string opening_date { get; set; }
        public string closing_date { get; set; }
        public string description { get; set; }
        public string director_notes { get; set; }
        public List<ContextphotoSet> contextphoto_set { get; set; }
        public List<BagphotoSet> bagphoto_set { get; set; }

        public override string ToString()
        {
            return string.Format("{0}.{1}.{2}.{3}.{4}", 
                utm_hemisphere, utm_zone, area_utm_easting_meters, area_utm_northing_meters, context_number);
        }

    }



}
