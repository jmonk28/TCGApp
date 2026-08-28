using System.Runtime.InteropServices;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TCGApp.Server.Models
{
    public class CollectionWithCounts
    {
        public int CollectionID { get; set; }
        public string CollectionName { get; set; }
        public string CollectionType { get; set; }
        public int CardCount { get; set; }
        public int IsBase { get; set; }
        public int TCGUserID { get; set; }
    }

}
