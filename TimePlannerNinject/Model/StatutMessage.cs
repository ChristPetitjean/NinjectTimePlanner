using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimePlannerNinject.Model
{
   using GalaSoft.MvvmLight.Messaging;

   public static class StatutMessage
   {
      public const string Token = "tokenStatutMessage";
      public static void SendStatutMessage(string message)
      {
         Messenger.Default.Send(message, Token);
      }
   }
}
