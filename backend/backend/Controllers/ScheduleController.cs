using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers
{
    [ApiController]
    [Route("api/test")]
    public class ScheduleController : Controller
    {
        // TODO move this variable from here
        private static readonly HttpClient client = new HttpClient();
        
        
        [HttpGet]
        public async Task<string> Get()
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            
            var values = new Dictionary<string, string>
            {
                { "faculty", "1004" },
                {"group", "DS-1910"},
                {"sdate", "01.03.2021"},
                {"edate","07.03.2021"}
            };

            var content = new FormUrlEncodedContent(values);

            // send post request to timetable and save it to "response"
            var response =  await client.PostAsync("http://195.95.232.162:8082/cgi-bin/timetable.cgi?n=700", content);

            // encode response using "windows 1251"
            var buffer = await response.Content.ReadAsByteArrayAsync();
            var byteArray = buffer.ToArray();
            var responseString = Encoding.GetEncoding(1251).GetString(byteArray, 0, byteArray.Length);

            
            return responseString;
        }
    }
}