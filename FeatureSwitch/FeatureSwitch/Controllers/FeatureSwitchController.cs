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

            FeatureAccess featureAccess = new FeatureAccess();
            featureAccess.Email = "sydhsn.sh@gmail.com";
            featureAccess.FeatureName = "Profile";
            featureAccess.Enable = true;

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
