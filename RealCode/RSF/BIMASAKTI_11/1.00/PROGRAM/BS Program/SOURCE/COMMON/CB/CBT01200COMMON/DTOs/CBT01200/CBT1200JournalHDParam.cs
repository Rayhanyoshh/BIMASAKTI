using CBT01200Common.DTOs;

namespace CBT01200Common.DTOs
{
    public class CBT1200JournalHDParam : CBT01200TransHDRecordDTO
    {
        public string CUSER_ID { get; set; }
        public string CLANGUAGE_ID { get; set; }
        public string CACTION { get; set; }
    }
}