using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Domains.ViewModels
{
    
    public interface IReCaptchaClass
    {
        string Validate(string EncodedResponse);
    }
    public class ReCaptchaClass : IReCaptchaClass
    {
        private readonly IConfiguration _configuration;
        public ReCaptchaClass(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public string Validate(string EncodedResponse)
        {
            var client = new System.Net.WebClient(); 
            var PrivateKey = _configuration.GetSection("reCaptcha").GetSection("SecretKey").Value; 
            var GoogleReply = client.DownloadString(string.Format(_configuration.GetSection("reCaptcha").GetSection("RecaptchaSiteVerifyURL").Value, PrivateKey, EncodedResponse)); 
            var captchaResponse = JsonConvert.DeserializeObject<ReCaptchaClass>(GoogleReply); 
            return captchaResponse.Success.ToLower();
        }


        [JsonProperty("success")]
        public string Success
        {
            get { return m_Success; }
            set { m_Success = value; }
        }

        private string m_Success;


        [JsonProperty("error-codes")]

        public List<string> ErrorCodes
        {
            get { return m_ErrorCodes; }
            set { m_ErrorCodes = value; }
        }

        private List<string> m_ErrorCodes;

       
    }
}
