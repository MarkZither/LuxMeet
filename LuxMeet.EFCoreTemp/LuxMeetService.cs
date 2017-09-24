using System;

namespace LuxMeet.EFCoreTemp
{
    public class LuxMeetService
    {
        private LuxMeetContext _context;

        public LuxMeetService(LuxMeetContext context)
        {
            _context = context;
        }
    }
}
