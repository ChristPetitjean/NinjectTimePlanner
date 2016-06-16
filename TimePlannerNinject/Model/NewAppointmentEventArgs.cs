using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimePlannerNinject.Model
{
   public struct NewAppointmentEventArgs
   {
      public DateTime? StratDate;

      public DateTime? EndDate;

      public int? CandidateId;

      public int? RequirementId;
   }
}
