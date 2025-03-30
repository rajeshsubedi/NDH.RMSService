using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.Models.DataModels.HomepageManagementModels
{
    public class HomepageSpecialGroups
    {
        public Guid GroupId { get; set; }                 
        public string GroupName { get; set; }            
        public string GroupDescription { get; set; }    
        public string GroupType { get; set; }            
        public DateTime StartDate { get; set; }         
        public DateTime? EndDate { get; set; }          
        public string Status { get; set; }               
        public DateTime CreatedAt { get; set; }          
        public DateTime UpdatedAt { get; set; }           
        public int? CreatedBy { get; set; }             
        public int? UpdatedBy { get; set; }              
        public string ImageUrl { get; set; }
    }
}
