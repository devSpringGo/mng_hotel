namespace ManageHotel.ViewModels
{
    public class BookingEvent
    {
        public string RoomName { get; set; }
        public string Summary { get; set; }
        public string Description { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public string Uid { get; set; }
        public DateTime? Created { get; set; }
    }

}
