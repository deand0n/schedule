using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using backend.Models;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers
{
    [ApiController]
    [Route("api/schedule")]
    public class ScheduleController : Controller
    {
        // TODO move this variable from here
        private static readonly HttpClient Client = new HttpClient();
        
        [HttpGet]
        public async Task<Day[]> Get()
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            
            var values = new Dictionary<string, string>
            {
                { "faculty", "1004" },
                {"group", "DS-1910"},
                {"sdate", "01.03.2021"},
                {"edate","28.03.2021"}
            };

            var content = new FormUrlEncodedContent(values);

            // send POST request to timetable and save it to "response"
            var response =  await Client.PostAsync("http://195.95.232.162:8082/cgi-bin/timetable.cgi?n=700", content);

            // encode response using "windows 1251"
            var buffer = await response.Content.ReadAsByteArrayAsync();
            var byteArray = buffer.ToArray();
            var responseString = Encoding.GetEncoding(1251).GetString(byteArray, 0, byteArray.Length);
            
            // regex for tables
            Regex tableRegex = new Regex("<div class=\"col-md-6\">(.*?)</div>");
            // regex for date
            Regex dateRegex = new Regex("<h4>(.*?) <small>");
            // regex for day of the week 
            Regex weekDayRegex = new Regex("<small>(.*?)</small>");
            
            MatchCollection matches = tableRegex.Matches(responseString);
            int counter = tableRegex.Matches(responseString).Count;  
            
            Day[] days = new Day[counter];
            string tables = "";
            int j = 0;
            
            foreach (Match match in matches)
            {
                tables += match.Value;
                days[j] = new Day();
                j++;
            }

            for (int i = 0; i < counter; i++)
            {
                days[i].Date = dateRegex.Matches(tables)[i].Groups[1].Value;
                days[i].DayOfTheWeek = weekDayRegex.Matches(tables)[i].Groups[1].Value;
            }
            return days;
        }
    }
}