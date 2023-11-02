using Repo_PMS.Models;
using Repo_PMS.Models.ModelMyAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Repo_PMS.Repository
{
    internal class RepoMYAPI
    {
        private String baseurl = "";
        private ContextPMS _context = new ContextPMS();
        public async Task<user> Login(Login lgn)
        {
            user? VTMD = new user(); 
            using(HttpClient MyApiClient = new HttpClient())
            {
                string endpoint = baseurl + @"MyApi/Get";
                HttpContent body = new StringContent(JsonConvert.SerializeObject(lgn), Encoding.UTF8,"Application/json");
                var request = await MyApiClient.PostAsync(endpoint, body);     

                if(request.IsSuccessStatusCode) 
                {
                    var response = await request.Content.ReadAsStringAsync();
                    VTMD = JsonConvert.DeserializeObject<user>(response);   
                }    
            } 
            
            if(VTMD.response == 200)
            {
                if(VTMD.name != null) 
                {
                    VTMD.role = _context.RoleDetails.FirstOrDefault(r => r.RoleId == Convert.ToInt32(VTMD.role)).RoleName;
                    VTMD.udeg = _context.deginations.FirstOrDefault(r => r.Id == Convert.ToInt32(VTMD.role)).DeginationName;
                    VTMD.udept = _context.departments.FirstOrDefault(r => r.Id == Convert.ToInt32(VTMD.udept)).DeptName;
                }  
            }
            return VTMD;    
        }
        public async Task<VmTupple> Createuser(UserDetail user)
        {
            VmTupple VMTD = new VmTupple(); 
            using(HttpClient MyApiclient = new HttpClient()) 
            {
                string endpoint = baseurl + @"MyApi/Register";
                HttpContent body = new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json");
                var request = await MyApiclient.PostAsync(endpoint, body);
                if(request.IsSuccessStatusCode) 
                {
                    var response = await request.Content.ReadAsStringAsync();       
                    VMTD = JsonConvert.DeserializeObject<VmTupple>(response);   
                }

            }
            return (VMTD);    
        }
    }
}
