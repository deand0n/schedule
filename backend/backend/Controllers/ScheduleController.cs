using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using backend.Models;
using backend.Services;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers
{
    [ApiController]
    [Route("api/schedule")]
    public class ScheduleController : Controller
    {
        
        private ITestClass _testClass;
        public ScheduleController(ITestClass testClass)
        {
            _testClass = testClass;
        }
        [HttpGet]
        public async Task<Day[]> Get()
        {
            // Encoding utf8 = Encoding.UTF8;
            // Encoding win1251 = Encoding.GetEncoding(1251);
            // string sss = values["group"];
            // byte[] utf8Bytes = utf8.GetBytes(sss.Substring(0, sss.IndexOf("-")));
            // byte[] win1251Bytes = Encoding.Convert(utf8, win1251, utf8Bytes);
            
            // string hex = BitConverter.ToString(win1251Bytes);
            // hex = hex.Replace("-", "%");
            // hex += sss.Substring(sss.IndexOf("-"), sss.Length-2);
            // hex = hex.Insert(0, "%");
            // Console.WriteLine(hex);
            // values["group"] = hex;
            // Console.WriteLine(values["group"]);
            // string encodedString = win1251.GetString(win1251Bytes);
            // values["group"] = encodedString;
            
            var content = new StringContent(
                "faculty=1004&teacher=&group=DS-1910&sdate=07.03.2021&edate=28.03.2021", Encoding.UTF8, "text/html");
            
             var responseString = await _testClass.PostDataAsync("http://195.95.232.162:8082/cgi-bin/timetable.cgi?n=700", content);

             var days = _testClass.ProcessData(responseString);

            return days;
        }
    }
}