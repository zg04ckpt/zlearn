using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Common
{
    public class ExpressionFilter
    {
        public string? Property {  get; set; }
        public object? Value { get; set; }
        public Comparison Comparison { get; set; }
    }

    //enum as comparison
    public enum Comparison
    {
        [Display(Name = "==")]
        Equal,

        [Display(Name = "<")]
        LessThan,

        [Display(Name = ">")]
        GreaterThan,

        [Display(Name = ">=")]
        GreaterThanOrEqual,

        [Display(Name = "<=")]
        LessThanOrEqual,

        [Display(Name = "!=")]
        NotEqual,

        [Display(Name = "Contains")]
        Contains,

        [Display(Name = "StartsWith")]
        StartsWith, 

        [Display(Name = "EndsWith")]
        EndsWith, 
    }
}
