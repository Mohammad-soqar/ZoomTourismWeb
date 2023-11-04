using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZoomTourism.Models
{
    public class LeadDay
    {
        [SkipValidationForId]
        public int Id { get; set; }
        [ValidateNever]
        public string? Destination { get; set; }
        [ValidateNever]
        public int? numOfDays { get; set; }
        [ValidateNever]
        public int? LeadId { get; set; }
        [ValidateNever]
        public Lead? Lead { get; set; }

    }


    public class SkipValidationForIdAttribute : ValidationAttribute, IClientModelValidator
    {
        public override bool IsValid(object value)
        {
            var leadDay = value as LeadDay;
            if (leadDay != null && leadDay.Id == -1)
            {
                return true; // Skip validation for Id
            }
            return base.IsValid(value);
        }

        public void AddValidation(ClientModelValidationContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            MergeAttribute(context.Attributes, "data-val", "true");
            MergeAttribute(context.Attributes, "data-val-skipvalidationforid", GetErrorMessage());
        }

        private void MergeAttribute(IDictionary<string, string> attributes, string key, string value)
        {
            if (attributes.ContainsKey(key))
            {
                attributes[key] = value;
            }
            else
            {
                attributes.Add(key, value);
            }
        }

        public string GetErrorMessage()
        {
            return "Validation skipped for Id field with value -1.";
        }
    }
}
