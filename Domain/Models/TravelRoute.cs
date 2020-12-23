namespace Domain.Models
{
    public class TravelRoute
    {
        private int _value;

        public TravelRoute(Route route, int value)
        {
            Route = route;
            Value = value;
        }

        public Route Route { get; private set; }
        public int Value 
        { 
            get => _value; 
            private set 
            {  
                if (value >= 0)
                    _value = value;
            } 
        }
    }
}