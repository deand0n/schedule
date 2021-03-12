using System;
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
            
            // regex for day (div's around tables)
            var dayRegex = new Regex("<div class=\"col-md-6\">(.*?)</table></div>");
            // regex for date (xx.xx.xxxx)
            var dateRegex = new Regex("<h4>(.*?) <small>");
            // regex for day of the week 
            var weekDayRegex = new Regex("<small>(.*?)</small>");
            // regex for lessons in the day
            var lessonRegex = new Regex("<tr>(.*?)</tr>");
            
            var ordinalNumberRegex = new Regex(@"<td>(.?\d)</td>");
            var startTimeRegex = new Regex(@"<td>([0-9]{2}\:[0-9]{2})<br>");
            var endTimeRegex = new Regex(@"<br>([0-9]{2}\:[0-9]{2})</td>");
            var nameRegex = new Regex(@"[0-9]{2}\:[0-9]{2}</td><td>(.*?)</td></tr>");
            
            
            
            var dayMatches = dayRegex.Matches(responseString);
            int dayCounter = dayMatches.Count;
            Day[] days = new Day[dayCounter];
            int[] lessonsCounter = new int[dayCounter];
            string tables = "";
            
            for (int i = 0; i < dayCounter; i++)
            {
                tables += dayMatches[i].Value;
            }
            
            for (int i = 0; i < dayCounter; i++)
            {
                days[i] = new Day();
                days[i].Lessons = new List<Lesson>();
                lessonsCounter[i] = lessonRegex.Matches(dayMatches[i].Value).Count;
                for (int j = 0; j < lessonsCounter[i]; j++)
                {
                    days[i].Lessons.Add(new Lesson());
                }
            }
            //TODO refactor this mess
            
            for (int i = 0; i < dayCounter; i++)
            {
                days[i].Date = dateRegex.Matches(tables)[i].Groups[1].Value;
                days[i].DayOfTheWeek = weekDayRegex.Matches(tables)[i].Groups[1].Value;
                for (int j = 0; j < lessonsCounter[i]; j++)
                {
                    days[i].Lessons[j].OrdinalNumber = ordinalNumberRegex.Matches(dayMatches[i].Value)[j].Groups[1].Value;
                    days[i].Lessons[j].StartTime = startTimeRegex.Matches(dayMatches[i].Value)[j].Groups[1].Value;
                    days[i].Lessons[j].EndTime = endTimeRegex.Matches(dayMatches[i].Value)[j].Groups[1].Value;
                    
                    string tempName = nameRegex.Matches(dayMatches[i].Value)[j].Groups[1].Value;
                    string tempType, tempGroups;
                    if (tempName == " ")
                    {
                        tempName = null;
                        tempType = null;
                        tempGroups = null;
                    }
                    else
                    {
                        Regex typeRegex = new Regex(@"\((.*?)\)<br> ");
                        tempType = typeRegex.Match(tempName).Groups[1].Value;
                        
                        tempName = tempName.Replace(typeRegex.Match(tempName).Value, "");
                        tempName = tempName.Replace("\u00A0", " ");
                        tempName = tempName.Replace("<br>  <div class='link'> </div> ", "");

                        tempGroups = "DS-1910";
                        if (tempName.Contains("<br>"))
                        {
                            Regex groupsRegex = new Regex("<br> (.*?)<br> <div class='link'> </div>");
                            tempGroups = groupsRegex.Match(tempName).Groups[1].Value;

                            tempName = tempName.Replace(groupsRegex.Match(tempName).Value, "");
                        }
                        
                    }
                    days[i].Lessons[j].Name = tempName;
                    days[i].Lessons[j].Type = tempType;
                    days[i].Lessons[j].Groups = tempGroups;
                }
            }
            return days;
        }
    }
}