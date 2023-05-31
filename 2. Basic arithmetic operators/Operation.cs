using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2.Basic_arithmetic_operators
{
    public class Operation
    {
        /// <summary>
        /// The left side of the operation
        /// </summary>
        public string LeftSide { get; set; }

        /// <summary>
        /// The right side of the operation
        /// </summary>

        public string RightSide { get; set; }

        /// <summary>
        /// The operator type
        /// </summary>

        public OperationType OperationType { get; set; }
        public Operation()
        {
            this.LeftSide = string.Empty;
            this.RightSide = string.Empty;
        }
        /// <summary>
        /// Saves the values currently parsed to allow for parentheses to be resolved
        /// </summary>
        public string SavedValue { get; set; }        

        public OperationType SavedOperation { get; set; }   
    }
}
