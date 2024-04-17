namespace EventApi.Model
{
    public class Event
    {
        public int Id { get; set; }

        public string Attendees { get; set; }

        public string Nome_Evento { get; set; }
        
        public DateTime Data_Evento { get; set; }
    }
}
