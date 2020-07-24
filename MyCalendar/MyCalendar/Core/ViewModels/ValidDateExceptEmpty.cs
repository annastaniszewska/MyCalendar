namespace MyCalendar.Core.ViewModels
{
    public class ValidDateExceptEmpty : ValidDate
    {
        public override bool IsValid(object value)
        {
            if (value != null)
            {
                return base.IsValid(value);
            }
            else
            {
                return true;
            }
        }
    }
}