using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace XLantCore.Models
{
    public enum PlanStatus
    {
        [Display(Name="In Force")]
        InForce,
        [Display(Name="Submitted")]
        SubmittedToProvider,
        Draft,
        [Display(Name="Out Of Force")]
        OutOfForce,
        Unknown
    }
}
