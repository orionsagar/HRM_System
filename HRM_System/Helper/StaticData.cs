using Application.Tasks.Queries.QCompany;
using Domains.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc.Rendering;
using Persistence.Repository;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace UKHRM.Helper
{
    public class StaticData
    {
        public static string HostPath = "https://workerpermituk.ultimateit-bd.com";

        public readonly static List<string> Gender = new()
        {
            "Male",
            "Female",
            "Others"
        };


        public readonly static List<string> BloodGroup = new()
        {
            "N/A",
            "A+",
            "A-",
            "B+",
            "B-",
            "AB+",
            "AB-",
            "O+",
            "O-",
        };
        
        public readonly static List<string> Religion = new()
        {
            "N/A",
            "Islam",
            "Hindu",
            "Buddist",
            "Cristian",
            
        };
        
    }
    





}
