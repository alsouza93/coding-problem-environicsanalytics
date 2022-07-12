using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net.Http;

namespace CodingProblem.Infra
{
    public class PostalCodeRepository
    {
        private readonly HttpClient httpClient;
        public PostalCodeRepository()
        {
            httpClient = new HttpClient();
        }

        public int GetSegmentCode(string postalCode)
        {
            string url = string.Concat("https://prizm.environicsanalytics.com/api/pcode/get_segment?postal_code=", postalCode);


            var apiresult = httpClient.GetAsync(url).Result;


            if (apiresult.IsSuccessStatusCode)
            {
                var response = JsonConvert.DeserializeObject<PostalCodeDto>(apiresult.Content.ReadAsStringAsync().Result);

                if (response.Format == "unique")
                {
                    return Convert.ToInt32(response.Data);
                }

                if (response.Format == "multi")
                {
                    var jArray = response.Data as JArray;
                    List<PostalCodeMultiDto> aa = jArray.ToObject<List<PostalCodeMultiDto>>();
                    return aa[0].prizm_id;
                }
            }

            return 0;
        }
    }
}