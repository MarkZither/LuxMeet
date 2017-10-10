using System;

namespace LuxMeet.EFCore
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
