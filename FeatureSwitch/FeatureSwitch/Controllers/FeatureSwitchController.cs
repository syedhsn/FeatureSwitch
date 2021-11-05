using FeatureSwitch.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace FeatureSwitch.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class FeatureSwitchController : Controller
    {
        private readonly FeatureSwitchContext _context;

        public FeatureSwitchController(FeatureSwitchContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<Access>> GetResponse(string email, string featureName)
        {
            if (string.IsNullOrEmpty(email) && string.IsNullOrEmpty(featureName))
            {
                return NotFound();
            }

            var isAccessValid = await _context.FeatureAccess.Where(x => x.Email == email && x.FeatureName == featureName).ToListAsync();

            if (isAccessValid.Count == 0)
            {
                return NotFound();
            }
            else
            {
                if (isAccessValid.FirstOrDefault().Enable == true)
                {
                    return new Access { canAccess = true };
                }
                else
                {
                    return new Access { canAccess = false };
                }
            }
        }

        [HttpPost]
        public async Task<ActionResult<FeatureAccess>> PostResponse()
        {
            var url = "https://localhost:44340/POST/feature";

            var httpRequest = (HttpWebRequest)WebRequest.Create(url);
            httpRequest.Method = "POST";

            httpRequest.Accept = "application/json";
            httpRequest.ContentType = "application/json";

            var data = @"{
                    ""Email"": ""sydhsn.sh@gmail.com"",
                    ""FeatureName"": Profile,
                    ""Enable"": true
                    }";

            FeatureAccess featureAccess = new FeatureAccess();
            Dictionary<string, string> featureAccessJson = JsonConvert.DeserializeObject<Dictionary<string, string>>(data);
            foreach(var x in featureAccessJson)
            {
                
                if(x.Key == "Email")
                {
                    featureAccess.Email = x.Value;
                }
                if (x.Key == "FeatureName")
                {
                    featureAccess.FeatureName = x.Value;
                }
                if (x.Key == "Enable")
                {
                    featureAccess.Enable = Boolean.Parse(x.Value);
                }
                
            }

            try
            {
                _context.FeatureAccess.Add(featureAccess);
                await _context.SaveChangesAsync();

                return StatusCode(200);
            }
            catch (Exception ex)
            {
                return StatusCode(304);

            }
        }
    }
}
