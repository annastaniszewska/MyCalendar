using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;
using System.Web.Mvc;
using Microsoft.Ajax.Utilities;
using MyCalendar.Controllers;
using Type = MyCalendar.Core.Models.Type;

namespace MyCalendar.Core.ViewModels
{
    public class CycleEventFormViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Please enter start date.")]
        [ValidDate]
        public string StartDate { get; set; }
        
        [ValidDateExceptEmpty]
        public string EndDate { get; set; }

        [Required(ErrorMessage = "Please provide type.")]
        public byte Type { get; set; }

        public IEnumerable<Type> Types { get; set; }
        
        public string Time { get; set; }

        public string Heading { get; set; }

        public string Action
        {
            get
            {
                Expression<Func<CycleEventsController, ActionResult>> update = 
                    (c => c.Update(this));

                Expression<Func<CycleEventsController, ActionResult>> create = 
                    (c => c.Create(this));

                var action = Id != 0 ? update : create;
                var actionName = (action.Body as MethodCallExpression).Method.Name;

                return actionName;
            }
        }

        public DateTime GetStartDate()
        {
            return DateTime.Parse($"{StartDate} {Time}");
        }

        public DateTime GetEndDate(int days)
        {
            return EndDate.IsNullOrWhiteSpace() ? DateTime.Parse($"{StartDate}").AddDays(days) : DateTime.Parse($"{EndDate}");
        }
    }
}