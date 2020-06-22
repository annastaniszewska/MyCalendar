﻿using MyCalendar.Controllers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;
using System.Web.Mvc;
using Type = MyCalendar.Models.Type;

namespace MyCalendar.ViewModels
{
    public class CycleEventFormViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Please enter start date.")]
        [ValidDate]
        public string StartDate { get; set; }

        [Required(ErrorMessage = "Please enter end date.")]
        [ValidDate]
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

        public DateTime GetEndDate()
        {
            return DateTime.Parse($"{EndDate}");
        }
    }
}