using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace XLantCore.Models
{
    public partial class DataMap
    {
        public int Id { get; set; }
        [Display(Name="Xlant Object")]
        public string ClassName { get; set; }
        [Display(Name="CSVFile")]
        public string SourceName { get; set; }
        [Display(Name="Xlant Field Name")]
        public string InternalFieldName { get; set; }
        [Display(Name="CSV Column Header")]
        public string ExternalFieldName { get; set; }
    }
}
