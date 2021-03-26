using System;
using System.Activities;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace OpenRPA.Custom.Activities
{
    public class ExecuteStoredProcedure : CodeActivity
    {
        [RequiredArgument]
        public InArgument<string> ConnectionString { get; set; }
        [RequiredArgument]
        public InArgument<string> Command { get; set; }

        public InArgument<Dictionary<string, object>> Arguments { get; set; }

        protected override void Execute(CodeActivityContext context)
        {
            using (SqlConnection con = new SqlConnection(ConnectionString.Get(context)))
            {
                using (SqlCommand cmd = new SqlCommand(Command.Get(context), con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    Dictionary<string, object> Args = Arguments.Get(context);

                    foreach (KeyValuePair<string, object> arg in Args)
                    {
                        cmd.Parameters.AddWithValue(arg.Key, arg.Value);
                    }

                    con.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
