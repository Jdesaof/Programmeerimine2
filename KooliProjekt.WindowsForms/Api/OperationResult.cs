using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace KooliProjekt.WindowsForms
{
    [ExcludeFromCodeCoverage]
    public class OperationResult
    {
        public Dictionary<string, string> PropertyErrors { get; set; }
        public List<string> Errors { get; set; }

        public bool HasErrors 
        {
            get
            {
                return PropertyErrors?.Count > 0 ||
                       Errors?.Count > 0;
            }
        }

        public bool ShouldSerializeHasErrors()
        {
            return HasErrors;
        }

        public OperationResult AddError(string error)
        {
            if (Errors == null)
            {
                Errors = new List<string>();
            }

            Errors.Add(error);

            return this;
        }

        public OperationResult AddPropertyError(string property, string error)
        {
            if (PropertyErrors == null)
            {
                PropertyErrors = new Dictionary<string, string>();
            }
            else if (PropertyErrors.ContainsKey(property))
            {
                return this;
            }

            PropertyErrors.Add(property, error);

            return this;
        }
    }
}