using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Happy.Scaffolding.MVC.Models
{
    public enum euColumnType
    {
        /// <summary>
        /// string
        /// </summary>
        stringCT
        ,
        /// <summary>
        /// DateTime
        /// </summary>
        datetimeCT
        ,
        /// <summary>
        /// int
        /// </summary>
        intCT
        ,
        /// <summary>
        /// decimal
        /// </summary>
        decimalCT
        ,
        /// <summary>
        /// RelatedModel
        /// </summary>
        RelatedModel
    }
}
