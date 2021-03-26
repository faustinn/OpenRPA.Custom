using System;
using System.IO;
using System.Activities;

namespace OpenRPA.Custom.Activities
{
    public class LogMessage : CodeActivity
    {
        [RequiredArgument]
        public InArgument<string> Text { get; set; }
        [RequiredArgument]
        public InArgument<String> LogFileName { get; set; }

        protected override void Execute(CodeActivityContext context)
        {
            try
            {
                string DataLogText = System.Environment.NewLine + "[" + DateTime.Now.ToString("dd-MM-yyyy hh:mm:ss") + "] ";
                File.AppendAllText(LogFileName.Get(context), DataLogText + Text.Get(context));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }
    }
}
