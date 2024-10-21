using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domains.Models
{
    public class PageHeaderProps
    {
        public string PageTitle { get; set; }
        public string PageDesc { get; set; }
        public string PageCreateLinkName { get; set; }
        public string IsAdd { get; set; }
        public string PageCreateLink { get; set; }
        public string[] BreadcrumbText { get; set; }
        public string[] BreadcrumbLink { get; set; }
    }



}
